using Microsoft.AspNetCore.Mvc.Rendering;

namespace Corporation.Models;

public class EmployeeCreateViewModel
{
    public EmployeeModel EmployeeModel { get; set; }
    
    public IEnumerable<SelectListItem> IdDepartmentGrouping { get; set; }
}