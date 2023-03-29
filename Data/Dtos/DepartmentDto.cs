namespace HD_Backend.Data.Dtos

{
    public class DepartmentDto
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Code { get; set; }
    }


    public class CreateDepartmentDto : DepartmentAddUpdateDto
    {
        
    }

   public class UpdateDepartmentDto : DepartmentAddUpdateDto
    {

    }


    public abstract class DepartmentAddUpdateDto
    {
        public string? Name { get; set; }

        public string? Code { get; set; }
    }
}
