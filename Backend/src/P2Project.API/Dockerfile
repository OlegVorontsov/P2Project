FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/P2Project.API/P2Project.API.csproj", "src/P2Project.API/"]
COPY ["src/Shared/P2Project.SharedKernel/P2Project.SharedKernel.csproj", "src/Shared/P2Project.SharedKernel/"]
COPY ["src/Shared/P2Project.Core/P2Project.Core.csproj", "src/Shared/P2Project.Core/"]
COPY ["src/Shared/P2Project.Framework/P2Project.Framework.csproj", "src/Shared/P2Project.Framework/"]
COPY ["src/Accounts/P2Project.Accounts.Web/P2Project.Accounts.Web.csproj", "src/Accounts/P2Project.Accounts.Web/"]
COPY ["src/Accounts/P2Project.Accounts.Domain/P2Project.Accounts.Domain.csproj", "src/Accounts/P2Project.Accounts.Domain/"]
COPY ["src/Accounts/P2Project.Accounts.Agreements/P2Project.Accounts.Agreements.csproj", "src/Accounts/P2Project.Accounts.Agreements/"]
COPY ["src/Accounts/P2Project.Accounts.Application/P2Project.Accounts.Application.csproj", "src/Accounts/P2Project.Accounts.Application/"]
COPY ["src/Accounts/P2Project.Accounts.Infrastructure/P2Project.Accounts.Infrastructure.csproj", "src/Accounts/P2Project.Accounts.Infrastructure/"]
COPY ["src/Volunteers/P2Project.Volunteers.Web/P2Project.Volunteers.Web.csproj", "src/Volunteers/P2Project.Volunteers.Web/"]
COPY ["src/Volunteers/P2Project.Volunteers.Domain/P2Project.Volunteers.Domain.csproj", "src/Volunteers/P2Project.Volunteers.Domain/"]
COPY ["src/Volunteers/P2Project.Volunteers.Agreements/P2Project.Volunteers.Agreements.csproj", "src/Volunteers/P2Project.Volunteers.Agreements/"]
COPY ["src/Volunteers/P2Project.Volunteers.Application/P2Project.Volunteers.Application.csproj", "src/Volunteers/P2Project.Volunteers.Application/"]
COPY ["src/Volunteers/P2Project.Volunteers.Infrastructure/P2Project.Volunteers.Infrastructure.csproj", "src/Volunteers/P2Project.Volunteers.Infrastructure/"]
COPY ["src/Species/P2Project.Species.Web/P2Project.Species.Web.csproj", "src/Species/P2Project.Species.Web/"]
COPY ["src/Species/P2Project.Species.Domain/P2Project.Species.Domain.csproj", "src/Species/P2Project.Species.Domain/"]
COPY ["src/Species/P2Project.Species.Agreements/P2Project.Species.Agreements.csproj", "src/Species/P2Project.Species.Agreements/"]
COPY ["src/Species/P2Project.Species.Application/P2Project.Species.Application.csproj", "src/Species/P2Project.Species.Application/"]
COPY ["src/Species/P2Project.Species.Infrastructure/P2Project.Species.Infrastructure.csproj", "src/Species/P2Project.Species.Infrastructure/"]
COPY ["src/VolunteerRequests/P2Project.VolunteerRequests.Web/P2Project.VolunteerRequests.Web.csproj", "src/VolunteerRequests/P2Project.VolunteerRequests.Web/"]
COPY ["src/VolunteerRequests/P2Project.VolunteerRequests.Domain/P2Project.VolunteerRequests.Domain.csproj", "src/VolunteerRequests/P2Project.VolunteerRequests.Domain/"]
COPY ["src/VolunteerRequests/P2Project.VolunteerRequests.Agreements/P2Project.VolunteerRequests.Agreements.csproj", "src/VolunteerRequests/P2Project.VolunteerRequests.Agreements/"]
COPY ["src/VolunteerRequests/P2Project.VolunteerRequests.Application/P2Project.VolunteerRequests.Application.csproj", "src/VolunteerRequests/P2Project.VolunteerRequests.Application/"]
COPY ["src/VolunteerRequests/P2Project.VolunteerRequests.Infrastructure/P2Project.VolunteerRequests.Infrastructure.csproj", "src/VolunteerRequests/P2Project.VolunteerRequests.Infrastructure/"]
COPY ["src/Discussions/P2Project.Discussions.Web/P2Project.Discussions.Web.csproj", "src/Discussions/P2Project.Discussions.Web/"]
COPY ["src/Discussions/P2Project.Discussions.Domain/P2Project.Discussions.Domain.csproj", "src/Discussions/P2Project.Discussions.Domain/"]
COPY ["src/Discussions/P2Project.Discussions.Agreements/P2Project.Discussions.Agreements.csproj", "src/Discussions/P2Project.Discussions.Agreements/"]
COPY ["src/Discussions/P2Project.Discussions.Application/P2Project.Discussions.Application.csproj", "src/Discussions/P2Project.Discussions.Application/"]
COPY ["src/Discussions/P2Project.Discussions.Infrastructure/P2Project.Discussions.Infrastructure.csproj", "src/Discussions/P2Project.Discussions.Infrastructure/"]
RUN dotnet restore "src/P2Project.API/P2Project.API.csproj"
COPY . .
WORKDIR "/src/src/P2Project.API"
RUN dotnet build "P2Project.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "P2Project.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "P2Project.API.dll"]
