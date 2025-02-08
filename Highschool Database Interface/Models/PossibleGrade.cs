using System;
using System.Collections.Generic;

namespace Highschool_Database_Interface.Models;

public partial class PossibleGrade
{
    public int GradeId { get; set; }

    public string GradeLetter { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
