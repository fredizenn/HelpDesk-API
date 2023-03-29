using Microsoft.AspNetCore.Identity;

namespace HD_Backend.Data.Entities
{
    public class User : IdentityUser
    {
        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public long? DepartmentId { get; set; }

        public Department? Department { get; set; }

        public long? FacultyId { get; set; }

        public Faculty? Faculty { get; set; }

    }
}
