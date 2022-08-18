FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /service
EXPOSE 7019
ENV ASPNETCORE_URLS=http://+:7019

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuctionLiveService/AuctionService.csproj", "AuctionLiveService/"]
RUN dotnet restore "AuctionLiveService/AuctionLiveService.csproj"
COPY . ./
RUN dotnet build "/AuctionLiveService/AuctionLiveService.csproj" -c Release -o /service/build

FROM build AS publish
RUN dotnet publish "AuctionLiveService.csproj" -c Release -o /service/publish

FROM base AS service
WORKDIR /service
COPY --from=build-env /service/publish .
ENTRYPOINT ["dotnet", "AuctionLiveService.dll"]