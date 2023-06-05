using Microsoft.AspNetCore.Mvc.Rendering;

namespace Corporation.Models;

public class EmployeeDetailsViewModel
{
    public DateTime DateOfBirth { get; set; }

    public EmployeeModel EmployeeModel { get; set; }

    public IEnumerable<SelectListItem> IdDepartmentGrouping { get; set; }
}