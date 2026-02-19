FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Domain/Domain.csproj Domain/
COPY Application/Application.csproj Application/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Web/Web.csproj Web/

RUN dotnet restore Web/Web.csproj

COPY . .
WORKDIR /src/Web
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

EXPOSE 8080
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Web.dll"]
