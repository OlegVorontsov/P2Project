
namespace P2Project.Application.Volunteers.Queries.GetVolunteers
{
    public class GetVolunteersHandler
    {
        private readonly IVolunteersRepository _volunteersRepository;
        public GetVolunteersHandler(
            IVolunteersRepository volunteersRepository)
        {
            _volunteersRepository = volunteersRepository;
        }
        public async Task Handle(CancellationToken cancellationToken)
        {
            var volunteers = 
        }
    }
}
