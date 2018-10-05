FROM microsoft/dotnet:latest

COPY . /app
WORKDIR /app

RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]


ENV ASPNETCORE_URLS="http://0.0.0.0:5000";"https://0.0.0.0:5001"

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "run"]
