FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY TvMazeScraper.Integration/TvMazeScraper.Integration.csproj TvMazeScraper.Integration/
COPY TvMazeScraper.Integration.Domain/TvMazeScraper.Integration.Domain.csproj TvMazeScraper.Integration.Domain/
COPY TvMazeScraper.DAL/TvMazeScraper.DAL.csproj TvMazeScraper.DAL/
COPY TvMazeScraper.Contracts/TvMazeScraper.Contracts.csproj TvMazeScraper.Contracts/
RUN dotnet restore TvMazeScraper.Integration/TvMazeScraper.Integration.csproj
COPY . .
WORKDIR /src/TvMazeScraper.Integration
RUN dotnet build TvMazeScraper.Integration.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TvMazeScraper.Integration.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TvMazeScraper.Integration.dll"]
