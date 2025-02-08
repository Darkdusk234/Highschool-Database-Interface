using System;
using System.Collections.Generic;

namespace Highschool_Database_Interface.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int? ClassId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string SocialSecurity { get; set; } = null!;

    public virtual Class? Class { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
