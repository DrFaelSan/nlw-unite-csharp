using System.ComponentModel.DataAnnotations.Schema;

namespace PassIn.Infrastructure.Entities;

public class Attendee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [Column("Event_Id")]
    public Guid EventId { get; set; }

    [Column("Created_At")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("Attendee_Id")]
    public CheckIn? CheckIn { get; set; }
}

