# [Choice] .NET version: 5.0, 3.1, 2.1
ARG VARIANT=3.1
FROM mcr.microsoft.com/vscode/devcontainers/dotnet:0-${VARIANT}

RUN apt-get update && apt-get install -y build-essential g++ mariadb-client

# RUN dotnet tool install --global dotnet-ef
# RUN dotnet tool install --global dotnet-aspnet-codegenerator --version="3.1.5"

ENV PATH $PATH:/home/vscode/.dotnet/tools

# WORKDIR /workspace/server

# RUN dotnet add package Microsoft.EntityFrameworkCore
# RUN dotnet add package Microsoft.EntityFrameworkCore.Design
# RUN dotnet add package Microsoft.EntityFrameworkCore.SqlServer
# RUN dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
# RUN dotnet add package Pomelo.EntityFrameworkCore.MySql
