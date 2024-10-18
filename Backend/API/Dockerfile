FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet build --configuration Release --output /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y libreoffice libgdiplus libxml2


COPY --from=build-env /app/build ./

EXPOSE 8080

CMD ["dotnet", "API.dll"]
