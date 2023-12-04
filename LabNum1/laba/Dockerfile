FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["laba/laba.csproj", "laba/"]
RUN dotnet restore "laba/laba.csproj"
COPY . .
WORKDIR "/src/laba"
RUN dotnet build "laba.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "laba.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "laba.dll"]
