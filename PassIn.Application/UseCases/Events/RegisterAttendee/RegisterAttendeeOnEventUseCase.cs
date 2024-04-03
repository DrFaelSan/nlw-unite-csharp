using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee;

public class RegisterAttendeeOnEventUseCase
{
    private readonly PassInDbContext _dbContext;

    public RegisterAttendeeOnEventUseCase()
        =>  _dbContext = new PassInDbContext();

    public ResponseRegisteredJson Execute(Guid eventId, RequestRegisterEventJson request)
    {
        Validate(eventId, request);

        Attendee entity = new()
        {
            Email = request.Email,
            Name = request.Name,
            EventId = eventId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Attendees.Add(entity);
        _dbContext.SaveChanges();

        return new ResponseRegisteredJson
        {
            Id = entity.Id
        };
    }

    private void Validate(Guid eventId, RequestRegisterEventJson request)
    {
        Event? eventEntity = _dbContext.Events.Find(eventId);

        if (eventEntity is null)
            throw new NotFoundException($"O não existe um evento com esse id({eventId})");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ErrorOnValidationException("O Nome é inválido");

        if(!EmailIsValid(request.Email))
            throw new ErrorOnValidationException("O E-mail é inválido");

        if(AttendeeAlreadyRegistered(eventId, request.Email))
            throw new ConflictException("Não é possível registrar duas vezes o mesmo participante");

        int attendeesForEvent = _dbContext.Attendees.Count(a => a.EventId == eventId);
        if (attendeesForEvent == eventEntity.MaximumAttendees)
            throw new ErrorOnValidationException("Não existe mais espaço para esse evento.");
    }

    private bool EmailIsValid(string email)
    {
        try
        {
            new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool AttendeeAlreadyRegistered(Guid eventId, string email)
        => _dbContext
            .Attendees
            .Any(a => a.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) && a.EventId == eventId);
}
