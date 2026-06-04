using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Dapper;

namespace Backend.Repositories
{
    public interface IQueueRepository
    {
        Task<string> GetNextQueueAsync();
        Task ResetQueueAsync();
    }

    public class QueueRepository : IQueueRepository
    {
        private readonly DbContext _context;

        public QueueRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<string> GetNextQueueAsync()
        {
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Lock the row for concurrency control (UPDLOCK)
                var state = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "SELECT LastLetter, LastNumber FROM QueueState WITH (UPDLOCK, ROWLOCK) WHERE Id = 1",
                    transaction: transaction);

                char lastLetter = ((string)state.LastLetter)[0];
                int lastNumber = state.LastNumber;

                int nextNumber = lastNumber + 1;
                char nextLetter = lastLetter;

                if (nextNumber > 9)
                {
                    nextNumber = 0;
                    nextLetter = (char)(lastLetter + 1);
                    if (nextLetter > 'Z')
                    {
                        nextLetter = 'A'; // Wrap around or handle overflow
                    }
                }
                
                if (lastNumber == -1) { // Special case for first run
                    nextLetter = 'A';
                    nextNumber = 0;
                }

                string queueNumber = $"{nextLetter}{nextNumber}";

                await connection.ExecuteAsync(
                    "UPDATE QueueState SET LastLetter = @Letter, LastNumber = @Number WHERE Id = 1",
                    new { Letter = nextLetter.ToString(), Number = nextNumber },
                    transaction: transaction);

                await connection.ExecuteAsync(
                    "INSERT INTO Queues (QueueNumber) VALUES (@QueueNumber)",
                    new { QueueNumber = queueNumber },
                    transaction: transaction);

                transaction.Commit();
                return queueNumber;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task ResetQueueAsync()
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("UPDATE QueueState SET LastLetter = 'A', LastNumber = -1 WHERE Id = 1");
        }
    }
}
