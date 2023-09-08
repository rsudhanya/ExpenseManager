#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExpenseManager.Api/ExpenseManager.Api.csproj", "ExpenseManager.Api/"]
RUN dotnet restore "ExpenseManager.Api/ExpenseManager.Api.csproj"
COPY . .
WORKDIR "/src/ExpenseManager.Api"
RUN dotnet build "ExpenseManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpenseManager.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseManager.Api.dll"]