using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Task3.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // 1. INSERT a record into the two table EmployeeTask3 and DesignationTask3
        public ActionResult InsertRecordDesignation()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "INSERT INTO DesignationTask3 (Designation) VALUES(@Designation);";
                SqlCommand cmd = new SqlCommand(Query, connection);
                cmd.Parameters.AddWithValue("@Designation", "Manager");

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                return Content("Record Inserted into DesignationTask3");
            }
        }

        public ActionResult InsertRecordEmployee()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO EmployeeTask3 (FirstName, MiddleName, LastName, DOB, MobileNumber, Address , Salary , DesignationId) VALUES(@FirstName, @MiddleName, @LastName, @DOB, @MobileNumber, @Address , @Salary , @DesignationId)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FirstName", "John");
                cmd.Parameters.AddWithValue("@MiddleName", "D");
                cmd.Parameters.AddWithValue("@LastName", "Doe");
                cmd.Parameters.AddWithValue("@DOB", DateTime.ParseExact("25-12-1990", "dd-MM-yyyy", null));
                cmd.Parameters.AddWithValue("@MobileNumber", "1234567890");
                cmd.Parameters.AddWithValue("@Address", "Ahmedabad");
                cmd.Parameters.AddWithValue("@Salary", 50000);
                cmd.Parameters.AddWithValue("@DesignationId", 1);

               

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                return Content("Record Inserted into EmployeeTask3");
            }
        }


        // 2. Write a query to count the number of records by designation name

        public ActionResult CountDesignation()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Designation, COUNT(*) AS Count FROM DesignationTask3 GROUP BY Designation";
                SqlCommand cmd = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<string> designations = new List<string>();

                while (reader.Read())
                {
                    designations.Add(reader["Designation"].ToString() + " - " + reader["Count"].ToString());
                }

                connection.Close();

                return Content(string.Join("<br>", designations));

            }
        }


        // 3. Write a query to display First Name, Middle Name, Last Name & Designation name

        public ActionResult JoinTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT e.FirstName as firstName , e.MiddleName as middleName , e.LastName as lastName , d.Designation as designation FROM EmployeeTask3 e INNER JOIN DesignationTask3 d ON e.DesignationId = d.Id";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<string> joinTable = new List<string>();
                while (reader.Read())
                {
                    joinTable.Add(reader["firstName"] + " " + reader["middleName"] + " " + reader["lastName"] + " " + reader["designation"]);

                }
                connection.Close();

                return Content(string.Join("<br>" , joinTable));
            }
        }



    }
}