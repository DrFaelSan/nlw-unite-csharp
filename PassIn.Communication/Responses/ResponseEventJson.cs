namespace PassIn.Communication.Responses;
public record ResponseEventJson
(
   Guid Id,
   string Title ,
   string Details,
   int MaximumAttendees,
   int AttendeesAmount
);
