ARG PROJECT_NAME=Api

FROM mcr.microsoft.com/dotnet/aspnet:10.0.0-preview.6-alpine3.22 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview-alpine AS build
ARG PROJECT_NAME
WORKDIR /src
COPY . .
RUN dotnet restore "${PROJECT_NAME}/${PROJECT_NAME}.csproj"
WORKDIR "/src/${PROJECT_NAME}"
RUN dotnet build "${PROJECT_NAME}.csproj" -c Release -o /app/build

FROM build AS publish
ARG PROJECT_NAME
RUN dotnet publish "${PROJECT_NAME}.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ARG PROJECT_NAME
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV PROJECT_NAME=${PROJECT_NAME}
ENTRYPOINT ["sh", "-c", "dotnet ${PROJECT_NAME}.dll"]