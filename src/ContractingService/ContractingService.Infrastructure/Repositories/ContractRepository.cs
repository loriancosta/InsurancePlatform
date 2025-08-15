using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ContractingService.Domain.Entities;
using ContractingService.Domain.Interfaces;

namespace ContractingService.Infrastructure.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly string _connectionString;

    public ContractRepository(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
            throw new ArgumentNullException(nameof(configuration));

    public async Task<Contract> CreateAsync(Contract contract)
    {
        const string sql = @"
            INSERT INTO Contracts (Id, ProposalId, CustomerName, InsuranceType, Value, ContractDate)
            VALUES (@Id, @ProposalId, @CustomerName, @InsuranceType, @Value, @ContractDate)";

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new
        {
            contract.Id,
            contract.ProposalId,
            contract.CustomerName,
            contract.InsuranceType,
            contract.Value,
            contract.ContractDate
        });

        return contract;
    }

    public async Task<Contract?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, ProposalId, CustomerName, InsuranceType, Value, ContractDate
            FROM Contracts 
            WHERE Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

        return result is null ? null : new Contract(
            result.Id,
            result.ProposalId,
            result.CustomerName,
            result.InsuranceType,
            result.Value,
            result.ContractDate);
    }

    public async Task<Contract?> GetByProposalIdAsync(Guid proposalId)
    {
        const string sql = @"
            SELECT Id, ProposalId, CustomerName, InsuranceType, Value, ContractDate
            FROM Contracts 
            WHERE ProposalId = @ProposalId";

        using var connection = new SqlConnection(_connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { ProposalId = proposalId });

        return result is null ? null : new Contract(
            result.Id,
            result.ProposalId,
            result.CustomerName,
            result.InsuranceType,
            result.Value,
            result.ContractDate);
    }

    public async Task<IEnumerable<Contract>> GetAllAsync()
    {
        const string sql = @"
            SELECT Id, ProposalId, CustomerName, InsuranceType, Value, ContractDate
            FROM Contracts 
            ORDER BY ContractDate DESC";

        using var connection = new SqlConnection(_connectionString);
        var results = await connection.QueryAsync<dynamic>(sql);

        return results.Select(result => new Contract(
            result.Id,
            result.ProposalId,
            result.CustomerName,
            result.InsuranceType,
            result.Value,
            result.ContractDate));
    }

    public async Task<bool> ExistsByProposalIdAsync(Guid proposalId)
    {
        const string sql = "SELECT COUNT(1) FROM Contracts WHERE ProposalId = @ProposalId";

        using var connection = new SqlConnection(_connectionString);
        var count = await connection.QuerySingleAsync<int>(sql, new { ProposalId = proposalId });
        return count > 0;
    }
}
