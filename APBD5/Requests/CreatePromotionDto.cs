using System.ComponentModel.DataAnnotations;

namespace APBD3.Requests
{
    public class CreatePromotionDto
    {
        [Required] public string Studies { get; set; }

        [Required] public int Semester { get; set; }
    }
}