FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /service
EXPOSE 7137
ENV ASPNETCORE_URLS=http://+:7137

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AuthenticationServer/AuthenticationServer.csproj", "AuthenticationServer/"]
RUN dotnet restore "AuthenticationServer/AuthenticationServer.csproj"
COPY . ./
RUN dotnet build "/AuthenticationServer/AuthenticationServer.csproj" -c Release -o /service/build

FROM build AS publish
RUN dotnet publish "AuthenticationServer.csproj" -c Release -o /service/publish

FROM base AS service
WORKDIR /service
COPY --from=build-env /service/publish .
ENTRYPOINT ["dotnet", "AuthenticationServer.dll"]