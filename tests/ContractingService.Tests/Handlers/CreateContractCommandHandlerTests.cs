using ContractingService.Application.Commands;
using ContractingService.Application.Handlers;
using ContractingService.Domain.Entities;
using ContractingService.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Moq.AutoMock;

namespace ContractingService.Tests.Handlers;

public class CreateContractCommandHandlerTests
{
    private readonly AutoMocker _mocker;
    private readonly CreateContractCommandHandler _handler;

    public CreateContractCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<CreateContractCommandHandler>();
    }

    [Fact]
    public async Task Handle_WithValidApprovedProposal_ShouldCreateContract()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var command = new CreateContractCommand(proposalId);
        var mockProposal = new ProposalDto(
            proposalId,
            "João Silva",
            "Seguro Auto",
            1500m,
            2, // Approved
            DateTime.UtcNow);

        var mockContract = Contract.Create(proposalId, "João Silva", "Seguro Auto", 1500m);

        _mocker.GetMock<IContractRepository>()
            .Setup(x => x.ExistsByProposalIdAsync(proposalId))
            .ReturnsAsync(false);

        _mocker.GetMock<IProposalServiceClient>()
            .Setup(x => x.GetProposalAsync(proposalId))
            .ReturnsAsync(mockProposal);

        _mocker.GetMock<IContractRepository>()
            .Setup(x => x.CreateAsync(It.IsAny<Contract>()))
            .ReturnsAsync(mockContract);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ProposalId.Should().Be(proposalId);
        result.CustomerName.Should().Be("João Silva");
        result.InsuranceType.Should().Be("Seguro Auto");
        result.Value.Should().Be(1500m);

        _mocker.GetMock<IContractRepository>()
            .Verify(x => x.CreateAsync(It.IsAny<Contract>()), Times.Once);
    }
}
