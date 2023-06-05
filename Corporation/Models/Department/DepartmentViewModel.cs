namespace Corporation.Models;

public class DepartmentViewModel
{
    public IEnumerable<IGrouping<string, DepartmentModel>> GroupedDepartments { get; set; }
    
    public List<DepartmentModel> Departments { get; set; }
}