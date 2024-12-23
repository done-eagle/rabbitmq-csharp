FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /rabbitmq-app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /rabbitmq-app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /rabbitmq-app
EXPOSE 6000
ENV ASPNETCORE_URLS=http://+:6000
COPY --from=build /rabbitmq-app/published-app /rabbitmq-app
ENTRYPOINT [ "dotnet", "/rabbitmq-app/RabbitMqExample.Api.dll" ]