using MediatR;

namespace Movies.Application.Features.Actors.Commands.PartialUpdateActor;

public class PartialUpdateActorCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? Birthdate { get; set; }
}
