using System;
using System.Collections.Generic;

namespace Highschool_Database_Interface.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int? RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public decimal? Salary { get; set; }

    public int? DepartmentId { get; set; }

    public DateOnly? DateStarted { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual EmployeeRole? Role { get; set; }
}
