-- Create database if not exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'InsurancePlatform')
BEGIN
    CREATE DATABASE InsurancePlatform;
END
GO

USE InsurancePlatform;
GO

-- Create Proposals table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Proposals' AND xtype='U')
BEGIN
    CREATE TABLE Proposals (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        CustomerName NVARCHAR(255) NOT NULL,
        InsuranceType NVARCHAR(100) NOT NULL,
        Value DECIMAL(18,2) NOT NULL,
        Status INT NOT NULL,
        CreatedDate DATETIME2 NOT NULL,
        UpdatedDate DATETIME2 NULL
    );
END
GO

-- Create Contracts table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Contracts' AND xtype='U')
BEGIN
    CREATE TABLE Contracts (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        ProposalId UNIQUEIDENTIFIER NOT NULL,
        CustomerName NVARCHAR(255) NOT NULL,
        InsuranceType NVARCHAR(100) NOT NULL,
        Value DECIMAL(18,2) NOT NULL,
        ContractDate DATETIME2 NOT NULL
    );
END
GO

-- Create indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Proposals_Status')
BEGIN
    CREATE INDEX IX_Proposals_Status ON Proposals(Status);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Proposals_CreatedDate')
BEGIN
    CREATE INDEX IX_Proposals_CreatedDate ON Proposals(CreatedDate);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Contracts_ProposalId')
BEGIN
    CREATE INDEX IX_Contracts_ProposalId ON Contracts(ProposalId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Contracts_ContractDate')
BEGIN
    CREATE INDEX IX_Contracts_ContractDate ON Contracts(ContractDate);
END
GO
