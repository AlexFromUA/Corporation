using Corporation.Models;

namespace Corporation.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentModel>> GetDepartments();

    Task<DepartmentModel> GetDepartment(string departmentId);
}