namespace HD_Backend.Data.Entities
{
    public class Ticket
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public long? DepartmentId { get; set; }

        public Department? Department { get; set; }

        public long? FacultyId { get; set; }

        public Faculty? Faculty { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Priority { get; set; }
    }
}
