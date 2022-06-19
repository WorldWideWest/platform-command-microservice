FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-en
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-en /app/out . 
ENTRYPOINT [ "dotnet", "CommandService.dll" ]