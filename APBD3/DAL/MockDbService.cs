using System.Collections.Generic;
using APBD3.Models;

namespace APBD3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student { Id = 1, FirstName = "Jan", LastName = "Kowalski" },
                new Student { Id = 2, FirstName = "Anna", LastName = "Malewski" },
                new Student { Id = 3, FirstName = "Andrzej", LastName = "Andrzejewicz" }
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}