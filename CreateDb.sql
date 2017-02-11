
/*

Conventions:
DateTimeOffset means local time to the user
DateTime means UTC

*/


EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'Coin'
GO
USE [master]
GO
ALTER DATABASE [Coin] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
USE [master]
GO
/****** Object:  Database [Coin]    Script Date: 15/11/2015 23:18:33 ******/
DROP DATABASE [Coin]
GO

create database Coin;
go

use Coin;
GO

/*
drop index IX_DomainEvent__AggregateId_Version on AggregateRootEvent;
drop table AggregateRootEvent;
drop table AggregateRoot;
drop table AccountTransactionAccountTransactionCategory;
drop table AccountTransaction;
drop table BankSpecificTransactionType;
drop table AccountTransactionType;
drop table AccountTransactionCategoryMatchPattern;
drop table AccountTransactionCategoryMatchPatternMatchMethod;
drop table AccountTransactionCategory;
drop table AccountTransactionStatus;
drop table AccountStatement;
drop table Fund;
drop index IX_Account_AccountNumberAndSortCode on BankAccount;
drop table BankAccount;
drop table Account;
drop table [User];
drop table Household;
drop table Bank;
drop index IX_AuditLogByCorrelationId on AuditLog;
drop table AuditLog;
*/

create table AuditLog (
	Id int not null identity(1,1),
	MessageId uniqueidentifier not null,
	CorrelationId uniqueidentifier not null,
	CausationId uniqueidentifier not null,
	Timestamp datetimeoffset(2) not null,
	MessageTypeName nvarchar(512) not null,
	PayloadJson nvarchar(max) not null,
	constraint PK_Audit primary key (Id)
);

create index IX_AuditLogByCorrelationId on AuditLog (CorrelationId);

create table Currency (
	Id int not null,
	Code nvarchar(50) not null,
	Name nvarchar(256) not null
	constraint PK_Currency primary key (Id)
);

create table Bank (
	Id int not null identity(1,1),
	Name nvarchar(256) not null
	constraint PK_Bank primary key (Id)
);

insert into Bank (Name) values ('Halifax');

create table Person (
	Id int not null identity(1,1),
	Name varchar(50) not null,
	constraint PK_Person primary key (Id)
);

-- An abstract concept of an Account (not all accounts are bank accounts) to which statements/transactions are recorded
create table Account (
	Id int not null identity(1,1),
	Name nvarchar(256) not null,
	PersonId int not null,
	CurrencyId int not null,
	constraint PK_Account primary key (Id),
	constraint FK_Account__Person foreign key (PersonId) references Person (Id),
	constraint FK_Account__Currency foreign key (CurrencyId) references Currency (Id)
);

-- Additional details to describe a bank account
create table BankAccount (
	Id int not null identity(1,1),
	BankId int not null,
	AccountId int not null,
	CreditLimit money not null,
    AccountNumber nvarchar(50) not null,
	SortCode nvarchar(50) not null,
	constraint PK_BankAccount primary key (Id),
	constraint FK_BankAccount__Account foreign key (AccountId) references Account (Id),
	constraint FK_BankAccount__Bank foreign key (BankId) references Bank (Id)
);

create index IX_BankAccount_AccountNumberAndSortCode on BankAccount (AccountNumber, SortCode);

-- A fund within a single actual account, to allow the funds in the account to be divided up (like a "virtual" account of sorts)
create table Fund (
	Id int not null identity(1,1),
	AccountId int not null,
	Name nvarchar(256) not null,
	constraint PK_Fund primary key (Id),
	constraint FK_Fund__Account foreign key (AccountId) references Account (Id)
);

-- Statements are used to divide transactions up, and mainly to make calculating the running balance calculation scalable
create table AccountStatement (
	Id int not null identity(1,1),
	AccountId int not null,
	PeriodStart datetimeoffset(2) not null,
	PeriodEnd datetimeoffset(2) not null,
	StartingBalance money not null,
	constraint PK_AccountStatement primary key (Id),
	constraint FK_AccountStatement__Account foreign key (AccountId) references Account (Id)
);

-- Transactions can be simply recorded, or reconciled against a bank-supplied statement
create table AccountTransactionStatus (
	Id int not null,
	Name nvarchar(256) not null,
	constraint PK_AccountTransactionStatus primary key (Id)
);

-- Categorise spending to see where the money goes
create table AccountTransactionCategory (
	Id int not null identity(1,1),
	Name nvarchar(256) not null,
	constraint PK_AccountTransactionCategory primary key (Id)
);

create table AccountTransactionType (
	Id int not null,
	Name nvarchar(256) not null,
	IsIncome bit not null,
	constraint PK_AccountTransactionTypeId primary key (Id)
);

create table BankSpecificTransactionType (
	Id int not null identity(1,1),
	BankId int not null,
	Name nvarchar(256) not null,
	Description nvarchar(256) not null,
	AccountTransactionTypeId int null,
	constraint PK_BankSpecificTransactionType primary key (Id),
	constraint FK_BankSpecificTransactionType__Bank foreign key (BankId) references Bank (Id),
	constraint FK_BankSpecificTransactionType__AccountTransactionType foreign key (AccountTransactionTypeId) references AccountTransactionType (Id)
);

