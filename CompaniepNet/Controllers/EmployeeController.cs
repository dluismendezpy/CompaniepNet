using CompaniepNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;

namespace CompaniepNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT 
                                Id,
                                Name,
                                Department,
                                convert(varchar(10), DateOfJoining, 120) as DateOfJoining,
                                PhotoFileName
                             FROM dbo.Employee";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("connString");
            SqlDataReader myReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand myCommand = new(query, sqlConnection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"INSERT INTO dbo.Employee (Name, Department, DateOfJoining, PhotoFileName)
                             VALUES (@Name, @Department, @DateOfJoining, @PhotoFileName)";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("connString");
            SqlDataReader myReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand myCommand = new(query, sqlConnection))
                {
                    myCommand.Parameters.AddWithValue("@Name", employee.Name);
                    myCommand.Parameters.AddWithValue("@Department", employee.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"UPDATE dbo.Employee 
                             SET Name=@Name,
                                 Department=@Department,
                                 DateOfJoining=@DateOfJoining,
                                 PhotoFileName=@PhotoFileName
                             WHERE Id=@Id";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("connString");
            SqlDataReader myReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand myCommand = new(query, sqlConnection))
                {
                    myCommand.Parameters.AddWithValue("@Id", employee.Id);
                    myCommand.Parameters.AddWithValue("@Name", employee.Name);
                    myCommand.Parameters.AddWithValue("@Department", employee.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpDelete("{Id}")]
        public JsonResult Delete(int Id)
        {
            string query = @"DELETE FROM dbo.Employee WHERE Id=@Id";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("connString");
            SqlDataReader myReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand myCommand = new(query, sqlConnection))
                {
                    myCommand.Parameters.AddWithValue("@Id", Id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _webHostEnvironment.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}
