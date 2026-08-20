using MySqlConnector;
using StudentRecordApp.Data;

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

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Department: ");
            string department = Console.ReadLine();

            Console.Write("Enter Semester: ");
            string semester = Console.ReadLine();

            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query = @"INSERT INTO Students
                                 (Name, Age, Email, Department, Semester)
                                 VALUES
                                 (@Name, @Age, @Email, @Department, @Semester)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@Semester", semester);

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
                            Console.WriteLine("--------------------------------");
                            Console.WriteLine($"ID: {reader["StudentId"]}");
                            Console.WriteLine($"Name: {reader["Name"]}");
                            Console.WriteLine($"Age: {reader["Age"]}");
                            Console.WriteLine($"Email: {reader["Email"]}");
                            Console.WriteLine($"Department: {reader["Department"]}");
                            Console.WriteLine($"Semester: {reader["Semester"]}");
                        }
                    }
                }
            }

            Pause();
        }

        static void UpdateStudent()
        {
            Console.Clear();

            Console.WriteLine("Update Student");

            Console.Write("Enter Student ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter New Age: ");
            int age = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter New Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter New Department: ");
            string department = Console.ReadLine();

            Console.Write("Enter New Semester: ");
            string semester = Console.ReadLine();

            using (MySqlConnection connection = database.GetConnection())
            {
                connection.Open();

                string query = @"UPDATE Students
                                 SET Name = @Name,
                                     Age = @Age,
                                     Email = @Email,
                                     Department = @Department,
                                     Semester = @Semester
                                 WHERE StudentId = @StudentId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Department", department);
                    command.Parameters.AddWithValue("@Semester", semester);

                    int rows = command.ExecuteNonQuery();

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