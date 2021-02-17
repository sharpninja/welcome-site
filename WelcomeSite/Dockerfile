#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WelcomeSite.csproj", "/src"]
RUN dotnet restore "WelcomeSite.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "WelcomeSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WelcomeSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WelcomeSite.dll"]