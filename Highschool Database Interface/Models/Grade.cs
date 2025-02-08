using System;
using System.Collections.Generic;

namespace Highschool_Database_Interface.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? StudentId { get; set; }

    public int? TeacherId { get; set; }

    public DateOnly GradeSet { get; set; }

    public int? SubjectId { get; set; }

    public int? StudentGradeId { get; set; }

    public virtual Student? Student { get; set; }

    public virtual PossibleGrade? StudentGrade { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Employee? Teacher { get; set; }
}
