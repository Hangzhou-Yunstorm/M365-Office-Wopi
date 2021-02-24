#!/bin/bash

setenforce 0
sed 's/^SELINUX=.*$/SELINUX=permissive/g' -i /etc/sysconfig/selinux

echo 'Please wait until nginx configuration to complete...'
#yum install -y epel-release &>/dev/null
#yum clean all && yum makecache &>/dev/null
#yum install -y nginx &>/dev/null
sed '38,57s/^/#/g' -i /etc/nginx/nginx.conf
bk=backend
bks="upstream ${bk} {
"
oldIFS="$IFS"
IFS=','
read -r -p 'Please enter other NLB IP(ip1,ip2... ):  ' -a ips
IFS="$oldIFS"
bks="upstream ${bk} {
    server localhost:8080;
"
for ip in "${ips[@]}"; do
    bks="${bks}    server $ip;
"
done
bks="${bks}}
"
read -r -n 1 -p 'Is https?[y/n]: ' https_p
https_p=${https_p,,}
echo
if [ x"$https_p" = x"y" ]; then
    read -r -p 'SSL certificat file full path: ' ssl
    read -r -p 'SSL certificat key file full path: ' ssl_key
    cat <<EOF > /etc/nginx/conf.d/upstream.conf
${bks}
server {
    listen 443;
    server_name _;
    ssl on;
    ssl_certificate  ${ssl};
    ssl_certificate_key ${ssl_key};
    ssl_session_timeout 5m;
    ssl_ciphers ECDHE-RSA-AES128-GCM-SHA256:ECDHE:ECDH:AES:HIGH:!NULL:!aNULL:!MD5:!ADH:!RC4;
    ssl_protocols TLSv1 TLSv1.1 TLSv1.2;
    ssl_prefer_server_ciphers on;
    location / {
        proxy_pass http://${bk};
        proxy_set_header Host \$host;
        client_max_body_size 1024m;
    }
}

server {
    listen 80;
    server_name _;
    rewrite ^(.*)\$ https://\$host\$1 permanent;
}

server {
    listen 808;
    server_name _;
    location / {
        proxy_pass http://localhost:8080;
        proxy_set_header Host \$host;
        client_max_body_size 1024m;
    }  
}
EOF
else
    cat <<EOF > /etc/nginx/conf.d/upstream.conf
${bks}
server {  
    listen 80;
    server_name localhost;
    location / {
        proxy_pass http://${bk};
        proxy_set_header Host \$host;
        client_max_body_size 1024m;
    }  
}

server {
    listen 808;
    server_name _;
    location / {
        proxy_pass http://localhost:8080;
        proxy_set_header Host \$host;
        client_max_body_size 1024m;
    }  
}
EOF
fi

echo 'Please wait until nginx started...'
systemctl start nginx
systemctl enable nginx
setsebool -P httpd_can_network_connect 1

echo 'please wait until keepalived configuration to complete...'
#yum install -y keepalived &>/dev/null
cat <<EOF > /etc/keepalived/ngx_chk.sh
#!/bin/bash
d=\`date --date today +%Y%m%d_%H:%M:%S\`
n=\`ps -C nginx --no-heading|wc -l\`
if [ \$n -eq "0" ]; then
    systemctl start nginx
    n2=\`ps -C nginx --no-heading|wc -l\`
    if [ \$n2 -eq "0"  ]; then
        echo "\$d nginx down,keepalived will stop" >> /var/log/check_ng.log
        systemctl stop keepalived
    fi
fi
EOF
chmod +x /etc/keepalived/ngx_chk.sh
read -r -p 'Please enter NLB VIP: ' vip
read -r -n 1 -p 'Is master node?[y/n]: ' master_p
master_p=${master_p,,}
echo
state="BACKUP"
priority=100
if [ x"$master_p" = x"y" ]; then
    state="MASTER"
    priority=$((priority+1))
fi
vipid=$(echo $vip | awk -F. '{print $4}')
nic=$(ip link show | grep -iv loopback | head -n 1 | awk '{print $2}' | awk -F: '{print $1}')
chkname=nginx-chk
#chkcmd="\"/bin/curl -i localhost &> /dev/null\""
chkcmd="\"/etc/keepalived/ngx_chk.sh\""
cat <<EOF > /etc/keepalived/keepalived.conf
vrrp_script ${chkname} {
    script ${chkcmd}
    interval 5
    timeout 5
    weight 10
    fall 2
    rise 1
    user nobody
}
vrrp_instance ${HOSTNAME} {
    state ${state}
    priority ${priority}
    preempt
    interface ${nic}
    virtual_router_id ${vipid}
    virtual_ipaddress {
        ${vip}
    }
    track_script {
        ${chkname}
    }
}
EOF
systemctl start keepalived
systemctl enable keepalived

sudo firewall-cmd --direct --permanent --add-rule ipv4 filter INPUT 0 --in-interface ${nic} --destination 224.0.0.18 --protocol vrrp -j ACCEPT
sudo firewall-cmd --reload

echo 'Please wait until docker configuration to complete...'
#yum install -y yum-utils device-mapper-persistent-data lvm2 &>/dev/null
#yum-config-manager --add-repo http://mirrors.aliyun.com/docker-ce/linux/centos/docker-ce.repo &>/dev/null
#yum clean all && yum make cache &>/dev/null
#yum -y install docker-ce &>/dev/null
systemctl start docker
systemctl enable docker

sudo firewall-cmd --permanent --zone=public --add-service=http 
sudo firewall-cmd --permanent --zone=public --add-service=https
sudo firewall-cmd --zone=public --add-port=80/tcp --permanent
sudo firewall-cmd --zone=public --add-port=808/tcp --permanent
sudo firewall-cmd --zone=public --add-port=443/tcp --permanent
sudo firewall-cmd --reload
#systemctl restart firewalld

#echo "Please wait until docker image pulled & running..."
 read -r -p 'Please enter docker image name: ' image_name

#docker pull zhangc1128/officewopi:8080
docker run --name officewopi8080 -d --net host -p 8080:8080 --restart=always ${image_name}
