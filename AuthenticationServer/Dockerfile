FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ["AuthenticationServer/AuthenticationServer.csproj", "AuthenticationServer/"]
RUN dotnet restore "AuthenticationServer/AuthenticationServer.csproj"

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS service
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 7137
ENV ASPNETCORE_URLS=http://+:7137
ENTRYPOINT ["dotnet", "AuthenticationServer.dll"]