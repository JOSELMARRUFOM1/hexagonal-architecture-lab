# =========================================================
# Stage 1: Build
# =========================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY NuGet.Config ./

COPY src/BudgetService.Domain/BudgetService.Domain.csproj \
     src/BudgetService.Domain/

COPY src/BudgetService.Application/BudgetService.Application.csproj \
     src/BudgetService.Application/

COPY src/BudgetService.Infrastructure/BudgetService.Infrastructure.csproj \
     src/BudgetService.Infrastructure/

COPY src/BudgetService.Api/BudgetService.Api.csproj \
     src/BudgetService.Api/

RUN dotnet restore src/BudgetService.Api/BudgetService.Api.csproj \
    --configfile NuGet.Config

COPY src/ src/

RUN dotnet publish src/BudgetService.Api/BudgetService.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================================================
# Stage 2: Runtime
# =========================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "BudgetService.Api.dll"]