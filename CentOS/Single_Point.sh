#!/bin/bash

bk=backend
bks="upstream ${bk} {
    server localhost:8080;
}
"
echo 'Please wait until nginx installed...'

yum install -y epel-release &>/dev/null
yum clean all && yum makecache &>/dev/null
yum install -y nginx &>/dev/null
sed '38,57s/^/#/g' -i /etc/nginx/nginx.conf

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
        index index.html index.htm;
        client_max_body_size 1024m;
    }
}
server {
    listen 80;
    server_name _;
    rewrite ^(.*)\$ https://\$host\$1 permanent;
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
        index index.html index.htm;
        client_max_body_size 1024m;
    }  
}
EOF
fi

echo 'Please wait until nginx started...'
systemctl start nginx
systemctl enable nginx
setsebool -P httpd_can_network_connect 1

echo 'Please wait until docker installed...'
yum install -y yum-utils device-mapper-persistent-data lvm2 &>/dev/null
yum-config-manager --add-repo http://mirrors.aliyun.com/docker-ce/linux/centos/docker-ce.repo &>/dev/null
yum clean all && yum make cache &>/dev/null
yum -y install docker-ce &>/dev/null
systemctl start docker
systemctl enable docker

sudo firewall-cmd --permanent --zone=public --add-service=http 
sudo firewall-cmd --permanent --zone=public --add-service=https
sudo firewall-cmd --zone=public --add-port=80/tcp --permanent
sudo firewall-cmd --zone=public --add-port=443/tcp --permanent
sudo firewall-cmd --reload
#systemctl restart firewalld

echo "Please wait until docker image pulled & running..."

docker pull zhangc1128/officewopi:8080
docker run --name officewopi8080 -d --net host -p 8080:8080 --restart=always zhangc1128/officewopi:8080
