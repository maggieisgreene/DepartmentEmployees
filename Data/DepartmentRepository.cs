using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using DepartmentsEmployees.Models;

namespace DepartmentsEmployees.Data
{
    public class DepartmentRepository
    {
        public SqlConnection Connection
        {
            get
            {
                // This is "address" of the database
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DepartmentsEmployees;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }

        public List<Department> GetAllDepartments()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, DeptName
                    FROM Department";
                 

                SqlDataReader reader = cmd.ExecuteReader();

                List<Department> allDepartments = new List<Department>();

                while (reader.Read())
                {
                    int idColumn = reader.GetOrdinal("Id");
                    int idValue = reader.GetInt32(idColumn);

                    int deptNameColumn = reader.GetOrdinal("DeptName");
                    string deptNameValue = reader.GetInt32(deptNameColumn);

                    var department = new Department()
                    {
                        Id = idValue,
                        DeptName = deptNameValue
                    };

                    allDepartments.Add(department);
                }

                reader.Close();

                return allDepartments;
                }
            }
        }

        public Department GetDepartmentById(int departmentId)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT DeptName WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", departmentId));

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int idColumn = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumn);

                        int deptNameColumn = reader.GetOrdinal("DeptName");
                        string deptNameValue = reader.GetInt32(deptNameColumn);

                        var department = new Department()
                        {
                            Id = idValue,
                            DeptName = deptNameValue
                        };

                        reader.Close();
                        
                        return department;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public Department CreateNewDepartment(Department departmentToAdd)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Department (DeptName)
                    OUTPUT INSERTED.Id
                    VALUES (@deptName)";

                    cmd.Parameters.Add(new SqlParameter("@deptName", departmentToAdd.DeptName));

                    int id = (int)cmd.ExecuteScalar();

                    departmentToAdd.Id = id;

                    return departmentToAdd;
                }
            }
        }

        public void UpdateDepartment(int departmentId, Department department)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    UPDATE Department
                    SET DeptName = @deptName
                    WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@deptName", department.DeptName));
                    cmd.Parameters.Add(new SqlParameter("@id", department.Id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Department WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", departmentId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}