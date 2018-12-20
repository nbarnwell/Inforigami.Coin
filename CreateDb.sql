
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

create table TimePeriod (
	Id int not null,
	Name nvarchar(256) not null,
	constraint PK_TimePeriod primary key (Id)
);

insert into TimePeriod (Id, Name) values (1, 'Day');
insert into TimePeriod (Id, Name) values (2, 'Week');
insert into TimePeriod (Id, Name) values (3, 'Fortnightly');
insert into TimePeriod (Id, Name) values (4, '4-weekly');
insert into TimePeriod (Id, Name) values (5, 'Monthly');
insert into TimePeriod (Id, Name) values (6, 'Quarterly');
insert into TimePeriod (Id, Name) values (7, 'Annually');

create table Currency (
	Id int not null,
	Code nvarchar(50) not null,
	Name nvarchar(256) not null
	constraint PK_Currency primary key (Id)
);

insert into Currency (Id, Code, Name) values (1, 'GBP', 'British Pound');
insert into Currency (Id, Code, Name) values (2, 'EUR', 'Euro');

create table Bank (
	Id int not null identity(1,1),
	Name nvarchar(256) not null
	constraint PK_Bank primary key (Id)
);

insert into Bank (Name) values ('Halifax');
declare @halifaxBankId int = scope_identity();
insert into Bank (Name) values ('Santander');
insert into Bank (Name) values ('Test');

create table Household (
	Id int not null identity(1,1),
	Name varchar(50) not null,
	constraint PK_Household primary key (Id)
);

create table UserAccount (
	Id int not null identity(1,1),
	Username nvarchar(50) not null,
	constraint PK_UserAccount primary key (Id)
);

create table Person (
	Id int not null identity(1,1),
	Name varchar(50) not null,
	UserAccountId int not null,
	HouseholdId int null,
	constraint PK_Person primary key (Id),
	constraint FK_Person__UserAccount foreign key (UserAccountId) references UserAccount (Id),
	constraint FK_Person__Household foreign key (HouseholdId) references Household (Id)
);

-- An abstract concept of an Account (not all accounts are bank accounts) to which statements/transactions are recorded
create table Account (
	Id int not null identity(1,1),
	Name nvarchar(256) not null,
	PersonId int not null,
	CurrencyId int not null,
	TimePeriodId int not null,
	constraint PK_Account primary key (Id),
	constraint FK_Account__Person foreign key (PersonId) references Person (Id),
	constraint FK_Account__Currency foreign key (CurrencyId) references Currency (Id),
	constraint FK_Account__TimePeriod foreign key (TimePeriodId) references TimePeriod (Id)
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

insert into AccountTransactionStatus values (1, 'Recorded');
insert into AccountTransactionStatus values (2, 'Reconciled');
insert into AccountTransactionStatus values (3, 'Investigating');
insert into AccountTransactionStatus values (4, 'Budgeted');


-- Categorise spending to see where the money goes
create table AccountTransactionCategory (
	Id int not null identity(1,1),
	Name nvarchar(256) not null,
	constraint PK_AccountTransactionCategory primary key (Id)
);

insert into AccountTransactionCategory (Name) values ('Misc');
insert into AccountTransactionCategory (Name) values ('Vehicle Fuel');
insert into AccountTransactionCategory (Name) values ('Motorbike Fuel');
insert into AccountTransactionCategory (Name) values ('Groceries');
insert into AccountTransactionCategory (Name) values ('Cash Withdrawal');
insert into AccountTransactionCategory (Name) values ('Mortgage');
insert into AccountTransactionCategory (Name) values ('Household Bill');

create table AccountTransactionType (
	Id int not null,
	Name nvarchar(256) not null,
	IsIncome bit not null,
	constraint PK_AccountTransactionTypeId primary key (Id)
);

insert into AccountTransactionType (Id, Name, IsIncome) values (1, 'Debit Card', 0);
insert into AccountTransactionType (Id, Name, IsIncome) values (2, 'Direct Debit', 1);
insert into AccountTransactionType (Id, Name, IsIncome) values (3, 'Cash Machine Withdrawal', 0);
insert into AccountTransactionType (Id, Name, IsIncome) values (4, 'Bank Credit', 1);
insert into AccountTransactionType (Id, Name, IsIncome) values (5, 'Transfer In', 1);
insert into AccountTransactionType (Id, Name, IsIncome) values (6, 'Transfer Out', 0);
insert into AccountTransactionType (Id, Name, IsIncome) values (7, 'Interest Charged', 0);
insert into AccountTransactionType (Id, Name, IsIncome) values (8, 'Interest Earned', 1);
insert into AccountTransactionType (Id, Name, IsIncome) values (9, 'Deposit', 1);

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

insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'BGC', 'Bank Giro Credit');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'BNS', 'Bonus');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'BP', 'Bill Payment');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'CHG', 'Charge');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'CHQ', 'Cheque');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'COM', 'Commission');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'COR', 'Correction');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'CPT', 'Cashpoint');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'CSH', 'Cash');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'CSQ', 'Cash/Cheque');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'DD', 'Direct Debit');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'DEB', 'Debit Card');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'DEP', 'Deposit');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'EFT', 'EFTPOS (electronic funds transfer at point of sale)');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'EUR', 'Euro Cheque');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'FE', 'Foreign Exchange');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'FEE', 'Fixed Service Charge');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'FPC', 'Faster Payment charge');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'FPI', 'Faster Payment incoming');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'FPO', 'Faster Payment outgoing');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'IB', 'Internet Banking');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'INT', 'Interest');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'MPI', 'Mobile Payment incoming');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'MPO', 'Mobile Payment outgoing');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'MTG', 'Mortgage');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'NS', 'National Savings Dividend');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'NSC', 'National Savings Certificates');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'OTH', 'Other');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'PAY', 'Payment');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'PSB', 'Premium Savings Bonds');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'PSV', 'Paysave');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'SAL', 'Salary');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'SPB', 'Cashpoint');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'SO', 'Standing Order');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'STK', 'Stocks/Shares');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'TD', 'Dep Term Dec');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'TDG', 'Term Deposit Gross Interest');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'TDI', 'Dep Term Inc');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'TDN', 'Term Deposit Net Interest');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'TFR', 'Transfer');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'UT', 'Unit Trust');
insert into BankSpecificTransactionType (BankId, Name, Description) values (@halifaxBankId, 'SUR', 'Excess Reject');

