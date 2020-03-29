using System.Collections.Generic;
using APBD3.Models;

namespace APBD3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        
        public IEnumerable<Enrollment> GetStudentEntries(int indexNumber);
    }
}