using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Dapper;

namespace Backend.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<int> AddAsync(User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<int> RegisterAsync(string username, string passwordHash);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<User>("SELECT * FROM Users");
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(User user)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO Users (FullName, BirthDate, Age, Address) VALUES (@FullName, @BirthDate, @Age, @Address); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username", new { Username = username });
        }

        public async Task<int> RegisterAsync(string username, string passwordHash)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO Users (Username, PasswordHash, FullName, BirthDate, Age, Address) VALUES (@Username, @PasswordHash, '', '1900-01-01', 0, ''); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(sql, new { Username = username, PasswordHash = passwordHash });
        }
    }
}
