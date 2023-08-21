FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY Riduttore .
RUN dotnet restore "Server/Riduttore.Server.csproj"
RUN dotnet build "Server/Riduttore.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server/Riduttore.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt -y update && apt -y upgrade && apt install -y ffmpeg && apt install -y sqlite3 libsqlite3-dev
ENTRYPOINT ["dotnet", "Riduttore.Server.dll"]