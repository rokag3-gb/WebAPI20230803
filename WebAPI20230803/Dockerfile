
# golang:1.20 �̹����� chisel �̸����� ���ο� �ܰ� ����
FROM golang:1.20 as chisel

# GitHub ����ҿ��� Chisel repo Ŭ���ϰ� /opt/chisel ���� �̵�,
RUN git clone --depth 1 -b main https://github.com/canonical/chisel /opt/chisel
WORKDIR /opt/chisel
RUN go build ./cmd/chisel

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build

# �ռ� ������ Chisel ���� ������ /usr/bin/ ���� ����
COPY --from=chisel /opt/chisel/chisel /usr/bin/

# /rootfs ���丮 �����ϰ� chisel cut ���� Ubuntu 22.04 ����� �ʿ��� ���̺귯���� �����ؼ� /rootfs ���丮�� ����
RUN mkdir /rootfs
RUN chisel cut --release "ubuntu-22.04" --root /rootfs/ libicu70_libs libssl3_libs

WORKDIR /source

COPY . .
RUN dotnet restore "WebAPI20230803.sln"

WORKDIR /source/WebAPI20230803

# �Խ�
RUN dotnet publish -c Release -o /app --self-contained false

# final stage/image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:7.0-jammy-chiseled
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV TZ=Asia/Seoul
USER root

COPY --from=build /rootfs /
WORKDIR /app
COPY --from=build /app .

EXPOSE 80
EXPOSE 8080
EXPOSE 443
ENTRYPOINT ["dotnet", "WebAPI20230803.dll"]



#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["WebAPI20230803/WebAPI20230803.csproj", "WebAPI20230803/"]
#RUN dotnet restore "WebAPI20230803/WebAPI20230803.csproj"
#COPY . .
#WORKDIR "/src/WebAPI20230803"
#RUN dotnet build "WebAPI20230803.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "WebAPI20230803.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "WebAPI20230803.dll"]