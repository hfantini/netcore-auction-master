FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AuctionMaster.csproj", ""]
RUN dotnet restore "./AuctionMaster.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AuctionMaster.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuctionMaster.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionMaster.dll"]