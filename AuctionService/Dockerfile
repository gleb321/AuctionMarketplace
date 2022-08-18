FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /service
EXPOSE 7061
ENV ASPNETCORE_URLS=http://+:7061

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuctionService/AuctionService.csproj", "AuctionService/"]
RUN dotnet restore "AuctionService/AuctionService.csproj"
COPY . ./
RUN dotnet build "/AuctionService/AuctionService.csproj" -c Release -o /service/build

FROM build AS publish
RUN dotnet publish "AuctionService.csproj" -c Release -o /service/publish

FROM base AS service
WORKDIR /service
COPY --from=build-env /service/publish .
ENTRYPOINT ["dotnet", "AuctionService.dll"]