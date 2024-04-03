using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Events.GetById;

public class GetEventByIdUseCase
{
    public ResponseEventJson Execute(Guid id)
    {
        PassInDbContext? dbContext = new PassInDbContext();

        Event? entity = dbContext.Events.Include(e => e.Attendees).FirstOrDefault(e => e.Id == id);

        if (entity is null)
            throw new NotFoundException($"O Evento com esse id({id}) não foi encontrado");

        return new ResponseEventJson(entity.Id, entity.Title,entity.Details, entity.MaximumAttendees, entity.Attendees.Count());
    }
}
