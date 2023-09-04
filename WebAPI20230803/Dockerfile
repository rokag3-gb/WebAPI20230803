
# golang:1.20 이미지를 chisel 이름으로 새로운 단계 시작
FROM golang:1.20 as chisel

# GitHub 저장소에서 Chisel repo 클론하고 /opt/chisel 으로 이동,
RUN git clone --depth 1 -b main https://github.com/canonical/chisel /opt/chisel
WORKDIR /opt/chisel
RUN go build ./cmd/chisel

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build

# 앞서 빌드한 Chisel 실행 파일을 /usr/bin/ 으로 복사
COPY --from=chisel /opt/chisel/chisel /usr/bin/

# /rootfs 디렉토리 생성하고 chisel cut 으로 Ubuntu 22.04 릴리즈에 필요한 라이브러리를 추출해서 /rootfs 디렉토리에 저장
RUN mkdir /rootfs
RUN chisel cut --release "ubuntu-22.04" --root /rootfs/ libicu70_libs libssl3_libs

WORKDIR /source

COPY . .

RUN dotnet restore "WebAPI20230803.csproj"


# 게시
RUN dotnet publish -c Release -o /app --self-contained false

# final stage/image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:7.0-jammy-chiseled
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV TZ=Asia/Seoul
USER root

COPY --from=build /rootfs /
WORKDIR /app
COPY --from=build /app .

#EXPOSE 80
EXPOSE 8080
#EXPOSE 443
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