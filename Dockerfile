FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG RAILWAY_SERVICE_NAME
ARG RAILWAY_ENVIRONMENT

COPY ./src/Chat /source

WORKDIR /source

ARG TARGETARCH

RUN --mount=type=cache,id=s/aecef585-2a2d-4fee-af04-8d9151fba668,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

COPY --from=build /app .

USER $APP_UID

ENTRYPOINT ["dotnet", "Chat.dll"]
