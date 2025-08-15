using Moq;
using Moq.AutoMock;
using ProposalService.Application.Commands;
using ProposalService.Application.Handlers;
using ProposalService.Domain.Entities;
using ProposalService.Domain.Enums;
using ProposalService.Domain.Interfaces;
namespace ProposalService.Tests.Handlers;

public class CreateProposalCommandHandlerTests

{

    private readonly AutoMocker _mocker;
    private readonly CreateProposalCommandHandler _handler;

    public CreateProposalCommandHandlerTests()
    {
        _mocker = new AutoMocker();
        _handler = _mocker.CreateInstance<CreateProposalCommandHandler>();
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateProposal()
    {
        // Arrange
        var command = new CreateProposalCommand("João Silva", "Seguro Auto", 1500.00m);
        var expectedProposal = Proposal.Create(command.CustomerName, command.InsuranceType, command.Value);
        _mocker.GetMock<IProposalRepository>()
            .Setup(x => x.CreateAsync(It.IsAny<Proposal>()))
            .ReturnsAsync(expectedProposal);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.CustomerName, result.CustomerName);
        Assert.Equal(command.InsuranceType, result.InsuranceType);
        Assert.Equal(command.Value, result.Value);
        Assert.Equal(ProposalStatus.UnderAnalysis, result.Status);
        Assert.Equal("Em Análise", result.StatusDescription);
        _mocker.GetMock<IProposalRepository>()
            .Verify(x => x.CreateAsync(It.IsAny<Proposal>()), Times.Once);

    }

}

