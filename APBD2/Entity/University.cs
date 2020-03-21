using System;
using System.Collections.Generic;
using APBD2.Entity;

namespace APBD2
{
    public class University
    {
        public DateTime CreatedAt { get; set; }
        public string Author { get; set; }
        public List<Student> Students { get; set; }
        public List<ActiveStudy> ActiveStudies { get; set; }
    }
}