#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WithSecure.Interview.Api/WithSecure.Interview.Api.csproj", "WithSecure.Interview.Api/"]
RUN dotnet restore "WithSecure.Interview.Api/WithSecure.Interview.Api.csproj"
COPY . .
WORKDIR "/src/WithSecure.Interview.Api"
RUN dotnet build "WithSecure.Interview.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WithSecure.Interview.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WithSecure.Interview.Api.dll"]