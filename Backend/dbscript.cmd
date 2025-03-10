docker-compose up -d

dotnet-ef database drop -f -c AccountsWriteDbContext -p ./src/Accounts/P2Project.Accounts.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef database drop -f -c VolunteersWriteDbContext -p ./src/Volunteers/P2Project.Volunteers.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef database drop -f -c VolunteerRequestsWriteDbContext -p ./src/VolunteerRequests/P2Project.VolunteerRequests.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef database drop -f -c DiscussionsWriteDbContext -p ./src/Discussions/P2Project.Discussions.Infrastructure/ -s ./src/P2Project.API/

dotnet-ef migrations remove -c AccountsWriteDbContext -p ./src/Accounts/P2Project.Accounts.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef migrations remove -c VolunteersWriteDbContext -p ./src/Volunteers/P2Project.Volunteers.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef migrations remove -c VolunteerRequestsWriteDbContext -p ./src/VolunteerRequests/P2Project.VolunteerRequests.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef migrations remove -c DiscussionsWriteDbContext -p ./src/Discussions/P2Project.Discussions.Infrastructure/ -s ./src/P2Project.API/

dotnet-ef migrations add Accounts_init -c AccountsWriteDbContext -p ./src/Accounts/P2Project.Accounts.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef migrations add Volunteers_init -c VolunteersWriteDbContext -p ./src/Volunteers/P2Project.Volunteers.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef migrations add VolunteerRequests_init -c VolunteerRequestsWriteDbContext -p ./src/VolunteerRequests/P2Project.VolunteerRequests.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef migrations add Discussions_init -c DiscussionsWriteDbContext -p ./src/Discussions/P2Project.Discussions.Infrastructure/ -s ./src/P2Project.API/

dotnet-ef database update -c AccountsWriteDbContext -p ./src/Accounts/P2Project.Accounts.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef database update -c VolunteersWriteDbContext -p ./src/Volunteers/P2Project.Volunteers.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef database update -c VolunteerRequestsWriteDbContext -p ./src/VolunteerRequests/P2Project.VolunteerRequests.Infrastructure/ -s ./src/P2Project.API/
dotnet-ef database update -c DiscussionsWriteDbContext -p ./src/Discussions/P2Project.Discussions.Infrastructure/ -s ./src/P2Project.API/

pause