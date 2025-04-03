using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Task1.Models;


namespace Task1.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // 1. INSERT a record into the table
        public ActionResult Index()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employee";
                SqlCommand cmd = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<Employee> employees = new List<Employee>();
                while (reader.Read())
                {
                    employees.Add(new Employee()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                       
                    });
                }

                connection.Close();


                return View(employees);
            }
               
        }
        public ActionResult InsertFirst()
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "INSERT INTO Employee (FirstName, MiddleName, LastName, DOB, MobileNumber, Address) VALUES(@FirstName, @MiddleName, @LastName, @DOB, @MobileNumber, @Address)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FirstName", "John");
                cmd.Parameters.AddWithValue("@MiddleName", "D");
                cmd.Parameters.AddWithValue("@LastName", "Doe");
                cmd.Parameters.AddWithValue("@DOB", DateTime.ParseExact("25-12-1990", "dd-MM-yyyy", null));
                cmd.Parameters.AddWithValue("@MobileNumber", "1234567890");
                cmd.Parameters.AddWithValue("@Address", "Ahmedabad");

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

            }

            return View();
        }

        //// 2. INSERT multiple records for testing
        public ActionResult InsertMultiplePersons()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employee (FirstName, MiddleName, LastName, DOB, MobileNumber, Address) VALUES(@FirstName, @MiddleName, @LastName, @DOB, @MobileNumber, @Address)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FirstName", "Mohn");
                cmd.Parameters.AddWithValue("@MiddleName", "M");
                cmd.Parameters.AddWithValue("@LastName", "Doe");
                cmd.Parameters.AddWithValue("@DOB", DateTime.ParseExact("25-12-1991", "dd-MM-yyyy", null));
                cmd.Parameters.AddWithValue("@MobileNumber", "1234567800");
                cmd.Parameters.AddWithValue("@Address", "Bhavnagar");

                SqlCommand cmd2 = new SqlCommand(query, connection);
                cmd2.Parameters.AddWithValue("@FirstName", "Jane");
                cmd2.Parameters.AddWithValue("@MiddleName", "A");
                cmd2.Parameters.AddWithValue("@LastName", "Smith");
                cmd2.Parameters.AddWithValue("@DOB", DateTime.ParseExact("15-08-1995", "dd-MM-yyyy", null));
                cmd2.Parameters.AddWithValue("@MobileNumber", "0987654321");
                cmd2.Parameters.AddWithValue("@Address", "Surat");

                SqlCommand cmd3 = new SqlCommand(query, connection);
                cmd3.Parameters.AddWithValue("@FirstName", "Alice");
                cmd3.Parameters.AddWithValue("@MiddleName", "B");
                cmd3.Parameters.AddWithValue("@LastName", "Johnson");
                cmd3.Parameters.AddWithValue("@DOB", DateTime.ParseExact("10-10-1992", "dd-MM-yyyy", null));
                cmd3.Parameters.AddWithValue("@MobileNumber", "1122334455");
                cmd3.Parameters.AddWithValue("@Address", "Vadodara");


                connection.Open();
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                cmd3.ExecuteNonQuery();
                connection.Close();
            }
            return View();
        }

        // 3.  Write an Update query to change the First Name to “SQLPerson” for the first record
        public ActionResult UpdateFirstName()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "UPDATE Employee SET FirstName = @FirstName WHERE Id = @ID";
                SqlCommand cmd = new SqlCommand(Query, connection);
                cmd.Parameters.AddWithValue("@FirstName", "SQLPerson");
                cmd.Parameters.AddWithValue("@ID", 1);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return View();
        }

        // 4. Write an Update query to change the Middle Name to “I” for all records
        public ActionResult UpdateAllMiddleName()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Employee SET MiddleName = @MiddleName";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@MiddleName", "T");

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return View();
        }

        // 5. Write a delete query to delete record having Id column value less than 2
        public ActionResult DeleteRecord()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employee WHERE Id < @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", 2);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
                return View();
        }


        // 6.  Write a query to delete all the data from the table
        public ActionResult DeleteAllRecords()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employee";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return View();
        }
    }
}