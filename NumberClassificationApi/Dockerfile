FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7177

ENV ASPNETCORE_URLS=http://+:7177

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["NumberClassificationApi/NumberClassificationApi.csproj", "NumberClassificationApi/"]
RUN dotnet restore "NumberClassificationApi/NumberClassificationApi.csproj"
COPY . .
WORKDIR "/src/NumberClassificationApi"
RUN dotnet build "NumberClassificationApi.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "NumberClassificationApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NumberClassificationApi.dll"]
