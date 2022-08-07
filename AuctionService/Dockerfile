FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuctionService/AuctionService.csproj", "AuctionService/"]
RUN dotnet restore "AuctionService/AuctionService.csproj"
COPY . .
WORKDIR "/src/AuctionService"
RUN dotnet build "AuctionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuctionService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionService.dll"]
