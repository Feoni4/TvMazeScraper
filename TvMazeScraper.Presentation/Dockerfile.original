FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["TvMazeScraper.Presentation/TvMazeScraper.Presentation.csproj", "TvMazeScraper.Presentation/"]
COPY ["TvMazeScraper.DAL/TvMazeScraper.DAL.csproj", "TvMazeScraper.DAL/"]
COPY ["TvMazeScraper.Contracts/TvMazeScraper.Contracts.csproj", "TvMazeScraper.Contracts/"]
COPY ["TvMazeScraper.Presentation.Domain/TvMazeScraper.Presentation.Domain.csproj", "TvMazeScraper.Presentation.Domain/"]
RUN dotnet restore "TvMazeScraper.Presentation/TvMazeScraper.Presentation.csproj"
COPY . .
WORKDIR "/src/TvMazeScraper.Presentation"
RUN dotnet build "TvMazeScraper.Presentation.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TvMazeScraper.Presentation.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TvMazeScraper.Presentation.dll"]