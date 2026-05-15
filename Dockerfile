FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props ./
COPY AgendAI.Domain/ AgendAI.Domain/
COPY AgendAI.Application/ AgendAI.Application/
COPY AgendAI.CrossCutting/ AgendAI.CrossCutting/
COPY AgendAI.Infra/ AgendAI.Infra/
COPY AgendAI.API/ AgendAI.API/

RUN dotnet publish AgendAI.API/AgendAI.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AgendAI.API.dll"]
