FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

RUN dotnet build --configuration Release --output /app/build

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20 AS runtime
WORKDIR /app

RUN apk update && apk add --no-cache libreoffice

COPY --from=build-env /app/build ./
COPY ./testfiles ./testfiles

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
