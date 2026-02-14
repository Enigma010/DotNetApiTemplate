#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
# Set up GitHub Package Registry credentials for private NuGet packages
ARG GITHUBCFG_USERNAME
ARG GITHUBCFG_PAT
ARG GITHUBCFG_NAMESPACE

ENV GITHUBCFG_USERNAME=$GITHUBCFG_USERNAME
ENV GITHUBCFG_PAT=$GITHUBCFG_PAT
ENV GITHUBCFG_NAMESPACE=$GITHUBCFG_NAMESPACE

WORKDIR /src
# The .csproj files are copied so they can be read and 
# the nuget packages restored before the rest of the source code is copied
COPY ["Api/Api/Api.csproj", "Api/Api/Api.csproj"]
COPY ["App/App.csproj", "App/App.csproj"]
COPY ["nuget.config", "nuget.config"]
RUN echo $GITHUBCFG_NAMESPACE
RUN dotnet restore "./Api/Api/Api.csproj"
COPY . .
WORKDIR "/src/Api/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME [ "/log" ]
ENTRYPOINT ["dotnet", "Api.dll"]