using System;
using System.Collections.Generic;

namespace Highschool_Database_Interface.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public bool CourseActive { get; set; }

    public virtual Department? Department { get; set; }
}
