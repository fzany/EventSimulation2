FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["AndelaEventTest2/AndelaEventTest2.csproj", "AndelaEventTest2/"]
RUN dotnet restore "AndelaEventTest2/AndelaEventTest2.csproj"
COPY . .
WORKDIR "/src/AndelaEventTest2"
RUN dotnet build "AndelaEventTest2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AndelaEventTest2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AndelaEventTest2.dll"]
