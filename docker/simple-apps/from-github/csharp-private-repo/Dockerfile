FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

# build image to container structure using sdk ASP.NET Core 3.1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY lucros/src/ProfitSharingApi/ .
#COPY lucros/src/ProfitSharingApi/ProfitSharingApi/ProfitSharingApi.csproj ProfitSharingApi/
RUN dotnet restore "ProfitSharingApi/ProfitSharingApi.csproj"

COPY . .

WORKDIR /src/ProfitSharingApi
RUN dotnet build ProfitSharingApi.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProfitSharingApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80

ENTRYPOINT ["dotnet" "ProfitSharingApi.dll"]








#FROM node as builder

#RUN git clone https://github.com/phillies/vue_example.git /opt/vue_example
#WORKDIR /opt/vue_example

#RUN npm install && npm run build

#FROM nginx

#COPY --from=builder /opt/vue_example/dist/ /usr/share/nginx/html/

#EXPOSE 80