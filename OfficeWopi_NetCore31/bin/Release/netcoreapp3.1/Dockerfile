#引入镜像
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

RUN apt-get update -y && apt-get install -y libgdiplus && apt-get clean && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll

# 设置工作路径
WORKDIR /OfficeWopi_NetCore
 
# 将当前文件夹下的所有文件全部复制到工作目录
COPY  . .
 
# 向外界暴露80端口
EXPOSE 8080
 
# 执行dotnet OfficeWopi_NetCore31.dll命令
ENTRYPOINT ["dotnet", "OfficeWopi_NetCore31.dll"]

#docker build -t officewopi .  

#docker tag officewopi:latest zhangc1128/officewopi:8080

#docker push zhangc1128/officewopi:8080

#docker pull zhangc1128/officewopi:8080

# 运行镜像实例
#docker run --name officewopi10 -d --net host -p 8080:8080 --restart=always zhangc1128/officewopi:8080