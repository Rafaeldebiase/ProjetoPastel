#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pastel.App/Pastel.App.csproj", "Pastel.App/"]
RUN dotnet restore "Pastel.App/Pastel.App.csproj"
COPY . .
WORKDIR "/src/Pastel.App"
RUN dotnet build "Pastel.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pastel.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pastel.App.dll"]