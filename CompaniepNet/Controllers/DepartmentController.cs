using CompaniepNet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CompaniepNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM dbo.Department";
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
        public JsonResult Post(Department department)
        {
            string query = @"INSERT INTO dbo.Department VALUES (@Name)";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("connString");
            SqlDataReader myReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand myCommand = new(query, sqlConnection))
                {
                    myCommand.Parameters.AddWithValue("@Name", department.Name);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    sqlConnection.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"UPDATE dbo.Department SET Name=@Name WHERE Id=@Id";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("connString");
            SqlDataReader myReader;

            using (SqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (SqlCommand myCommand = new(query, sqlConnection))
                {
                    myCommand.Parameters.AddWithValue("@Id", department.Id);
                    myCommand.Parameters.AddWithValue("@Name", department.Name);
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
            string query = @"DELETE FROM dbo.Department WHERE Id=@Id";
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
    }
}
