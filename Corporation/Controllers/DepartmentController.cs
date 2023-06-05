using Corporation.Interfaces;
using Corporation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Corporation.Controllers;

public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly IEmployeeService _employeeService;

    public DepartmentController(IDepartmentService departmentService, IEmployeeService employeeService)
    {
        _departmentService = departmentService;
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var departments = await _departmentService.GetDepartments();
            var groupedDepartments = departments.GroupBy(employee => employee.ParentDepartmentId);

            var departmentViewModel = new DepartmentViewModel
            {
                Departments = departments,
                GroupedDepartments = groupedDepartments
            };
            return View("Department", departmentViewModel);
        }
        catch (Exception ex)
        {
            return RedirectToAction("ErrorOccured", "Error");
        }
    }

    public async Task<IActionResult> GetDepartment(string id)
    {
        try
        {
            var currentDepartment = await _departmentService.GetDepartment(id);
            var departments = await _departmentService.GetDepartments();
            var childDepartments = departments.Where(department => department.ParentDepartmentId == id).ToList();
            var employees = await _employeeService.GetEmployees();
            var workersOfDepartment = employees.Where(employee => employee.DepartmentId == id).ToList();
            var parentDepartment =
                departments.FirstOrDefault(department => department.Id == currentDepartment.ParentDepartmentId)?.Name;

            DepartmentDetailsModelView model = new DepartmentDetailsModelView
            {
                Department = currentDepartment,
                Workers = workersOfDepartment,
                ParentDepartmentName = parentDepartment,
                ChildDepartments = childDepartments
            };
            return View("DepartmentDetails", model);
        }
        catch (Exception ex)
        {
            return RedirectToAction("ErrorOccured", "Error");
        }
    }
}