using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Dapper;

namespace Backend.Repositories
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAllAsync();
        Task<int> UpdateStatusAsync(int id, string status, string? reason);
        Task<int> UpdateStatusesAsync(IEnumerable<int> ids, string status, string? reason);
    }

    public class DocumentRepository : IDocumentRepository
    {
        private readonly DbContext _context;

        public DocumentRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Document>("SELECT * FROM Documents");
        }

        public async Task<int> UpdateStatusAsync(int id, string status, string? reason)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Documents SET Status = @Status, Reason = @Reason WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id, Status = status, Reason = reason });
        }

        public async Task<int> UpdateStatusesAsync(IEnumerable<int> ids, string status, string? reason)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE Documents SET Status = @Status, Reason = @Reason WHERE Id IN @Ids";
            return await connection.ExecuteAsync(sql, new { Ids = ids, Status = status, Reason = reason });
        }
    }
}
