using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Dapper;

namespace Backend.Repositories
{
    public interface IProfileRepository
    {
        Task<int> AddAsync(UserProfile profile);
    }

    public class ProfileRepository : IProfileRepository
    {
        private readonly DbContext _context;

        public ProfileRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(UserProfile profile)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO Profiles (Email, Phone, BirthDay, Occupation, ProfileImageBase64) VALUES (@Email, @Phone, @BirthDay, @Occupation, @ProfileImageBase64); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(sql, profile);
        }
    }
}
