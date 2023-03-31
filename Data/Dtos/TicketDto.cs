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
    }

    public class CreateTicketDto : TicketAddUpdateDto
    {

    }

    public class UpdateTicketDto : TicketAddUpdateDto
    {

    }

    public abstract class TicketAddUpdateDto
    {
        public string? Code { get; set; }

        public string? Status { get; set; }

        public string? Type { get; set; }

        public string? Priority { get; set; }

        public long? DepartmentId { get; set; }

        public long? FacultyId { get; set; }
    }
}
