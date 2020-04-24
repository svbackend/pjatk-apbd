using System.Collections.Generic;
using APBD3.Models;

namespace APBD3.DAL
{
    public interface IDbService
    {
        public TeamMember GetTeamMember(int id);
        public void DeleteProject(int id);
    }
}