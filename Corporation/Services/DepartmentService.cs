using Corporation.Interfaces;
using Corporation.Models;
using Microsoft.Data.SqlClient;

namespace Corporation.Services;

public class DepartmentService : Service, IDepartmentService
{
    private List<DepartmentModel> departments;
    private bool hasChanges = true;

    public async Task<List<DepartmentModel>> GetDepartments()
    {
        if (hasChanges || departments == null)
        {
            departments = new List<DepartmentModel>();

            string queryString = "SELECT * from dbo.Department;";
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DepartmentModel department = new DepartmentModel
                        {
                            Id = reader[0].ToString(),
                            ParentDepartmentId = reader[3] != null ? reader[3].ToString()! : null,
                            Code = reader[2] != null ? reader[2].ToString()! : null,
                            Name = reader[1].ToString()!
                        };

                        departments.Add(department);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            hasChanges = false;
        }

        return departments;
    }

    public async Task<DepartmentModel> GetDepartment(string departmentId)
    {
        DepartmentModel department = null;

        if (departments == null)
        {
            await GetDepartments();
        }

        department = departments.SingleOrDefault(e => e.Id == departmentId);

        return department;
    }
}