namespace Corporation.Models;

public class DepartmentDetailsModelView
{
    public DepartmentModel Department { get; set; }

    public string? ParentDepartmentName { get; set; }

    public List<EmployeeModel> Workers { get; set; }
    
    public List<DepartmentModel> ChildDepartments { get; set; }
}