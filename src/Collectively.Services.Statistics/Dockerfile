FROM microsoft/dotnet:runtime
WORKDIR /dotnetapp
COPY ./bin/Docker .
ENV ASPNETCORE_URLS http://*:10004
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet Collectively.Services.Statistics.dll