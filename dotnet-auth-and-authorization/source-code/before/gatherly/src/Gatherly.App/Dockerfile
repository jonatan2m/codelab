#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Gatherly.App/Gatherly.App.csproj", "src/Gatherly.App/"]
RUN dotnet restore "src/Gatherly.App/Gatherly.App.csproj"
COPY . .
WORKDIR "/src/src/Gatherly.App"
RUN dotnet build "Gatherly.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gatherly.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gatherly.App.dll"]