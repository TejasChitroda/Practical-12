using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Task2.Models;

namespace Task2.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;


        // GET: Home
        public ActionResult Index()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EmployeeTask2";
                SqlCommand cmd = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<EmployeeTask2> employees = new List<EmployeeTask2>();
                while (reader.Read())
                {
                    employees.Add(new EmployeeTask2()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"].ToString())

                    });
                }
                connection.Close();
                return View(employees);
            }

        }

        // 1. INSERT a record into the table
        public ActionResult InsertRecord()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO EmployeeTask2 (FirstName,MiddleName ,  LastName, DOB, MobileNumber, Address , Salary) VALUES(@FirstName,@MiddleName, @LastName, @DOB, @MobileNumber, @Address , @Salary)";

                connection.Open();


                List<(string FirstName, string MiddleName, string LastName, string DOB, string MobileNumber, string Address, int Salary)> employees = new List<(string, string, string, string, string, string, int)>
                    {
                        ("John", "D", "Doe", "25-12-1990", "1234567890", "Ahmedabad", 50000),
                        ("Jane", null, "Smith", "10-06-1995", "9876543210", "Surat", 60000),
                        ("Michael", "A", "Johnson", "15-08-1988", "7894561230", "Vadodara", 75000),
                        ("Emily", null, "Brown", "05-04-1992", "4561237890", "Rajkot", 55000),
                        ("David", "K", "Williams", "12-11-1985", "9638527410", "Bhavnagar", 80000)
                    };

                foreach (var emp in employees)
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", emp.FirstName);
                        cmd.Parameters.AddWithValue("@MiddleName", emp.MiddleName ?? (object)DBNull.Value); // Handle null values
                        cmd.Parameters.AddWithValue("@LastName", emp.LastName);
                        cmd.Parameters.AddWithValue("@DOB", DateTime.ParseExact(emp.DOB, "dd-MM-yyyy", null));
                        cmd.Parameters.AddWithValue("@MobileNumber", emp.MobileNumber);
                        cmd.Parameters.AddWithValue("@Address", emp.Address);
                        cmd.Parameters.AddWithValue("@Salary", emp.Salary);

                        cmd.ExecuteNonQuery();
                    }
                }
                connection.Close();

            }
            return View();
        }

        // 2.  Write an SQL query to find the total amount of salaries

        public ActionResult TotalSalary()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT SUM(Salary) FROM EmployeeTask2";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                var totalSalary = cmd.ExecuteScalar();
                connection.Close();

                ViewBag.TotalSalary = totalSalary;
                return View();
            }

        }

        // 3. Write an SQL query to find all employees having DOB less than 01-01-2000
        public ActionResult FindDOB()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EmployeeTask2 WHERE DOB < @DOB";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@DOB", DateTime.ParseExact("01-01-2000", "dd-MM-yyyy", null));
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<string> employees = new List<string>();
                while (reader.Read())
                {
                    employees.Add(reader["FirstName"].ToString() + " " + reader["LastName"].ToString());
                }
                connection.Close();
                //return Content("Employees with DOB less than 01-01-2000: " + string.Join(", ", employees));
                ViewBag.Employees = employees;
                return View();
            }
        }

        // 4.   Write an SQL query to count employees having Middle Name NULL

        public ActionResult CountMiddleName()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM EmployeeTask2 WHERE MiddleName IS NULL";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                var count = cmd.ExecuteScalar();
                connection.Close();
                ViewBag.CountMiddleName = count;
                return View();
                //return Content("Count of employees with NULL Middle Name: " + count.ToString());
            }
        }

    }
}