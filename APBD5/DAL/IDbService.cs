using System.Collections.Generic;
using APBD3.Models;
using APBD3.Requests;

namespace APBD3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        
        public IEnumerable<Enrollment> GetStudentEntries(int indexNumber);
        
        public Enrollment EnrollStudent(CreateEnrollmentDto dto);
        
        public Enrollment Promote(CreatePromotionDto dto);
    }
}