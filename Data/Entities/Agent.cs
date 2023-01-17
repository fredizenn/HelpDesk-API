namespace HD_Backend.Data.Entities
{
    public class Agent
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public long? DepartmentId { get; set; }

        public Department? Department { get; set; }

        public long? FacultyId { get; set; }

        public Faculty? Faculty { get; set; }



    }
}
