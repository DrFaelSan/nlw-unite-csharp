using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Events.Register;

public class RegisterEventUseCase
{
    public ResponseRegisteredJson Execute(RequestEventJson request)
    {
        Validate(request);

        var dbContext = new PassInDbContext();

        var entity = new Event
        {
            Title = request.Title,
            Details = request.Details,
            MaximumAttendees = request.MaximumAttendees,
            Slug = request.Title.ToLower().Replace(" ", "-")
        };


        dbContext.Events.Add(entity);
        dbContext.SaveChanges();

        return new ResponseRegisteredJson
        {
            Id = entity.Id
        };
    }

    private void Validate(RequestEventJson request)
    {
        if (request.MaximumAttendees <= 0)
            throw new ErrorOnValidationException("Número de participantes é inválido.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ErrorOnValidationException("O titulo é inválido.");

        if (string.IsNullOrWhiteSpace(request.Details))
            throw new ErrorOnValidationException("Os Detalhes são inválidos.");

    }
}
