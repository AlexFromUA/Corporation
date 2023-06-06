using Corporation.Interfaces;
using Corporation.Models;
using Microsoft.Data.SqlClient;

namespace Corporation.Services;

public class DepartmentBaseService : BaseService, IDepartmentService
{
    private List<DepartmentModel>? _departments;
    private bool _hasChanges = true;

    public DepartmentBaseService(IConfiguration configuration) : base(configuration)
    {
    }
    
    public async Task<List<DepartmentModel>> GetDepartments()
    {
        if (_hasChanges || _departments == null)
        {
            _departments = new List<DepartmentModel>();

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

                        _departments.Add(department);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            _hasChanges = false;
        }

        return _departments;
    }

    public async Task<DepartmentModel> GetDepartment(string departmentId)
    {
        DepartmentModel department = null;

        if (_departments == null)
        {
            await GetDepartments();
        }

        department = _departments.SingleOrDefault(e => e.Id == departmentId);

        return department;
    }
}