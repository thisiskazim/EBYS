# 1. Aþama: Derleme (Build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Dýþarýdan gelecek Telerik bilgilerini alýyoruz
ARG TELERIK_USERNAME
ARG TELERIK_PASSWORD

# Konteyner ortam deðiþkenlerine atýyoruz
ENV TELERIK_USERNAME=$TELERIK_USERNAME
ENV TELERIK_PASSWORD=$TELERIK_PASSWORD

# Önbellek optimizasyonu için önce projeleri kopyalayýp restore ediyoruz
COPY ["EBYS.WebAPI/EBYS.WebAPI.csproj", "EBYS.WebAPI/"]
COPY ["EBYS.Application/EBYS.Application.csproj", "EBYS.Application/"]
COPY ["EBYS.Domain/EBYS.Domain.csproj", "EBYS.Domain/"]
COPY ["EBYS.Persistence/EBYS.Persistence.csproj", "EBYS.Persistence/"]
COPY ["EBYS.Web/EBYS.Web.csproj", "EBYS.Web/"]
COPY ["NuGet.Config", "./"]

# Baðýmlýlýklarý yüklüyoruz
RUN dotnet restore "EBYS.WebAPI/EBYS.WebAPI.csproj" --configfile NuGet.Config

# Tüm kaynak kodlarý kopyalýyoruz
COPY . .

# WebAPI projesini publish ediyoruz
WORKDIR "/src/EBYS.WebAPI"
RUN dotnet publish "EBYS.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Aþama: Çalýþtýrma (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "EBYS.WebAPI.dll"]