-- -- Budgets are more useful than categories because you can plan a budget and how much you plan to spend.
create table Budget (
	Id int not null identity(1,1),
	Name nvarchar(256) not null,
	HouseholdId int not null,
	constraint PK_Budget primary key (Id),
	constraint FK_Budget__Household foreign key (HouseholdId) references Household (Id)
);

create table BudgetItem (
	Id int not null identity(1,1),
	BudgetId int not null,
	Name nvarchar(256) not null,
	TimePeriodId int not null,
	AccountId int null,
	BankSpecificTransactionTypeId int null,
	TransactionDescriptionMatchPattern nvarchar(256) null,
	AmountLower money not null,
	AmountUpper money not null,
	constraint PK_BudgetItem primary key (Id),
	constraint FK_BudgetItem__Budget foreign key (BudgetId) references Budget (Id),
	constraint FK_BudgetItem__TimePeriod foreign key (TimePeriodId) references TimePeriod (Id),
	constraint FK_BudgetItem__BankSpecificTransactionType foreign key (BankSpecificTransactionTypeId) references BankSpecificTransactionType (Id),
	constraint FK_BudgetItem__Account foreign key (AccountId) references Account (Id)
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

create table BankAccountTransaction (
	Id int not null identity(1,1),
	AccountTransactionId int not null,
	BankId int not null,
	BankSpecificTransactionTypeId int not null,
	Description nvarchar(256) not null,
	constraint PK_BankAccountTransaction primary key (Id),
	constraint FK_BankAccountTransaction__Bank foreign key (Id) references Bank (Id),
	constraint FK_BankAccountTransaction__AccountTransaction foreign key (AccountTransactionId) references AccountTransaction (Id),
	constraint FK_BankAccountTransaction__BankSpecificTransactionType foreign key (BankSpecificTransactionTypeId) references BankSpecificTransactionType (Id)
);

create table AccountTransactionAccountTransactionCategory (
	AccountTransactionId int not null,
	AccountTransactionCategoryId int not null,
	Amount money not null,
	constraint PK_AccountTransactionAccountTransactionCategory primary key (AccountTransactionId, AccountTransactionCategoryId),
	constraint FK_AccountTransactionAccountTransactionCategory__AccountTransaction foreign key (AccountTransactionId) references AccountTransaction (Id),
	constraint FK_AccountTransactionAccountTransactionCategory__AccountTransactionCategory foreign key (AccountTransactionCategoryId) references AccountTransactionCategory (Id)
);

create table AccountTransactionBudgetItem (
	AccountTransactionId int not null,
	BudgetItemId int not null,
	Amount money not null,
	constraint PK_AccountTransactionBudgetItem primary key (AccountTransactionId, BudgetItemId),
	constraint FK_AccountTransactionBudgetItem__AccountTransaction foreign key (AccountTransactionId) references AccountTransaction (Id),
	constraint FK_AccountTransactionBudgetItem__BudgetItem foreign key (BudgetItemId) references BudgetItem (Id)
);

create table VehicleType (
	Id int not null,
	Name nvarchar(256) not null,
	constraint PK_VehicleType primary key (Id)
);

insert into VehicleType (Id, Name) values (0, 'Car');
insert into VehicleType (Id, Name) values (1, 'Motorbike');

create table Vehicle (
	Id int not null identity(1,1),
	VehicleTypeId int not null,
	Name nvarchar(256) not null,
	Make nvarchar(256) not null,
	Model nvarchar(256) not null,
	Registration nvarchar(256) not null,
	constraint PK_Vehicle primary key (Id),
	constraint FK_Vehicle__VehicleType foreign key (VehicleTypeId) references VehicleType (Id)
);

create table VehicleRefuelLog (
	Id int not null identity(1,1),
	VehicleId int not null,
	FuelLitres decimal not null,
	PencePerLitre int not null,
	Mileage int not null,
	constraint PK_VehicleRefuelLog primary key (Id),
	constraint FK_VehicleRefuelLog foreign key (VehicleId) references Vehicle (Id)
);

create table VehicleTravelPurposeType (
	Id int not null,
	Name nvarchar(256) not null,
	constraint PK_VehicleTravelPurposeType primary key (Id)
);

insert into VehicleTravelPurposeType (Id, Name) values (0, 'Personal');
insert into VehicleTravelPurposeType (Id, Name) values (1, 'Business');

create table VehicleMileageLog (
	Id int not null identity(1,1),
	VehicleId int not null,
	TripDateTime datetimeoffset(2) not null,
	StartMileage int not null,
	EndMileage int not null,
	VehicleTravelPurposeTypeId int not null,
	Purpose nvarchar(512) not null,
	[From] nvarchar(512) not null,
	[To] nvarchar(512) not null,
	constraint PK_VehicleMileageLog primary key (Id),
	constraint FK_VehicleMileageLog__Vehicle foreign key (VehicleId) references Vehicle (Id),
	constraint FK_VehicleMileageLog__VehicleTravelPurposeType foreign key (VehicleTravelPurposeTypeId) references VehicleTravelPurposeType (Id)
);

create table VehicleMaintenanceLogType (
	Id int not null,
	Name nvarchar(256) not null,
	constraint PK_VehicleMaintenanceLogType primary key (Id)
);

insert into VehicleMaintenanceLogType (Id, Name) values (0, 'Annual Service');
insert into VehicleMaintenanceLogType (Id, Name) values (1, 'Parts Replaced');

create table VehicleMaintenanceLog (
	Id int not null identity(1,1),
	VehicleId int not null,
	MaintenanceDateTime datetimeoffset(2) not null,
	Mileage int not null,
	constraint PK_VehicleMaintenanceLog primary key (Id),
	constraint FK_VehicleMaintenanceLog__Vehicle foreign key (VehicleId) references Vehicle (Id)
);

create table VehiclePart (
	Id int not null,
	Name nvarchar(256) not null,
	VehicleTypeId int not null,
	constraint PK_VehiclePart primary key (Id),
	constraint FK_VehiclePart__VehicleType foreign key (VehicleTypeId) references VehicleType (Id)
);

insert into VehiclePart (Id, VehicleTypeId, Name) values (1, 0, 'Front Left Tyre');
insert into VehiclePart (Id, VehicleTypeId, Name) values (2, 0, 'Front Right Tyre');
insert into VehiclePart (Id, VehicleTypeId, Name) values (3, 0, 'Rear Right Tyre');
insert into VehiclePart (Id, VehicleTypeId, Name) values (4, 0, 'Read Right Tyre');
insert into VehiclePart (Id, VehicleTypeId, Name) values (5, 0, 'Windscreen Wipers');
insert into VehiclePart (Id, VehicleTypeId, Name) values (6, 1, 'Front Tyre');
insert into VehiclePart (Id, VehicleTypeId, Name) values (7, 1, 'Rear Tyre');

create table VehiclePartsReplacementLog (
	Id int not null identity(1,1),
	VehicleMaintenanceLogId int not null,
	VehiclePartId int not null,
	constraint PK_VehiclePartsReplacementLog primary key (Id),
	constraint FK_VehiclePartsReplacementLog__VehicleMaintenanceLog foreign key (VehicleMaintenanceLogId) references VehicleMaintenanceLog (Id),
	constraint FK_VehiclePartsReplacementLog__VehiclePart foreign key (VehiclePartId) references VehiclePart (Id)
);