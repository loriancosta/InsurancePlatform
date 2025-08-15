using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Interfaces;

namespace ProposalService.Infrastructure.Repositories;

public class ProposalRepository : IProposalRepository
{
    private readonly string _connectionString;

    public ProposalRepository(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
            throw new ArgumentNullException(nameof(configuration));

    public async Task<Proposal> CreateAsync(Proposal proposal)
    {
        const string sql = @"
            INSERT INTO Proposals (Id, CustomerName, InsuranceType, Value, Status, CreatedDate, UpdatedDate)
            VALUES (@Id, @CustomerName, @InsuranceType, @Value, @Status, @CreatedDate, @UpdatedDate)";

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new
        {
            proposal.Id,
            proposal.CustomerName,
            proposal.InsuranceType,
            proposal.Value,
            Status = (int)proposal.Status,
            proposal.CreatedDate,
            proposal.UpdatedDate
        });

        return proposal;
    }

    public async Task<Proposal?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, CustomerName, InsuranceType, Value, Status, CreatedDate, UpdatedDate
            FROM Proposals 
            WHERE Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

        return result is null ? null : new Proposal(
            result.Id,
            result.CustomerName,
            result.InsuranceType,
            result.Value,
            (ProposalStatus)result.Status,
            result.CreatedDate,
            result.UpdatedDate);
    }

    public async Task<IEnumerable<Proposal>> GetAllAsync()
    {
        const string sql = @"
            SELECT Id, CustomerName, InsuranceType, Value, Status, CreatedDate, UpdatedDate
            FROM Proposals 
            ORDER BY CreatedDate DESC";

        using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync<dynamic>(sql);

        return results.Select(result => new Proposal(
            result.Id,
            result.CustomerName,
            result.InsuranceType,
            result.Value,
            (ProposalStatus)result.Status,
            result.CreatedDate,
            result.UpdatedDate));
    }

    public async Task<Proposal> UpdateAsync(Proposal proposal)
    {
        const string sql = @"
            UPDATE Proposals 
            SET CustomerName = @CustomerName, 
                InsuranceType = @InsuranceType, 
                Value = @Value, 
                Status = @Status, 
                UpdatedDate = @UpdatedDate
            WHERE Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new
        {
            proposal.Id,
            proposal.CustomerName,
            proposal.InsuranceType,
            proposal.Value,
            Status = (int)proposal.Status,
            proposal.UpdatedDate
        });

        return proposal;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        const string sql = "SELECT COUNT(1) FROM Proposals WHERE Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        var count = await connection.QuerySingleAsync<int>(sql, new { Id = id });
        return count > 0;
    }
}
