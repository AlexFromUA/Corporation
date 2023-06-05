namespace Corporation.Models;

public class EmployeeViewModel
{
    public List<DepartmentModel> Departments { get; set; }
    
    public IEnumerable<IGrouping<string,EmployeeModel>> GroupedEmployees { get; set; }
}