namespace HD_Backend.Data.Entities
{
    public class Ticket
    {
        public long Id { get; set; }

        public string? Code { get; set; }

        public long? DepartmentId { get; set; }

        public Department? Department { get; set; }

        public long? FacultyId { get; set; }

        public Faculty? Faculty { get; set; }

        public string? Status { get; set; } = "Open";

        public string? Type { get; set; }

        public string? Priority { get; set; }

        public string? ContactName { get; set; }

        public string? ContactPhoneNumber { get; set; }

        public string? ContactEmail { get; set; }

        public string? TicketDescription { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsResolved { get; set; }

        public bool OnHold { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsOpen { get; set; } = true;
    }
}
