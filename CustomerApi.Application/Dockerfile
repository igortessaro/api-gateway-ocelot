#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CustomerApi.Application/CustomerApi.Application.csproj", "CustomerApi.Application/"]
RUN dotnet restore "CustomerApi.Application/CustomerApi.Application.csproj"
COPY . .
WORKDIR "/src/CustomerApi.Application"
RUN dotnet build "CustomerApi.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerApi.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerApi.Application.dll"]