using System;

namespace APBD3.Models
{
    public class Enrollment
    {
        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
        public Study Study { get; set; }
    }
}