namespace HD_Backend.Data.Entities
{
    public class Department
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }

        public List<Faculty>? Faculties { get; set; }

        public List<User>? Users { get; set; }

        public List<Ticket>? Ticket { get; set; }
    }
}
