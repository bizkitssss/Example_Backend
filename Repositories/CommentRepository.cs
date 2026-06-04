using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Dapper;

namespace Backend.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<UserComment>> GetAllAsync();
        Task<int> AddAsync(string text);
    }

    public class CommentRepository : ICommentRepository
    {
        private readonly DbContext _context;

        public CommentRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserComment>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<UserComment>("SELECT * FROM Comments ORDER BY CreatedAt ASC");
        }

        public async Task<int> AddAsync(string text)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("INSERT INTO Comments (CommentText) VALUES (@Text)", new { Text = text });
        }
    }
}
