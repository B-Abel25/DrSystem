#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development

WORKDIR /app
EXPOSE 80
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DoctorSystem.csproj", "./"]
RUN dotnet restore "DoctorSystem.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "DoctorSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DoctorSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DoctorSystem.dll"]
