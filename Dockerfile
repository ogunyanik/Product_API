FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Product_API.csproj", "./"]
RUN dotnet restore "Product_API.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Product_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Product_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product_API.dll"]
