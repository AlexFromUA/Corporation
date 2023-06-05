namespace Corporation.Models;

public class DepartmentModel
{
    public string Id { get; set; }
    
    public string? ParentDepartmentId { get; set; }
    
    public string? Code { get; set; }
    
    public string Name { get; set; }
}