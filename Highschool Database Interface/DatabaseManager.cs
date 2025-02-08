using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Highschool_Database_Interface
{
    internal class DatabaseManager
    {
        private static readonly string _connectionString = @"Data Source = localhost;Initial Catalog = Highschool;Integrated Security = true;Trust Server Certificate = true;";

        public void ShowAllEmployees()
        {
            string query = "SELECT" +
                "\nemployees.first_name + ' ' + employees.last_name AS 'Name'," +
                "\nemployee_roles.role_name AS Job," +
                "\ndatediff(year, employees.dateStarted, getdate()) AS 'Years Worked'" +
                "\nFROM employees" +
                "\nJOIN employee_roles ON employees.role_id = employee_roles.role_id";
            ExecuteQuery(query);
            Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
            Console.ReadKey();
            Console.Clear();
        }

        private void ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetName(i) + "\t");
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader[i].ToString() + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();
            }
        }

        private void ExecuteCommand(SqlCommand command)
        {
            try
            {
                var rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " Rader påverkade.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private SqlCommand AddCommandParameters(string[,] parameters, SqlCommand command)
        {
            for (int i = 0; i < parameters.GetLength(0); i++)
            {
                command.Parameters.AddWithValue(parameters[i, 0], parameters[i, 1]);
            }

            return command;
        }

        public void AddEmployee(int roleId, string firstName, string lastName, decimal salary, int departmentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO employees (role_id, first_name, last_name, salary, department_id) VALUES
                                (@role_id, @first_name, @last_name, @salary, @department_id)";
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                string[,] parameters =
                    { {"@role_id", $"{roleId}"}, {"@first_name", firstName}, {"@last_name", lastName}, {"@salary", $"{salary}"}, {"@department_id", $"{departmentId}"} };

                command = AddCommandParameters(parameters, command);

                using (command)
                {
                    ExecuteCommand(command);
                }
            }
        }

        public void ShowAllGradesForAStudent(int studentId)
        {
            string query = "SELECT" +
                "\npossible_grades.grade_letter AS Grade," +
                "\nsubjects.subject_name AS Subject," +
                "\nemployees.first_name + ' ' + employees.last_name AS Teacher," +
                "\ngrades.grade_set AS 'Set'" +
                "\nFROM grades" +
                "\nJOIN possible_grades ON grades.student_grade_id = possible_grades.grade_id" +
                "\nJOIN subjects ON grades.subject_id = subjects.subject_id" +
                "\nJOIN employees ON grades.teacher_id = employees.employee_id" +
                $"\nWHERE grades.student_id = {studentId}";

            ExecuteQuery(query);

            Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
            Console.ReadKey();
            Console.Clear();
        }

        public void ShowDepartmentWageExepenses()
        {
            string query = "SELECT" +
                "\ndepartments.department_name AS Department," +
                "\nSUM(employees.salary) AS 'WageExpenses'" +
                "\nFROM employees" +
                "\nJOIN departments ON employees.department_id = departments.department_id" +
                "\nGROUP BY departments.department_name";

            ExecuteQuery(query);

            Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
            Console.ReadKey();
            Console.Clear();
        }

        public void ShowAverageWageInDepartments()
        {
            string query = "SELECT" +
                "\ndepartments.department_name AS Department," +
                "\nCAST(ROUND(AVG(employees.salary), 0) AS INT) AS 'Average_Wage'" +
                "\nFROM employees" +
                "\nJOIN departments ON employees.department_id = departments.department_id" +
                "\nGROUP BY departments.department_name";

            ExecuteQuery(query);

            Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
            Console.ReadKey();
            Console.Clear();
        }

        public void ShowStudentById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string procedure = "GetStudentById";

                SqlCommand command = new SqlCommand(procedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                string[,] parameters =
                    {{"@Id", $"{id}"}};

                command = AddCommandParameters(parameters, command);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetName(i) + "\t");
                        }
                        Console.WriteLine();
                        Console.WriteLine();
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader[i].ToString() + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine("Tryck på valfri knapp för att gå tillbaka till start menyn.");
                    Console.ReadKey();
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                connection.Close();
            }
        }

        public void AddGrade(int studentId, int teacherId, int subjectId, int studentGradeId)
        {
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"INSERT INTO grades(student_id, teacher_id, subject_id, student_grade_id) VALUES
                                    (@StudentId, @TeacherId, @SubjectId, @StudentGradeId)";

                    string[,] parameters =
                        { {"@StudentId", $"{studentId}"}, {"@TeacherId", $"{teacherId}"}, {"@SubjectId", $"{subjectId}"}, {"@StudentGradeId", $"{studentGradeId}"} };

                    command = AddCommandParameters(parameters, command);

                    using(command)
                    {
                        ExecuteCommand(command);
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    try
                    {
                        transaction.Rollback();
                    }
                    catch(Exception exRollback)
                    {
                        Console.WriteLine(exRollback.Message);
                    }
                }
            }
        }

        private string QueryToChoices(string query)
        {
            string choices = "";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (i % 2 != 0)
                                {
                                    choices += reader[i].ToString() + "\n";
                                }
                                else
                                {
                                    choices += reader[i].ToString() + ". ";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return choices;
            }
        }

        public string ShowAllRolesAsChoices()
        {
            string query = "SELECT * FROM employee_roles";
            string allRoles = QueryToChoices(query);
            return allRoles;
        }

        public string ShowAllDepartmentsAsChoices()
        {
            string query = "SELECT * FROM departments";
            string allDepartments = QueryToChoices(query);
            return allDepartments;
        }

        public string ShowAllStudentsAsChoices()
        {
            string query = "SELECT" +
                "\nstudents.student_id AS Id," +
                "\nstudents.first_name + ' ' + students.last_name AS 'Name'" +
                "\nFROM students";
            string allStudents = QueryToChoices(query);
            return allStudents;
        }

        public string ShowAllTeachersAsChoices()
        {
            string query = "SELECT" +
                "\nemployees.employee_id AS Id," +
                "\nemployees.first_name + ' ' + employees.last_name AS 'Name'" +
                "\nFROM employees" +
                "\nWHERE employees.role_id = 3";
            string allStudents = QueryToChoices(query);
            return allStudents;
        }

        public string ShowAllSubjectsAsChoices()
        {
            string query = "SELECT" +
                "\nsubjects.subject_id AS Id," +
                "\nsubjects.subject_name AS Subject" +
                "\nFROM subjects";
            string allSubjects = QueryToChoices(query);
            return allSubjects;
        }

        public string ShowAllPossibleGradesAsChoices()
        {
            string query = "SELECT" +
                "\npossible_grades.grade_id AS Id," +
                "\npossible_grades.grade_letter AS Grade" +
                "\nFROM possible_grades";
            string allPossibleGrades = QueryToChoices(query);
            return allPossibleGrades;
        }
    }
}
