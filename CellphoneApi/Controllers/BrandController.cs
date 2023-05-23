using CellphoneApi.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CellphoneApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly ILogger<BrandController> _logger;
        private readonly string _connectionString;  

        public BrandController(ILogger<BrandController> logger, IConfiguration config)
        {
            _logger = logger;
            _connectionString = config.GetConnectionString("CellphoneDB") ?? "";
        }

        [HttpGet("{brandId}")]
        public async Task<IActionResult> Get(int brandId)
        {
            //select using dapper
            using var connection = new SqlConnection(_connectionString);            
            connection.Open();
            var result = await connection.QueryAsync<Brand>("SELECT * FROM Brands WHERE BrandId = @BrandId", new { BrandId = brandId });
            if(result.Any())
            {
                var brand = result.FirstOrDefault();
                return Ok(brand);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //select all using dapper
            using var connection = new SqlConnection(_connectionString);            
            connection.Open();
            var result = await connection.QueryAsync<Brand>("SELECT * FROM Brands");
            if(result.Any())
            {
                var brands = result.ToList();
                return Ok(brands);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Brand brand)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Brands (Name) VALUES (@Name)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", brand.Name, DbType.String);      
            connection.Open();
            await connection.ExecuteAsync(query, parameters);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(Brand brand)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "UPDATE Brands SET Name = @Name WHERE BrandId = @BrandId";
            var parameters = new DynamicParameters();
            parameters.Add("Name", brand.Name, DbType.String);      
            parameters.Add("BrandId", brand.BrandId, DbType.Int32);      
            connection.Open();
            await connection.ExecuteAsync(query, parameters);
            return Ok();
        }

        [HttpDelete("{brandId}")]
        public async Task<IActionResult> Delete(int brandId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Brands WHERE BrandId = @BrandId";
            var parameters = new DynamicParameters();
            parameters.Add("BrandId", brandId, DbType.Int32);      
            connection.Open();
            await connection.ExecuteAsync(query, parameters);
            return Ok();
        }

    }
}