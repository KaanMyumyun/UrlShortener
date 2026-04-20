FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY URLShortner/*.csproj URLShortner/
RUN dotnet restore URLShortner/URLShortner.csproj

COPY . .
WORKDIR /src/URLShortner
RUN dotnet publish -c Release -o /app/publish

#run time
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY  --from=build /app/publish .
ENTRYPOINT [ "dotnet", "URLShortner.dll" ]