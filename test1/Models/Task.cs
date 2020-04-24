using System;

namespace APBD3.Models
{
    public class Task
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        
        public string Type { get; set; }
        public string ProjectName { get; set; }
    }
}