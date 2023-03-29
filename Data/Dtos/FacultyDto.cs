namespace HD_Backend.Data.Dtos
{
    public class FacultyDto
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }
    }

    public class CreateFacultyDto : FacultyAddUpdateDto
    {

    }

    public class UpdateFacultyDto : FacultyAddUpdateDto
    {

    }

    public abstract class FacultyAddUpdateDto
    {
        public string? Name { get; set; }

        public string? Code { get; set; }
    }
}
