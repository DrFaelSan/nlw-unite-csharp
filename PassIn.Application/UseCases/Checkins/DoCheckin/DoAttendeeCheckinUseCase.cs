using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Checkins.DoCheckin;

public class DoAttendeeCheckinUseCase
{
    private readonly PassInDbContext _dbContext;

    public DoAttendeeCheckinUseCase()
     => _dbContext = new PassInDbContext();

    public ResponseRegisteredJson Execute(Guid attendeeId)
    {
        Validate(attendeeId);

        var checkInEntity = new CheckIn
        {
            AttendeeId = attendeeId,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.CheckIns.Add(checkInEntity);
        _dbContext.SaveChanges();

        return new ResponseRegisteredJson {
            Id = checkInEntity.Id
        };
    }

    private void Validate(Guid attendeeId)
    {
        var existAttendee = _dbContext.Attendees.Any(a => a.Id == attendeeId);


        if (!existAttendee)
            throw new NotFoundException("O Participante com esse id não foi encontrado.");

        var existCheckIn = _dbContext.CheckIns.Any(c => c.AttendeeId == attendeeId);
        if (existCheckIn)
            throw new ConflictException("O Participante não pode fazer check-in novamente no mesmo evento.");

    }
}
