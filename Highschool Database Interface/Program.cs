using Highschool_Database_Interface.Models;
using Microsoft.Data.SqlClient;

namespace Highschool_Database_Interface
{
    internal class Program
    {
        private static DatabaseManager dbManager = new DatabaseManager();
        static void Main(string[] args)
        {
            while (true)
            {
                PrintMenuChoices("Välkommen till Highschool Gymnasium databas." +
                        "\nSkriv siffran av det alternativ ni vill göra." +
                        "\n1. Visa antal lärare som jobbar på de olika avdelningarna." +
                        "\n2. Visa alla elever." +
                        "\n3. Visa alla aktiva kurser." +
                        "\n4. Visa alla anställda." +
                        "\n5. Lägg till ny personal." +
                        "\n6. Visa alla betyg för en elev." +
                        "\n7. Visa hur mycket varje avdelning betalar i lön per månad." +
                        "\n8. Visa medel lönen i varje avdelning." +
                        "\n9. Lägg till nytt betyg för en elev." +
                        "\n10. Visa information om specifik elev." +
                        "\n11. Avsluta.");

                int choice = UserMenuChoiceInputReader("Välkommen till Highschool Gymnasium databas." +
                        "\nSkriv siffran av det alternativ ni vill göra." +
                        "\n1. Visa antal lärare som jobbar på de olika avdelningarna." +
                        "\n2. Visa alla elever." +
                        "\n3. Visa alla aktiva kurser." +
                        "\n4. Visa alla anställda" +
                        "\n5. Lägg till ny personal" +
                        "\n6. Visa alla betyg för en elev." +
                        "\n7. Visa hur mycket varje avdelning betalar i lön per månad." +
                        "\n8. Visa medel lönen i varje avdelning." +
                        "\n9. Lägg till nytt betyg för en elev." +
                        "\n10. Visa information om specifik elev." +
                        "\n11. Avsluta");

                if (choice == 1)
                {
                    Console.Clear();
                    TeacherAmountInDepartments();
                }
                else if (choice == 2)
                {
                    Console.Clear();
                    ShowAllStudents();
                }
                else if (choice == 3)
                {
                    Console.Clear();
                    ShowAllActiveCourses();
                }
                else if (choice == 4)
                {
                    Console.Clear();
                    dbManager.ShowAllEmployees();
                }
                else if (choice == 5)
                {
                    Console.Clear();
                    AddEmployee();
                }
                else if (choice == 6)
                {
                    Console.Clear();
                    ShowAStudentsGrades();
                }
                else if (choice == 7)
                {
                    Console.Clear();
                    dbManager.ShowDepartmentWageExepenses();
                }
                else if (choice == 8)
                {
                    Console.Clear();
                    dbManager.ShowAverageWageInDepartments();
                }
                else if (choice == 9)
                {
                    Console.Clear();
                    AddGrade();
                }
                else if (choice == 10)
                {
                    Console.Clear();
                    ShowStudentById();
                }
                else if (choice == 11)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Se till att bara skriva siffror som finns som val.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        public static void PrintMenuChoices(string message)
        {
            Console.WriteLine(message);
        }

        public static int UserMenuChoiceInputReader(string message)
        {
            int choice = 0;
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Det är inte ett giltigt alternativ." +
                    "Se till att bara skriva siffror som finns som alternativ");
                Console.ReadKey();
                Console.Clear();
                PrintMenuChoices(message);
            }
            return choice;
        }

