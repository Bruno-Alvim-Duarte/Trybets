FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

EXPOSE 5502

WORKDIR /app

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "TryBets.Matches.dll"]