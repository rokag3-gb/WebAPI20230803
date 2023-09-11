# Docker hub - ASP.NET Core Runtime 6.0 / 7.0 -> https://hub.docker.com/_/microsoft-dotnet-aspnet
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebAPI20230803.csproj", "."]
RUN dotnet restore "./WebAPI20230803.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebAPI20230803.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI20230803.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ENV TZ=Asia/Seoul
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI20230803.dll"]