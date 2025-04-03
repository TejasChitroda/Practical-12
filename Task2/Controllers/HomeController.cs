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
                string query = "SELECT * FROM Employee";
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
                        Salary = Convert.ToDecimal(reader["Salary"])

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
                string query = "INSERT INTO EmployeeTask2 (FirstName,  LastName, DOB, MobileNumber, Address , Salary) VALUES(@FirstName, @LastName, @DOB, @MobileNumber, @Address , @Salary)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FirstName", "John");
                //cmd.Parameters.AddWithValue("@MiddleName", "D");
                cmd.Parameters.AddWithValue("@LastName", "Doe");
                cmd.Parameters.AddWithValue("@DOB", DateTime.ParseExact("25-12-1990", "dd-MM-yyyy", null));
                cmd.Parameters.AddWithValue("@MobileNumber", "1234567890");
                cmd.Parameters.AddWithValue("@Address", "Ahmedabad");
                cmd.Parameters.AddWithValue("@Salary", 50000);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            return Content("Record Inserted");
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
                return Content("Total Salary: " + totalSalary.ToString());
            }
           
        }

        // 3. Write an SQL query to find all employees having DOB less than 01-01-2000
        public ActionResult FindDOB()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
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
                return Content("Employees with DOB less than 01-01-2000: " + string.Join(", ", employees));
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
                return Content("Count of employees with NULL Middle Name: " + count.ToString());
            }
        }

    }
}