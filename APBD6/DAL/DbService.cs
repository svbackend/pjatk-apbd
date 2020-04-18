using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using APBD3.Exceptions;
using APBD3.Models;
using APBD3.Requests;

namespace APBD3.DAL
{
    public class DbService : IDbService
    {
        private static string connectionString =
            "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s19319;Integrated Security=True;Trusted_Connection=true;";

        public bool IsStudentExists(string index)
        {
            var sql = @"SELECT 1 FROM Student WHERE IndexNumber = @IndexNumber";
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("IndexNumber", index);
                var dr = cmd.ExecuteReader();
                return dr.Read();
            }
        }

        public IEnumerable<Student> GetStudents()
        {
            var students = new List<Student>();

            var sql = @"
                SELECT FirstName, LastName, BirthDate, Studies.Name StudiesName, Semester, IndexNumber, StartDate FROM Student 
                LEFT JOIN Enrollment E ON Student.IdEnrollment = E.IdEnrollment 
                LEFT JOIN Studies on E.IdStudy = Studies.IdStudy
            ";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    students.Add(new Student
                    {
                        IndexNumber = dr["IndexNumber"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                        Enrollment = new Enrollment
                        {
                            Semester = int.Parse(dr["Semester"].ToString()),
                            StartDate = DateTime.Parse(dr["StartDate"].ToString()),
                            Study = new Study
                            {
                                Name = dr["StudiesName"].ToString()
                            }
                        },
                    });
                }
            }

            return students;
        }

        public IEnumerable<Enrollment> GetStudentEntries(int indexNumber)
        {
            // name, surname, date of birth, name of studies and semester number
            var sql =
                "SELECT E.IdEnrollment IdEnrollment, Semester, StartDate, Studies.Name StudiesName FROM Enrollment E RIGHT JOIN Student S on S.IdEnrollment = E.IdEnrollment LEFT JOIN Studies ON E.IdStudy = Studies.IdStudy WHERE S.IndexNumber = @IndexNumber";

            var enrollments = new List<Enrollment>();

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("IndexNumber", indexNumber);

                conn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    enrollments.Add(new Enrollment()
                    {
                        Semester = int.Parse(dr["Semester"].ToString()),
                        StartDate = DateTime.Parse(dr["StartDate"].ToString()),
                        Study = new Study
                        {
                            Name = dr["StudiesName"].ToString()
                        }
                    });
                }
            }

            return enrollments;
        }

        private int FindStudyIdByName(SqlCommand cmd, string name)
        {
            cmd.CommandText = @"SELECT IdStudy FROM Studies WHERE Name = @Name";

            cmd.Parameters.AddWithValue("Name", name);
            var dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                throw new StudiesNotFoundException();
            }

            var idStudy = (int) dr["IdStudy"];
            dr.Close();
            return idStudy;
        }

        private void CreateEnrollment(SqlCommand cmd, int idStudy)
        {
            cmd.CommandText = @"INSERT INTO Enrollment 
                (IdEnrollment, Semester, IdStudy, StartDate) 
                VALUES ((SELECT 1+MAX(IdEnrollment) FROM Enrollment), @Semester, @IdStudy, @StartDate);";

            cmd.Parameters.AddWithValue("Semester", 1);
            cmd.Parameters.AddWithValue("IdStudy", idStudy);
            cmd.Parameters.AddWithValue("StartDate", new DateTime());
            cmd.ExecuteNonQuery();
        }

        private Enrollment FindEnrollmentByIdStudy(SqlCommand cmd, int idStudy, int semester)
        {
            cmd.CommandText = @"SELECT IdEnrollment, Semester, StartDate FROM Enrollment WHERE Semester = @Semester AND IdStudy = @IdStudy";

            cmd.Parameters.AddWithValue("IdStudy", idStudy);
            cmd.Parameters.AddWithValue("Semester", semester);
            var dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                throw new EnrollmentNotFoundException();
            }

            var enrollment = new Enrollment
            {
                IdEnrollment = (int) dr["IdEnrollment"],
                Semester = (int) dr["Semester"],
                StartDate = DateTime.Parse(dr["StartDate"].ToString()),
                Study = new Study {IdStudy = idStudy},
            };
            dr.Close();
            return enrollment;
        }

        private Enrollment FindEnrollmentBySemesterAndStudies(SqlCommand cmd, int semester, string studies)
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"SELECT IdEnrollment, Semester, StartDate, S.IdStudy IdStudy FROM Enrollment E 
                    RIGHT JOIN Studies S ON (S.IdStudy = E.IdStudy AND S.Name = @Name) 
                    WHERE Semester = @Semester";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Name", studies);
            cmd.Parameters.AddWithValue("Semester", semester);
            var dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                throw new EnrollmentNotFoundException();
            }

            var enrollment = new Enrollment
            {
                IdEnrollment = (int) dr["IdEnrollment"],
                Semester = (int) dr["Semester"],
                StartDate = DateTime.Parse(dr["StartDate"].ToString()),
                Study = new Study
                {
                    IdStudy = (int) dr["IdStudy"],
                    Name = studies
                },
            };
            dr.Close();
            return enrollment;
        }

        public Enrollment EnrollStudent(CreateEnrollmentDto dto)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;

                var studyId = FindStudyIdByName(cmd, dto.Studies);

                var transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                Enrollment enrollment;
                try
                {
                    enrollment = FindEnrollmentByIdStudy(cmd, studyId, 1);
                }
                catch (EnrollmentNotFoundException e)
                {
                    CreateEnrollment(cmd, studyId);
                    enrollment = FindEnrollmentByIdStudy(cmd, studyId, 1);
                }

                cmd.CommandText = @"INSERT INTO Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) 
                    VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment)";

                cmd.Parameters.AddWithValue("IndexNumber", dto.IndexNumber);
                cmd.Parameters.AddWithValue("FirstName", dto.FirstName);
                cmd.Parameters.AddWithValue("LastName", dto.LastName);
                var birthDate = new SqlParameter("@BirthDate", SqlDbType.Date);
                birthDate.Value = dto.BirthDate;
                cmd.Parameters.Add(birthDate);
                cmd.Parameters.AddWithValue("IdEnrollment", enrollment.IdEnrollment);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    transaction.Rollback();

                    if (isNonUniqueConstraintException(e))
                    {
                        throw new StudentAlreadyExistsException();
                    }

                    throw e;
                }

                transaction.Commit();
                return enrollment;
            }
        }

        public Enrollment Promote(CreatePromotionDto dto)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                
                FindEnrollmentBySemesterAndStudies(cmd, dto.Semester, dto.Studies);
                
                cmd.CommandText = @"promoteStudentsByStudiesAndSemester";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Studies", dto.Studies);
                cmd.Parameters.AddWithValue("@Semester", dto.Semester);
                cmd.ExecuteNonQuery();

                return FindEnrollmentBySemesterAndStudies(cmd, 1 + dto.Semester, dto.Studies);
            }
        }

        private bool isNonUniqueConstraintException(SqlException e)
        {
            // https://stackoverflow.com/questions/24740376/best-way-to-catch-sql-unique-constraint-violations-in-c-sharp-during-inserts
            return e.Number == 2601 || e.Number == 2627;
        }
    }
}