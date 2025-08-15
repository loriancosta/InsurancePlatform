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
            "João Sem Braço",
            "Auto Insurance",
            1000m,
            2,
            DateTime.UtcNow);

        var mockContract = Contract.Create(proposalId, "John Doe", "Auto Insurance", 1000m);

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
        result.CustomerName.Should().Be("John Doe");
        result.InsuranceType.Should().Be("Auto Insurance");
        result.Value.Should().Be(1000m);

        _mocker.GetMock<IContractRepository>()
            .Verify(x => x.CreateAsync(It.IsAny<Contract>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingContract_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var command = new CreateContractCommand(proposalId);

        _mocker.GetMock<IContractRepository>()
            .Setup(x => x.ExistsByProposalIdAsync(proposalId))
            .ReturnsAsync(true);

        // Act & Assert
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Contract for proposal {proposalId} already exists.");
    }

    [Fact]
    public async Task Handle_WithNonExistentProposal_ShouldThrowArgumentException()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var command = new CreateContractCommand(proposalId);

        _mocker.GetMock<IContractRepository>()
            .Setup(x => x.ExistsByProposalIdAsync(proposalId))
            .ReturnsAsync(false);

        _mocker.GetMock<IProposalServiceClient>()
            .Setup(x => x.GetProposalAsync(proposalId))
            .ReturnsAsync((ProposalDto?)null);

        // Act & Assert
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"Proposal with ID {proposalId} not found.");
    }

    [Theory]
    [InlineData(1)] // UnderAnalysis
    [InlineData(3)] // Rejected
    public async Task Handle_WithNonApprovedProposal_ShouldThrowInvalidOperationException(int status)
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var command = new CreateContractCommand(proposalId);
        
        var mockProposal = new ProposalDto(
            proposalId,
            "John Doe",
            "Auto Insurance",
            1000m,
            status,
            DateTime.UtcNow);

        _mocker.GetMock<IContractRepository>()
            .Setup(x => x.ExistsByProposalIdAsync(proposalId))
            .ReturnsAsync(false);

        _mocker.GetMock<IProposalServiceClient>()
            .Setup(x => x.GetProposalAsync(proposalId))
            .ReturnsAsync(mockProposal);

        // Act & Assert
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Proposal {proposalId} is not approved. Current status: {status}");
    }
}
