using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Dapper;

namespace Backend.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product16>> GetAll16Async();
        Task<int> Add16Async(string code);
        Task<int> Delete16Async(int id);

        Task<IEnumerable<Product36>> GetAll36Async();
        Task<int> Add36Async(string code);
        Task<int> Delete36Async(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _context;

        public ProductRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product16>> GetAll16Async()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product16>("SELECT * FROM Products16");
        }

        public async Task<int> Add16Async(string code)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("INSERT INTO Products16 (ProductCode) VALUES (@Code)", new { Code = code });
        }

        public async Task<int> Delete16Async(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM Products16 WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<Product36>> GetAll36Async()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product36>("SELECT * FROM Products36");
        }

        public async Task<int> Add36Async(string code)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("INSERT INTO Products36 (ProductCode) VALUES (@Code)", new { Code = code });
        }

        public async Task<int> Delete36Async(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("DELETE FROM Products36 WHERE Id = @Id", new { Id = id });
        }
    }
}
