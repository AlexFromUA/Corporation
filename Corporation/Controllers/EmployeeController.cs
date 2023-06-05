using Corporation.Interfaces;
using Corporation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Corporation.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            List<EmployeeModel> employees = await _employeeService.GetEmployees();
            var groupedEmployees = employees.GroupBy(employee => employee.DepartmentId);

            List<DepartmentModel> departments = await _departmentService.GetDepartments();

            var employeeViewModel = new EmployeeViewModel
            {
                GroupedEmployees = groupedEmployees,
                Departments = departments
            };
            return View("Employee", employeeViewModel);
        }
        catch (Exception ex)
        {
            return RedirectToAction("ErrorOccured", "Error");
        }
    }

    public async Task<IActionResult> GetEmployee(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployee(id);
            var departments = await _departmentService.GetDepartments();
            var selectListItems = new List<SelectListItem>();
            departments.ForEach(department =>
                selectListItems.Add(new SelectListItem { Text = department.Name, Value = department.Id }));
            EmployeeDetailsViewModel model = new EmployeeDetailsViewModel
            {
                DateOfBirth = Convert.ToDateTime(employee.DateOfBirth),
                IdDepartmentGrouping = selectListItems,
                EmployeeModel = employee
            };

            return View("EmployeeDetails", model);
        }
        catch (Exception ex)
        {
            return RedirectToAction("ErrorOccured", "Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> CreateEmployee(string Id)
    {
        try
        {
            var departments = await _departmentService.GetDepartments();
            var selectListItems = new List<SelectListItem>();
            departments.ForEach(department => selectListItems.Add(new SelectListItem
                { Text = department.Name, Value = department.Id, Selected = department.Id == Id }));

            EmployeeCreateViewModel model = new EmployeeCreateViewModel
            {
                IdDepartmentGrouping = selectListItems
            };
            return View("EmployeeCreate", model);
        }
        catch (Exception ex)
        {
            return RedirectToAction("ErrorOccured", "Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(EmployeeCreateViewModel model)
    {
        if ((int)((DateTime.Now - Convert.ToDateTime(model.EmployeeModel.DateOfBirth)).TotalDays / 365.242199) > 18)
        {
            try
            {
                _employeeService.CreateEmployee(model.EmployeeModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorOccured", "Error");
            }
        }
        return RedirectToAction("CreateEmployee", new { id = model.EmployeeModel.DepartmentId });
    }

    public async Task<IActionResult> UpdateEmployee(EmployeeDetailsViewModel model)
    {
        if ((int)((DateTime.Now - model.DateOfBirth).TotalDays / 365.242199) > 18)
        {
            try
            {
                var employeeToUpdate = model.EmployeeModel;
                employeeToUpdate.DateOfBirth = model.DateOfBirth.ToString();

                _employeeService.UpdateEmployee(employeeToUpdate);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return RedirectToAction("ErrorOccured", "Error");
            }
        }

        return RedirectToAction("GetEmployee", new { id = model.EmployeeModel.Id });
    }

    public ActionResult DeleteEmployee(int id)
    {
        try
        {
            _employeeService.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return RedirectToAction("ErrorOccured", "Error");
        }
    }
}