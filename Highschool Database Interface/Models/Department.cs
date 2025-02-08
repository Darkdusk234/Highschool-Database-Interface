using System;
using System.Collections.Generic;

namespace Highschool_Database_Interface.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
