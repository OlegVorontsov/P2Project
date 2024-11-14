using CSharpFunctionalExtensions;
using P2Project.Domain.Shared;

namespace P2Project.Application.Volunteers.UpdateMainInfo
{
    public class UpdateMainInfoHandler
    {
        public async Task<Result<Guid, Error>> Handle(
            UpdateMainInfoRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateMainInfoCommand(request.VolunteerId);
            return command.VolunteerId;
        }
    }
}