        public static void TeacherAmountInDepartments()
        {
            using (var context = new HighschoolContext())
            {
                var departments = context.Departments;
                var employeeRoles = context.EmployeeRoles;
                var employees = context.Employees
                            .Where(e => e.RoleId == 3)
                            .ToList();

                if (departments != null && employeeRoles != null)
                {
                    foreach (var d in departments)
                    {
                        var employeesInDepartment = employees.Where(e => e.DepartmentId == d.DepartmentId)
                            .ToList();

                        if (employees != null)
                        {
                            Console.WriteLine($"{d.DepartmentName} har {employeesInDepartment.Count} lärare som jobbar där.");
                        }
                        else
                        {
                            Console.WriteLine("Ingen data hittades i employees tabellen.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ingen data hittades i tabellerna.");
                }

                Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static void ShowAllStudents()
        {
            using (var context = new HighschoolContext())
            {
                var students = context.Students
                    .ToList();
                var classes = context.Classes
                    .ToList();

                if (students != null && classes != null)
                {
                    foreach (var student in students)
                    {
                        var studentClass = classes.Where(c => c.ClassId == student.ClassId).FirstOrDefault();

                        Console.WriteLine($"{student.FirstName} {student.LastName}: Pers.nr {student.SocialSecurity}: Klass {studentClass.ClassName}");
                    }
                }
                else
                {
                    Console.WriteLine("Ingen data hittades i någon av tabbellerna.");
                }

                Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static void ShowAllActiveCourses()
        {
            using (var context = new HighschoolContext())
            {
                var courses = context.Courses.ToList();

                if (courses != null)
                {
                    foreach (var course in courses)
                    {
                        if (course.CourseActive)
                        {
                            Console.WriteLine(course.CourseName);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ingen data hittades i tabellen.");
                }

                Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static void AddEmployee()
        {
            while (true)
            {
                string roleChoices = "Vilken roll har den nya anställda?\n";
                roleChoices += dbManager.ShowAllRolesAsChoices();
                string departmentChoices = "Vilken avdelning jobbar den nya anställda på?\n";
                departmentChoices += dbManager.ShowAllDepartmentsAsChoices();
                int roleId = 0;
                string firstName = "";
                string lastName = "";
                decimal salary = 0;
                int departmentId = 0;
                bool innerLoopQuit = false;

                Console.WriteLine(roleChoices);
                roleId = UserMenuChoiceInputReader(roleChoices);
                Console.Clear();

                while (true)
                {
                    Console.WriteLine("Vad är den nya anställdas förnamn? Skriv QUIT om du vill avsluta.");
                    firstName = Console.ReadLine();

                    if (firstName == "")
                    {
                        Console.WriteLine("Du måste skriva något. Tryck enter för att gå tillbaka och försök igen.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                    else if (firstName.ToUpper().Equals("QUIT"))
                    {
                        Console.Clear();
                        innerLoopQuit = true;
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Är {firstName} det namn du ville skriva in? Skriva Y för ja och N för nej");
                        string correctInputFirstName = Console.ReadLine();

                        if (correctInputFirstName.ToUpper().Equals("Y"))
                        {
                            Console.Clear();
                            break;
                        }
                        else if (correctInputFirstName.ToUpper().Equals("N"))
                        {
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Ogiltig input, se till att bara antingen skriva Y eller N. " +
                                "Tryck Enter för att skriva in namnet igen.");
                            Console.ReadKey();
                            Console.Clear();
                            continue;
                        }
                    }
                }

                if (innerLoopQuit)
                {
                    continue;
                }

                while (true)
                {
                    Console.WriteLine("Vad är den nya anställdas efternamn? Skriv QUIT om du vill avsluta.");
                    lastName = Console.ReadLine();

                    if (firstName == "")
                    {
                        Console.WriteLine("Du måste skriva något. Tryck enter för att gå tillbaka och försök igen.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                    else if (firstName.ToUpper().Equals("QUIT"))
                    {
                        Console.Clear();
                        innerLoopQuit = true;
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Är {lastName} det namn du ville skriva in? Skriva Y för ja och N för nej");
                        string correctInputLastName = Console.ReadLine();

                        if (correctInputLastName.ToUpper().Equals("Y"))
                        {
                            Console.Clear();
                            break;
                        }
                        else if (correctInputLastName.ToUpper().Equals("N"))
                        {
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Ogiltig input, se till att bara antingen skriva Y eller N. " +
                                "Tryck Enter för att skriva in namnet igen.");
                            Console.ReadKey();
                            Console.Clear();
                            continue;
                        }
                    }
                }

                if (innerLoopQuit)
                {
                    continue;
                }

                Console.WriteLine("Vad är den nya anställdas lön?");
                while (!decimal.TryParse(Console.ReadLine(), out salary))
                {
                    Console.WriteLine("Fel input. Skriv bara siffror.");
                    Console.ReadKey();
                    Console.Clear();
                    PrintMenuChoices("Vad är den nya anställdas lön?");
                }
                Console.Clear();

                Console.WriteLine(departmentChoices);
                departmentId = UserMenuChoiceInputReader(departmentChoices);
                Console.Clear();

                dbManager.AddEmployee(roleId, firstName, lastName, salary, departmentId);
                Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
                Console.ReadKey();
                Console.Clear();
                break;
            }
        }

        public static void ShowAStudentsGrades()
        {
            int studentId = 0;
            string studentChoices = "Vilken student vill du se betygen för?\n";
            studentChoices += dbManager.ShowAllStudentsAsChoices();

            Console.WriteLine(studentChoices);
            studentId = UserMenuChoiceInputReader(studentChoices);
            Console.Clear();

            dbManager.ShowAllGradesForAStudent(studentId);
        }

        public static void AddGrade()
        {
            while(true)
            {
                string studentChoices = "Vilken student vill du sätta betyg för?\n";
                studentChoices += dbManager.ShowAllStudentsAsChoices();
                string teacherChoices = "Vem är det som sätter betyget?\n";
                teacherChoices += dbManager.ShowAllTeachersAsChoices();
                string subjectChoices = "Vilket ämne är betyget i?\n";
                subjectChoices += dbManager.ShowAllSubjectsAsChoices();
                string gradeChoices = "Vilket betyg fick eleven?\n";
                gradeChoices += dbManager.ShowAllPossibleGradesAsChoices();
                int studentId = 0;
                int teacherId = 0;
                int subjectId = 0;
                int gradeId = 0;
                bool innerLoopContinue = false;
                bool innerLoopQuit = false;

                Console.WriteLine(studentChoices);
                studentId = UserMenuChoiceInputReader(studentChoices);
                Console.Clear();

                while (true)
                {
                    Console.WriteLine($"Är {studentId} det id:et som du ville välja? " +
                        $"Skriv Y om det är korrekt. Skriv N om det är fel och du vill börja om valen. Skriv QUIT om du vill avsluta och gå tillbaka till start menyn.");
                    string correctStudentInput = Console.ReadLine();

                    if(correctStudentInput.ToUpper().Equals("Y"))
                    {
                        Console.Clear();
                        break;
                    }
                    else if(correctStudentInput.ToUpper().Equals("N"))
                    {
                        Console.Clear();
                        innerLoopContinue = true;
                        break;
                    }
                    else if(correctStudentInput.ToUpper().Equals("QUIT"))
                    {
                        Console.Clear();
                        innerLoopQuit = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig input, se till att bara antingen skriva Y eller N. " +
                            "Tryck Enter för att skriva in namnet igen.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }

                if(innerLoopQuit)
                {
                    break;
                }
                else if(innerLoopContinue)
                {
                    continue;
                }

                Console.WriteLine(teacherChoices);
                teacherId = UserMenuChoiceInputReader(teacherChoices);
                Console.Clear();

                while (true)
                {
                    Console.WriteLine($"Är {teacherId} det id:et som du ville välja? " +
                        $"Skriv Y om det är korrekt. Skriv N om det är fel och du vill börja om valen. Skriv QUIT om du vill avsluta och gå tillbaka till start menyn.");
                    string correctTeacherInput = Console.ReadLine();

                    if (correctTeacherInput.ToUpper().Equals("Y"))
                    {
                        Console.Clear();
                        break;
                    }
                    else if (correctTeacherInput.ToUpper().Equals("N"))
                    {
                        Console.Clear();
                        innerLoopContinue = true;
                        break;
                    }
                    else if (correctTeacherInput.ToUpper().Equals("QUIT"))
                    {
                        Console.Clear();
                        innerLoopQuit = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig input, se till att bara antingen skriva Y eller N. " +
                            "Tryck Enter för att skriva in namnet igen.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }

                if (innerLoopQuit)
                {
                    break;
                }
                else if (innerLoopContinue)
                {
                    continue;
                }

                Console.WriteLine(subjectChoices);
                subjectId = UserMenuChoiceInputReader(subjectChoices);
                Console.Clear();

                while (true)
                {
                    Console.WriteLine($"Är {subjectId} det id:et som du ville välja? " +
                        $"Skriv Y om det är korrekt. Skriv N om det är fel och du vill börja om valen. Skriv QUIT om du vill avsluta och gå tillbaka till start menyn.");
                    string correctSubjectInput = Console.ReadLine();

                    if (correctSubjectInput.ToUpper().Equals("Y"))
                    {
                        Console.Clear();
                        break;
                    }
                    else if (correctSubjectInput.ToUpper().Equals("N"))
                    {
                        Console.Clear();
                        innerLoopContinue = true;
                        break;
                    }
                    else if (correctSubjectInput.ToUpper().Equals("QUIT"))
                    {
                        Console.Clear();
                        innerLoopQuit = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig input, se till att bara antingen skriva Y eller N. " +
                            "Tryck Enter för att skriva in namnet igen.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }

                if (innerLoopQuit)
                {
                    break;
                }
                else if (innerLoopContinue)
                {
                    continue;
                }

                Console.WriteLine(gradeChoices);
                gradeId = UserMenuChoiceInputReader(gradeChoices);
                Console.Clear();

                while (true)
                {
                    Console.WriteLine($"Är {gradeId} det id:et som du ville välja? " +
                        $"Skriv Y om det är korrekt. Skriv N om det är fel och du vill börja om valen. Skriv QUIT om du vill avsluta och gå tillbaka till start menyn.");
                    string correctGradeInput = Console.ReadLine();

                    if (correctGradeInput.ToUpper().Equals("Y"))
                    {
                        Console.Clear();
                        break;
                    }
                    else if (correctGradeInput.ToUpper().Equals("N"))
                    {
                        Console.Clear();
                        innerLoopContinue = true;
                        break;
                    }
                    else if (correctGradeInput.ToUpper().Equals("QUIT"))
                    {
                        Console.Clear();
                        innerLoopQuit = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig input, se till att bara antingen skriva Y eller N. " +
                            "Tryck Enter för att skriva in namnet igen.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }
                }

                if (innerLoopQuit)
                {
                    break;
                }
                else if (innerLoopContinue)
                {
                    continue;
                }

                dbManager.AddGrade(studentId, teacherId, subjectId, gradeId);
                Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
                Console.ReadKey();
                Console.Clear();
                break;
            }
        }

        public static void ShowStudentById()
        {
            int studentId = 0;
            string studentChoices = "Vilken student vill du har informationen om?\n";
            studentChoices += dbManager.ShowAllStudentsAsChoices();

            Console.WriteLine(studentChoices);
            studentId = UserMenuChoiceInputReader(studentChoices);
            Console.Clear();

            dbManager.ShowStudentById(studentId);
        }
    }
}
