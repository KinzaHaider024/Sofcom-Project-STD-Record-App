using MySqlConnector;
using StudentRecordApp.Data;
using StudentRecordApp.Models;

namespace StudentRecordApp
{
    class Program
    {
        static Database database = new Database();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("================================");
                Console.WriteLine("       STUDENT RECORD APP");
                Console.WriteLine("================================");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Exit");
                Console.WriteLine("================================");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;

                    case "2":
                        ViewStudents();
                        break;

                    case "3":
                        UpdateStudent();
                        break;

                    case "4":
                        DeleteStudent();
                        break;

                    case "5":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
        }

        static void AddStudent()
        {
            Console.Clear();

            Console.WriteLine("Add Student");

            Student student = new Student();

            Console.Write("Enter Name: ");
            student.Name = Console.ReadLine();

            Console.Write("Enter Age: ");
            student.Age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Email: ");
            student.Email = Console.ReadLine();

            Console.Write("Enter Department: ");
            student.Department = Console.ReadLine();

            Console.Write("Enter Semester: ");
            student.Semester = Convert.ToInt32(Console.ReadLine());

            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query = @"INSERT INTO Students
                                 (Name, Age, Email, Department, Semester)
                                 VALUES
                                 (@Name, @Age, @Email, @Department, @Semester)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@Age", student.Age);
                    command.Parameters.AddWithValue("@Email", student.Email);
                    command.Parameters.AddWithValue("@Department", student.Department);
                    command.Parameters.AddWithValue("@Semester", student.Semester);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("\nStudent added successfully!");
            Pause();
        }

        static void ViewStudents()
        {
            Console.Clear();

            Console.WriteLine("Student Records");

            List<Student> students = new List<Student>();

            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Students";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Student student = new Student
                            {
                                StudentId = Convert.ToInt32(reader["StudentId"]),
                                Name = reader["Name"].ToString(),
                                Age = Convert.ToInt32(reader["Age"]),
                                Email = reader["Email"].ToString(),
                                Department = reader["Department"].ToString(),
                                Semester = Convert.ToInt32(reader["Semester"])
                            };

                            students.Add(student);
                        }
                    }
                }
            }

            foreach (Student student in students)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"ID: {student.StudentId}");
                Console.WriteLine($"Name: {student.Name}");
                Console.WriteLine($"Age: {student.Age}");
                Console.WriteLine($"Email: {student.Email}");
                Console.WriteLine($"Department: {student.Department}");
                Console.WriteLine($"Semester: {student.Semester}");
            }

            Pause();
        }

        static void UpdateStudent()
        {
            Console.Clear();

            Console.WriteLine("Update Student");

            Console.Write("Enter Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Student student = null;

            // Step 1: Pehle current data fetch karo
            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Students WHERE StudentId = @StudentId";

                using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@StudentId", id);

                    using (MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = new Student
                            {
                                StudentId = Convert.ToInt32(reader["StudentId"]),
                                Name = reader["Name"].ToString(),
                                Age = Convert.ToInt32(reader["Age"]),
                                Email = reader["Email"].ToString(),
                                Department = reader["Department"].ToString(),
                                Semester = Convert.ToInt32(reader["Semester"])
                            };
                        }
                    }
                }
            }

            // Step 2: Agar student nahi mila
            if (student == null)
            {
                Console.WriteLine("\nStudent not found.");
                Pause();
                return;
            }

            // Step 3: User se naya data poochein, khali chhodne pe purani value rahegi
            Console.WriteLine("\nLeave blank and press Enter to keep the current value.\n");

            Console.Write($"Enter New Name [{student.Name}]: ");
            string nameInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nameInput))
                student.Name = nameInput;

            Console.Write($"Enter New Age [{student.Age}]: ");
            string ageInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ageInput))
                student.Age = Convert.ToInt32(ageInput);

            Console.Write($"Enter New Email [{student.Email}]: ");
            string emailInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(emailInput))
                student.Email = emailInput;

            Console.Write($"Enter New Department [{student.Department}]: ");
            string departmentInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(departmentInput))
                student.Department = departmentInput;

            Console.Write($"Enter New Semester [{student.Semester}]: ");
            string semesterInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(semesterInput))
                student.Semester = Convert.ToInt32(semesterInput);

            // Step 4: Ab update query chalayein
            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string updateQuery = @"UPDATE Students
                                       SET Name = @Name,
                                           Age = @Age,
                                           Email = @Email,
                                           Department = @Department,
                                           Semester = @Semester
                                       WHERE StudentId = @StudentId";

                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@StudentId", student.StudentId);
                    updateCommand.Parameters.AddWithValue("@Name", student.Name);
                    updateCommand.Parameters.AddWithValue("@Age", student.Age);
                    updateCommand.Parameters.AddWithValue("@Email", student.Email);
                    updateCommand.Parameters.AddWithValue("@Department", student.Department);
                    updateCommand.Parameters.AddWithValue("@Semester", student.Semester);

                    int rows = updateCommand.ExecuteNonQuery();

                    if (rows > 0)
                        Console.WriteLine("\nStudent updated successfully!");
                    else
                        Console.WriteLine("\nStudent not found.");
                }
            }

            Pause();
        }

        static void DeleteStudent()
        {
            Console.Clear();

            Console.WriteLine("Delete Student");

            Console.Write("Enter Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM Students WHERE StudentId = @StudentId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", id);

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                        Console.WriteLine("\nStudent deleted successfully!");
                    else
                        Console.WriteLine("\nStudent not found.");
                }
            }

            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}