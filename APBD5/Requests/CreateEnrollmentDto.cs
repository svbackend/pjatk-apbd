using System.ComponentModel.DataAnnotations;

namespace APBD3.Requests
{
    public class CreateEnrollmentDto
    {
        [Required] public string IndexNumber { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public string BirthDate { get; set; }

        [Required] public string Studies { get; set; }
    }
}