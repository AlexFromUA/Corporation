using Corporation.Interfaces;
using Corporation.Models;
using Microsoft.Data.SqlClient;

namespace Corporation.Services;

public class EmployeeBaseService : BaseService, IEmployeeService
{
    private List<EmployeeModel>? _employees;
    private bool _hasChanges = true;
    
    public EmployeeBaseService(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<List<EmployeeModel>> GetEmployees()
    {
        if (_hasChanges || _employees == null)
        {
            _employees = new List<EmployeeModel>();

            string queryString = "SELECT * from dbo.Empoyee;";
            await using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EmployeeModel employee = new EmployeeModel
                        {
                            Id = Convert.ToInt32(reader[0]),
                            DepartmentId = reader[8].ToString()!,
                            Surname = reader[2].ToString()!,
                            FirstName = reader[1].ToString()!,
                            Patronymic = reader[3] != null ? reader[3].ToString()! : null,
                            DateOfBirth = reader[4].ToString()!,
                            DocSeries = reader[5] != null ? reader[5].ToString()! : null,
                            DocNumber = reader[6] != null ? reader[6].ToString()! : null,
                            Position = reader[7].ToString()!,
                            Age = (int)((DateTime.Now - Convert.ToDateTime(reader[4])).TotalDays / 365.242199)
                        };
                        _employees.Add(employee);
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

        return _employees;
    }

    public async Task<EmployeeModel> GetEmployee(int employeeId)
    {
        EmployeeModel employee = null;
        if (_employees == null)
        {
            await GetEmployees();
        }

        employee = _employees.SingleOrDefault(e => e.Id == employeeId);

        return employee;
    }

    public async void CreateEmployee(EmployeeModel model)
    {
        string queryString =
            "INSERT INTO dbo.Empoyee ([FirstName],[SurName],[Patronymic],[DateOfBirth],[DocSeries],[DocNumber],[Position],[DepartmentID]) VALUES (@FirstName, @SurName, @Patronymic, @DateOfBirth, @DocSeries, @DocNumber, @Position, @DepartmentID);";
        await using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@FirstName", model.FirstName);
            command.Parameters.AddWithValue("@SurName", model.Surname);
            command.Parameters.AddWithValue("@Patronymic", model.Patronymic ?? Convert.DBNull);
            command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
            command.Parameters.AddWithValue("@DocSeries", model.DocSeries ?? Convert.DBNull);
            command.Parameters.AddWithValue("@DocNumber", model.DocNumber ?? Convert.DBNull);
            command.Parameters.AddWithValue("@Position", model.Position);
            command.Parameters.AddWithValue("@DepartmentID", model.DepartmentId);


            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _hasChanges = true;
        }
    }

    public async void UpdateEmployee(EmployeeModel model)
    {
        string queryString =
            "UPDATE dbo.Empoyee SET [FirstName] = @FirstName,[SurName] = @SurName,[Patronymic] = @Patronymic,[DateOfBirth] = @DateOfBirth,[DocSeries] = @DocSeries,[DocNumber] = @DocNumber,[Position] = @Position,[DepartmentID]= @DepartmentID WHERE [ID] = @Id;";
        await using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", model.Id);
            command.Parameters.AddWithValue("@FirstName", model.FirstName);
            command.Parameters.AddWithValue("@SurName", model.Surname);
            command.Parameters.AddWithValue("@Patronymic", model.Patronymic ?? Convert.DBNull);
            command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
            command.Parameters.AddWithValue("@DocSeries", model.DocSeries ?? Convert.DBNull);
            command.Parameters.AddWithValue("@DocNumber", model.DocNumber ?? Convert.DBNull);
            command.Parameters.AddWithValue("@Position", model.Position);
            command.Parameters.AddWithValue("@DepartmentID", model.DepartmentId);


            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _hasChanges = true;
        }
    }

    public async void DeleteEmployee(int employeeId)
    {
        string queryString = "DELETE from dbo.Empoyee WHERE ID = @employeeId;";
        await using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@employeeId", employeeId);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _hasChanges = true;
        }
    }
}