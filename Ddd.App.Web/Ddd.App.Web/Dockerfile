#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root
COPY Output/Certs/RootCa /usr/local/share/ca-certificates/
RUN apt-get update && apt-get install -y ca-certificates && update-ca-certificates
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
USER root
RUN mkdir -p /app/build /app/publish && \
    chown -R app:app /app
# The .csproj files are copied so they can be read and 
# the nuget packages restored before the rest of the source code is copied
COPY --chown=app:app ["Ddd.App.Web/Ddd.App.Web/Ddd.App.Web.csproj", "Ddd.App.Web/Ddd.App.Web/Ddd.App.Web.csproj"]
COPY --chown=app:app ["Ddd.App/Ddd.App.csproj", "Ddd.App/Ddd.App.csproj"]
COPY --chown=app:app ["Ddd.App.Presentation/Ddd.App.Presentation.csproj", "Ddd.App.Presentation/Ddd.App.Presentation.csproj"]
COPY --chown=app:app ["Ddd.App.Web/Ddd.App.Web.Client/Ddd.App.Web.Client.csproj", "Ddd.App.Web/Ddd.App.Web.Client/Ddd.App.Web.Client.csproj"]
COPY --chown=app:app ["Ddd.App.Events/Ddd.App.Events.csproj", "Ddd.App.Events/Ddd.App.Events.csproj"]
COPY --chown=app:app ["nuget.config", "nuget.config"]
RUN dotnet restore "./Ddd.App.Web/Ddd.App.Web/Ddd.App.Web.csproj"
COPY . .
WORKDIR "/src/Ddd.App.Web/Ddd.App.Web"
RUN dotnet build "./Ddd.App.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Ddd.App.Web/Ddd.App.Web"
RUN dotnet publish "./Ddd.App.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME [ "/log" ]
ENTRYPOINT ["dotnet", "Ddd.App.Web.dll"]

FROM publish AS test
USER root
RUN dotnet tool install -g dotnet-reportgenerator-globaltool
WORKDIR /src
COPY --chown=app:app ["Solutions/Ddd.App/druntests.sh", "Solutions/Ddd.App/druntests.sh"]
COPY --chown=app:app ["Solutions/Ddd.App/CodeCoverage.runsettings.xml", "Solutions/Ddd.App/CodeCoverage.runsettings.xml"]
COPY --chown=app:app ["Solutions/Ddd.App/Ddd.App.sln", "Solutions/Ddd.App/Ddd.App.sln"]
ENTRYPOINT ["sh", "/src/Solutions/Ddd.App/druntests.sh"]