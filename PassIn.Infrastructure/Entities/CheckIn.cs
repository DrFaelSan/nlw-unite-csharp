using System.ComponentModel.DataAnnotations.Schema;

namespace PassIn.Infrastructure.Entities;

public class CheckIn
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Column("Created_At")]
    public DateTime CreatedAt { get; set; }
    
    [Column("Attendee_Id")]
    public Guid AttendeeId { get; set; }

    [ForeignKey("Attendee_Id")]
    public Attendee Attendee { get; set; } = default!;
}