create table AccountTransaction (
	Id int not null identity(1,1),
	AccountStatementId int,
	TransactionTime datetimeoffset(2),
	RecordedDate datetime not null,
	AccountTransactionStatusId int not null,
	Amount money not null,
	Payee nvarchar(256),
	Description nvarchar(256),
	AccountTransactionTypeId int not null,
	constraint PK_AccountTransaction primary key (Id),
	constraint FK_AccountTransaction__AccountStatement foreign key (AccountStatementId) references AccountStatement (Id),
	constraint FK_AccountTransaction__AccountTransactionStatus foreign key (AccountTransactionStatusId) references AccountTransactionStatus (Id),
	constraint FK_AccountTransaction__AccountTransactionType foreign key (AccountTransactionTypeId) references AccountTransactionType (Id),
);

-- Create BankAccountTransaction to import from bank, and reconcile with AccountTransaction

create table AccountTransactionAccountTransactionCategory (
	AccountTransactionId int not null,
	AccountTransactionCategoryId int not null,
	Amount money not null,
	constraint PK_AccountTransactionAccountTransactionCategory primary key (AccountTransactionId, AccountTransactionCategoryId),
	constraint FK_AccountTransactionAccountTransactionCategory__AccountTransaction foreign key (AccountTransactionId) references AccountTransaction (Id),
	constraint FK_AccountTransactionAccountTransactionCategory__AccountTransactionCategory foreign key (AccountTransactionCategoryId) references AccountTransactionCategory (Id)
);

insert into Currency (Id, Code, Name) values (1, 'GBP', 'British Pound');
insert into Currency (Id, Code, Name) values (2, 'EUR', 'Euro');

declare @bankId int
set @bankId = (select top 1 Id from Bank);

insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'BGC', 'Bank Giro Credit');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'BNS', 'Bonus');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'BP', 'Bill Payment');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'CHG', 'Charge');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'CHQ', 'Cheque');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'COM', 'Commission');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'COR', 'Correction');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'CPT', 'Cashpoint');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'CSH', 'Cash');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'CSQ', 'Cash/Cheque');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'DD', 'Direct Debit');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'DEB', 'Debit Card');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'DEP', 'Deposit');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'EFT', 'EFTPOS (electronic funds transfer at point of sale)');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'EUR', 'Euro Cheque');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'FE', 'Foreign Exchange');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'FEE', 'Fixed Service Charge');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'FPC', 'Faster Payment charge');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'FPI', 'Faster Payment incoming');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'FPO', 'Faster Payment outgoing');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'IB', 'Internet Banking');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'INT', 'Interest');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'MPI', 'Mobile Payment incoming');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'MPO', 'Mobile Payment outgoing');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'MTG', 'Mortgage');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'NS', 'National Savings Dividend');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'NSC', 'National Savings Certificates');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'OTH', 'Other');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'PAY', 'Payment');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'PSB', 'Premium Savings Bonds');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'PSV', 'Paysave');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'SAL', 'Salary');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'SPB', 'Cashpoint');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'SO', 'Standing Order');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'STK', 'Stocks/Shares');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'TD', 'Dep Term Dec');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'TDG', 'Term Deposit Gross Interest');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'TDI', 'Dep Term Inc');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'TDN', 'Term Deposit Net Interest');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'TFR', 'Transfer');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'UT', 'Unit Trust');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@bankId, 'SUR', 'Excess Reject');

insert into AccountTransactionStatus values (1, 'Recorded');
insert into AccountTransactionStatus values (2, 'Reconciled');
insert into AccountTransactionStatus values (3, 'Investigating');

insert into AccountTransactionCategory (Name) values ('Misc');
insert into AccountTransactionCategory (Name) values ('Vehicle Fuel');
insert into AccountTransactionCategory (Name) values ('Motorbike Fuel');
insert into AccountTransactionCategory (Name) values ('Groceries');
insert into AccountTransactionCategory (Name) values ('Cash Withdrawal');
insert into AccountTransactionCategory (Name) values ('Mortgage');
insert into AccountTransactionCategory (Name) values ('Ténéré');

insert into AccountTransactionType (Id, Name, IsIncome) values (1, 'Debit Card', 0);
insert into AccountTransactionType (Id, Name, IsIncome) values (2, 'Direct Debit', 1);
insert into AccountTransactionType (Id, Name, IsIncome) values (3, 'Cash Machine Withdrawal', 0);
insert into AccountTransactionType (Id, Name, IsIncome) values (4, 'Bank Credit', 1);
insert into AccountTransactionType (Id, Name, IsIncome) values (5, 'Transfer In', 1);
insert into AccountTransactionType (Id, Name, IsIncome) values (6, 'Transfer Out', 0);
insert into AccountTransactionType (Id, Name, IsIncome) values (7, 'Interest Charged', 0);
insert into AccountTransactionType (Id, Name, IsIncome) values (8, 'Interest Earned', 1);
insert into AccountTransactionType (Id, Name, IsIncome) values (9, 'Deposit', 1);