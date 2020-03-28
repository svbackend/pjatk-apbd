using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APBD3.Models;

namespace APBD3.DAL
{
    public class DbService : IDbService
    {
        //private static string connectionString = "jdbc:jtds:sqlserver://server-name/database_name;instance=instance_name";
        private static string connectionString = @"Server=db-mssql.pjwstk.edu.pl;Initial Catalog=s19319;User=s19319;Password=...;USENTLMV2=true;domain=PJWSTK";
        // private static string connectionString = "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s19319;Integrated Security=True;Trusted_Connection=true;User=s19319;Password=...";
        
        private static List<Student> _students;

        static DbService()
        {
            // name, surname, date of birth, name of studies and semester number
            var sql = "SELECT Student.FirstName, Student.LastName, Student.BirthDate, Studies.Name, E.Semester FROM Student LEFT JOIN Enrollment E on Student.IdEnrollment = E.IdEnrollment LEFT JOIN Studies on E.IdStudy = Studies.IdStudy";

            _students = new List<Student>();
            
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    _students.Add(new Student
                    {
                        FirstName = dr["Student.LastName"].ToString(),
                        LastName = dr["Student.LastName"].ToString(),
                        BirthDate = DateTime.Parse(dr["Student.BirthDate"].ToString()),
                        Enrollment = new Enrollment
                        {
                            Semester = int.Parse(dr["E.Semester"].ToString()),
                            Study = new Study
                            {
                                Name = dr["Studies.Name"].ToString()
                            }
                        },
                    });
                }
                
                conn.Close();
            }
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}