using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventId;

public class GetAllAttendeesByEventIdUseCase
{
    private readonly PassInDbContext _dbContext;

    public GetAllAttendeesByEventIdUseCase()
        => _dbContext = new PassInDbContext();

    public ResponseAllAttendeesJson Execute(Guid eventId)
    {
        var eventEntity = _dbContext
            .Events
            .Include(e => e.Attendees)
            .ThenInclude(a => a.CheckIn)
            .FirstOrDefault(e => e.Id == eventId);

        if (eventEntity is null)
            throw new NotFoundException($"O não existe um evento com esse id({eventId})");

        return new ResponseAllAttendeesJson
        {
            Attendees = eventEntity
            .Attendees
            .Select(a => new ResponseAttendeeJson()
            {
                CreatedAt = a.CreatedAt,
                Email = a.Email,
                Name = a.Name,
                Id = a.Id,
                CheckedInAt = a.CheckIn?.CreatedAt
            }).ToList()
        };

    }


}
