using System;
using System.Collections.Generic;

namespace APBD3.Models
{
    public class TeamMember
    {
        public string IdTeamMember { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public IEnumerable<Task> AssignedTasks { get; set; }
        public IEnumerable<Task> CreatedTasks { get; set; }
    }
}