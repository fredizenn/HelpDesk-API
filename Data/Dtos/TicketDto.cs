namespace HD_Backend.Data.Dtos
{
    public class TicketDto
    {
        public long Id { get; set; }

        public string? Code { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public string? Priority { get; set; }

        public long? DepartmentId { get; set; }

        public string? DepartmentCode { get; set; }

        public string? DepartmentName { get; set; }

        public long? FacultyId { get; set; }

        public string? FacultyName { get; set; }

        public string? FacultyCode { get; set; }

        public string? ContactName { get; set; }

        public string? ContactPhoneNumber { get; set; }

        public string? TicketDescription { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? ContactEmail { get; set; }


        public bool IsResolved { get; set; }

        public bool OnHold { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsOpen { get; set; }

    }

    public class CreateTicketDto : TicketAddUpdateDto
    {

    }

    public class UpdateTicketDto : TicketAddUpdateDto
    {

    }

    public class ResolveTicketDto
    {
        public bool IsResolved { get; set; } = true;
    }

    public class OpenTicketDto
    {
        public bool IsOpen { get; set; } = true;
    }

    public class CancelTicketDto
    {
        public bool IsCancelled { get; set; } = true;
    }

    public class HoldTicketDto
    {
        public bool OnHold { get; set; } = true;

    }
    public abstract class TicketAddUpdateDto
    {
        public string? Code { get; set; }

        public string? Type { get; set; }

        public string? Priority { get; set; }

        public long? DepartmentId { get; set; }

        public long? FacultyId { get; set; }

        public string? ContactName { get; set; }

        public string? ContactPhoneNumber { get; set; }

        public string? ContactEmail { get; set; }

        public string? TicketDescription { get; set; }


    }
}
