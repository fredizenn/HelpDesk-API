namespace HD_Backend.Data.Entities
{
    public class Faculty
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public long? DepartmentId { get; set; }

        public Department? Department { get; set; }

        public List<Agent>? Agents { get; set; }

        public List<User>? Users { get; set; }

        public List<Ticket>? Tickets { get; set; }
    }
}
