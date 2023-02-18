#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mubasa.Web/Mubasa.Web.csproj", "Mubasa.Web/"]
COPY ["Mubasa.DataAccess/Mubasa.DataAccess.csproj", "Mubasa.DataAccess/"]
COPY ["Mubasa.Models/Mubasa.Models.csproj", "Mubasa.Models/"]
COPY ["Mubasa.Utility/Mubasa.Utility.csproj", "Mubasa.Utility/"]
RUN dotnet restore "Mubasa.Web/Mubasa.Web.csproj"
COPY . .
WORKDIR "/src/Mubasa.Web"
RUN dotnet build "Mubasa.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mubasa.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mubasa.Web.dll"]