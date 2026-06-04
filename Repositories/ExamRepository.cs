using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Models;
using Dapper;

namespace Backend.Repositories
{
    public interface IExamRepository
    {
        Task<IEnumerable<Exam>> GetAllAsync();
        Task<int> AddAsync(Exam exam);
        Task<int> DeleteAsync(int id);
        Task<int> AddResultAsync(ExamResult result);
        Task<IEnumerable<ExamResult>> GetResultsAsync();
    }

    public class ExamRepository : IExamRepository
    {
        private readonly DbContext _context;

        public ExamRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exam>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Exam>("SELECT * FROM Exams ORDER BY Sequence");
        }

        public async Task<int> AddAsync(Exam exam)
        {
            using var connection = _context.CreateConnection();
            var maxSeq = await connection.QueryFirstOrDefaultAsync<int>("SELECT MAX(Sequence) FROM Exams");
            exam.Sequence = maxSeq + 1;
            var sql = "INSERT INTO Exams (QuestionText, OptionsJson, CorrectAnswer, Sequence) VALUES (@QuestionText, @OptionsJson, @CorrectAnswer, @Sequence); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(sql, exam);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var examToDelete = await connection.QueryFirstOrDefaultAsync<Exam>(
                    "SELECT * FROM Exams WHERE Id = @Id", new { Id = id }, transaction);

                if (examToDelete == null) return 0;

                await connection.ExecuteAsync("DELETE FROM Exams WHERE Id = @Id", new { Id = id }, transaction);

                // Re-order sequence
                await connection.ExecuteAsync(
                    "UPDATE Exams SET Sequence = Sequence - 1 WHERE Sequence > @Sequence",
                    new { Sequence = examToDelete.Sequence },
                    transaction);

                transaction.Commit();
                return 1;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> AddResultAsync(ExamResult result)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO ExamResults (ExamineeName, Score, TotalQuestions) VALUES (@ExamineeName, @Score, @TotalQuestions); SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.ExecuteScalarAsync<int>(sql, result);
        }

        public async Task<IEnumerable<ExamResult>> GetResultsAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ExamResult>("SELECT * FROM ExamResults ORDER BY ExamDate DESC");
        }
    }
}
