using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Task3.Models;

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
                cmd.Parameters.AddWithValue("@Designation", "HR");

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                return View();
            }
        }

        public ActionResult InsertRecordEmployee()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO EmployeeTask3 (FirstName, MiddleName, LastName, DOB, MobileNumber, Address , Salary , DesignationId) VALUES(@FirstName, @MiddleName, @LastName, @DOB, @MobileNumber, @Address , @Salary , @DesignationId)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@FirstName", "Lohn");
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
                return View();
            }
        }


        // 2. Write a query to count the number of records by designation name

        public ActionResult CountDesignation()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Designation, COUNT(E.Id) AS Count FROM DesignationTask3 D INNER JOIN EmployeeTask3 E on E.DesignationId = D.Id GROUP BY D.Designation;";
                SqlCommand cmd = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<CountDesignation> designations = new List<CountDesignation>();

                while(reader.Read())
                {
                    designations.Add(new CountDesignation
                    {
                        Designation = reader["Designation"].ToString(),
                        Count = Convert.ToInt32(reader["Count"])
                    });
                }

                connection.Close();
                return View(designations);

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
                List<JoinTable> joinTable = new List<JoinTable>();
                while (reader.Read())
                {
                    joinTable.Add(new JoinTable
                    {
                        FirstName = reader["firstName"].ToString(),
                        MiddleName = reader["middleName"].ToString(),
                        LastName = reader["lastName"].ToString(),
                        Designation = reader["designation"].ToString()
                    });

                }
                connection.Close();

                return View(joinTable);
            }
        }

        // 4. Create a database view that outputs Employee Id, First Name, Middle Name, Last Name, Designation, DOB, Mobile Number, Address & Salary
        public ActionResult EmployeeDetailsView()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM EmployeeDetailsView";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<EmployeeDetailsViewModel> employeeDetails = new List<EmployeeDetailsViewModel>();
                while (reader.Read())
                {
                    employeeDetails.Add(new EmployeeDetailsViewModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Designation = reader["Designation"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
                connection.Close();

                return View(employeeDetails);
            }
        }

        public ActionResult SpInsertDataDesignation()
        {
            return View();
        }

        // 5. Create a stored procedure to insert data into the Designation table with required parameters
        [HttpPost]
        public ActionResult SpInsertDataDesignation(Designations designations)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "EXEC InsertDesignation @Designation";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Designation", designations.Designation);

                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    return View("InsertRecordDesignation");
                }
                else
                {
                    return Content("Failed to insert record into DesignationTask3");
                }
            }
        }

        // 6. Create a stored procedure to insert data into the Employee table with required parameters
        public ActionResult SpInsertDataEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SpInsertDataEmployee(Employee employees)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("InsertEmployee", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", employees.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employees.MiddleName);
                cmd.Parameters.AddWithValue("@LastName", employees.LastName);
                cmd.Parameters.AddWithValue("@DOB", employees.DOB);
                cmd.Parameters.AddWithValue("@MobileNumber", employees.MobileNumber);
                cmd.Parameters.AddWithValue("@Address", employees.Address);
                cmd.Parameters.AddWithValue("@Salary", employees.Salary);
                cmd.Parameters.AddWithValue("@DesignationId", employees.DesignationId);

                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                connection.Close();

                return View("InsertRecordEmployee");

            }
        }


        // 7. Write a query that displays only those designation names that have more than 1 employee

        public ActionResult EmpInDesigMoreThanOne()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Designation, COUNT(E.Id) AS Count FROM DesignationTask3 D INNER JOIN EmployeeTask3 E on E.DesignationId = D.Id GROUP BY D.Designation HAVING COUNT(E.Id) > 1;";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<CountDesignation> designations = new List<CountDesignation>();
                while (reader.Read())
                {
                    designations.Add(new CountDesignation
                    {
                        Designation = reader["Designation"].ToString(),
                        Count = Convert.ToInt32(reader["Count"])
                    });
                }
                connection.Close();
                return View(designations);
            }
        }

        // 8. Create a stored procedure that returns a list of employees with columns Employee Id, First Name, Middle Name, Last Name, Designation, DOB, Mobile Number, Address & Salary (records should be ordered by DOB)
        public ActionResult SpGetEmployeeDetails()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllEmployees", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<EmployeeDetailsViewModel> employeeDetails = new List<EmployeeDetailsViewModel>();
                while (reader.Read())
                {
                    employeeDetails.Add(new EmployeeDetailsViewModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Designation = reader["Designation"].ToString(),
                        DOB = Convert.ToDateTime(reader["DOB"]),
                        MobileNumber = reader["MobileNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
                connection.Close();
                return View(employeeDetails);
            }
        }

        // 9. Create a stored procedure that return a list of employees by designation id (Input) with columns Employee Id, First Name, Middle Name, Last Name, DOB, Mobile Number, Address & Salary (records should be ordered by First Name)


        public ActionResult SpGetEmployeeByDesignationId()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SpGetEmployeeByDesignationId(DesignationId idFeild)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetEmployeesByDesignation", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DesignationId", Convert.ToInt32(idFeild.Identifier));
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                List<EmployeeDetailsViewModel> employeeDetails = new List<EmployeeDetailsViewModel>();
                while (reader.Read())
                {
                    employeeDetails.Add(new EmployeeDetailsViewModel
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
                return View("SpGetEmployeeByDesignationIdForView", employeeDetails);
            }
        }

        // 10. Write a query to find the employee having maximum salary

        public ActionResult MaxSalary()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select FirstName From EmployeeTask3 where Salary = ( SELECT Max(salary) FROM EmployeeTask3);";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                

                List<string> employeeName = new List<string>();
                while (reader.Read())
                {
                    employeeName.Add(reader["FirstName"].ToString());
                }
                connection.Close();
                ViewBag.EmployeeName = employeeName;
                return View();
            }
        }
    }
}