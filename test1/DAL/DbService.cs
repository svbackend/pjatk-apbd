using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using APBD3.Exceptions;
using APBD3.Models;

namespace APBD3.DAL
{
    public class DbService : IDbService
    {
        private static string connectionString =
            "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s19319;Integrated Security=True;Trusted_Connection=true;";

        public TeamMember GetTeamMember(int id)
        {
            var sql = @"SELECT * FROM TeamMember WHERE IdTeamMember = @IdTeamMember";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("IdTeamMember", id);
                
                var dr = cmd.ExecuteReader();
                
                if (!dr.Read())
                {
                    throw new TeamMemberNotFound();
                }
                
                TeamMember teamMember = new TeamMember
                {
                    IdTeamMember = dr["IdTeamMember"].ToString(),
                    Email = dr["Email"].ToString(),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    AssignedTasks = GetAssignedTasks(id),
                    CreatedTasks = GetCreatedTasks(id),
                };
                
                Console.WriteLine(teamMember.AssignedTasks.ToList().Count);
                Console.WriteLine(teamMember.CreatedTasks.ToList().Count);

                return teamMember;
            }
        }
        
        public IEnumerable<Task> GetAssignedTasks(int teamMemberId)
        {
            var tasks = new List<Task>();

            var sql = @"SELECT T.Name Name, T.Description Description, T.Deadline Deadline, TT.Name Type, P.Name ProjectName
                FROM Task T
                LEFT JOIN TaskType TT ON T.IdTaskType = TT.IdTaskType
                LEFT JOIN Project P on T.IdProject = P.IdProject
                WHERE IdAssignedTo = @IdAssignedTo
                ORDER BY T.Deadline DESC";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("IdAssignedTo", teamMemberId);
                
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    tasks.Add(new Task()
                    {
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        Deadline = DateTime.Parse(dr["Deadline"].ToString()),
                        Type = dr["Type"].ToString(),
                        ProjectName = dr["ProjectName"].ToString(),
                    });
                }
            }

            return tasks;
        }
        
        public IEnumerable<Task> GetCreatedTasks(int teamMemberId)
        {
            var tasks = new List<Task>();

            var sql = @"SELECT T.Name Name, T.Description Description, T.Deadline Deadline, TT.Name Type, P.Name ProjectName
                FROM Task T
                LEFT JOIN TaskType TT ON T.IdTaskType = TT.IdTaskType
                LEFT JOIN Project P on T.IdProject = P.IdProject
                WHERE IdCreator = @IdCreator
                ORDER BY T.Deadline DESC";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("IdCreator", teamMemberId);
                
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    tasks.Add(new Task()
                    {
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        Deadline = DateTime.Parse(dr["Deadline"].ToString()),
                        Type = dr["Type"].ToString(),
                        ProjectName = dr["ProjectName"].ToString(),
                    });
                }
            }

            return tasks;
        }

        public void DeleteProject(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;

                IsProjectExists(id);

                var transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;

                cmd.CommandText = @"DELETE FROM Task WHERE IdProject = @IdProject; DELETE FROM Project WHERE IdProject = @IdProject;";

                cmd.Parameters.AddWithValue("IdProject", id);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    transaction.Rollback();

                    throw e;
                }

                transaction.Commit();
            }
        }
        
        public bool IsProjectExists(int id)
        {
            var sql = @"SELECT 1 FROM Project WHERE IdProject = @IdProject";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("IdProject", id);
                
                var dr = cmd.ExecuteReader();
                
                if (!dr.Read())
                {
                    throw new ProjectNotFound();
                }

                return true;
            }
        }
    }
}