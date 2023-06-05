using Microsoft.Build.Framework;

namespace Corporation.Models;

public class EmployeeModel
{
    public int Id { get; set; }
    
    public string DepartmentId { get; set; }
    
    public string Surname { get; set; }
    
    public string FirstName { get; set; }
    
    public string? Patronymic { get; set; }
    
    public string DateOfBirth { get; set; }
    
    public string? DocSeries { get; set; }
    
    public string? DocNumber { get; set; }
    
    public string Position { get; set; }

    public int Age { get; set; }
}