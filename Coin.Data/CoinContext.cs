using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coin.Data
{
    public partial class CoinContext : DbContext
    {
        public CoinContext()
        {
        }

        public CoinContext(DbContextOptions<CoinContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountStatement> AccountStatement { get; set; }
        public virtual DbSet<AccountTransaction> AccountTransaction { get; set; }
        public virtual DbSet<AccountTransactionAccountTransactionCategory> AccountTransactionAccountTransactionCategory { get; set; }
        public virtual DbSet<AccountTransactionCategory> AccountTransactionCategory { get; set; }
        public virtual DbSet<AccountTransactionStatus> AccountTransactionStatus { get; set; }
        public virtual DbSet<AccountTransactionType> AccountTransactionType { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<Bank> Bank { get; set; }
        public virtual DbSet<BankAccount> BankAccount { get; set; }
        public virtual DbSet<BankSpecificTransactionType> BankSpecificTransactionType { get; set; }
        public virtual DbSet<Budget> Budget { get; set; }
        public virtual DbSet<BudgetItem> BudgetItem { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<Fund> Fund { get; set; }
        public virtual DbSet<Household> Household { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<TimePeriod> TimePeriod { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }
        public virtual DbSet<VehicleMaintenanceLog> VehicleMaintenanceLog { get; set; }
        public virtual DbSet<VehicleMaintenanceLogType> VehicleMaintenanceLogType { get; set; }
        public virtual DbSet<VehicleMileageLog> VehicleMileageLog { get; set; }
        public virtual DbSet<VehiclePart> VehiclePart { get; set; }
        public virtual DbSet<VehiclePartsReplacementLog> VehiclePartsReplacementLog { get; set; }
        public virtual DbSet<VehicleRefuelLog> VehicleRefuelLog { get; set; }
        public virtual DbSet<VehicleTravelPurposeType> VehicleTravelPurposeType { get; set; }
        public virtual DbSet<VehicleType> VehicleType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(local);Database=Coin;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account__Currency");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account__Person");

                entity.HasOne(d => d.TimePeriod)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.TimePeriodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account__TimePeriod");
            });

            modelBuilder.Entity<AccountStatement>(entity =>
            {
                entity.Property(e => e.PeriodEnd).HasColumnType("datetimeoffset(2)");

                entity.Property(e => e.PeriodStart).HasColumnType("datetimeoffset(2)");

                entity.Property(e => e.StartingBalance).HasColumnType("money");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountStatement)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountStatement__Account");
            });

            modelBuilder.Entity<AccountTransaction>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.Payee).HasMaxLength(256);

                entity.Property(e => e.RecordedDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionTime).HasColumnType("datetimeoffset(2)");

                entity.HasOne(d => d.AccountStatement)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.AccountStatementId)
                    .HasConstraintName("FK_AccountTransaction__AccountStatement");

                entity.HasOne(d => d.AccountTransactionStatus)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.AccountTransactionStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTransaction__AccountTransactionStatus");

                entity.HasOne(d => d.AccountTransactionType)
                    .WithMany(p => p.AccountTransaction)
                    .HasForeignKey(d => d.AccountTransactionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTransaction__AccountTransactionType");
            });

            modelBuilder.Entity<AccountTransactionAccountTransactionCategory>(entity =>
            {
                entity.HasKey(e => new { e.AccountTransactionId, e.AccountTransactionCategoryId });

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.HasOne(d => d.AccountTransactionCategory)
                    .WithMany(p => p.AccountTransactionAccountTransactionCategory)
                    .HasForeignKey(d => d.AccountTransactionCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTransactionAccountTransactionCategory__AccountTransactionCategory");

                entity.HasOne(d => d.AccountTransaction)
                    .WithMany(p => p.AccountTransactionAccountTransactionCategory)
                    .HasForeignKey(d => d.AccountTransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountTransactionAccountTransactionCategory__AccountTransaction");
            });

            modelBuilder.Entity<AccountTransactionCategory>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AccountTransactionStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AccountTransactionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasIndex(e => e.CorrelationId)
                    .HasName("IX_AuditLogByCorrelationId");

                entity.Property(e => e.MessageTypeName)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.PayloadJson).IsRequired();

                entity.Property(e => e.Timestamp).HasColumnType("datetimeoffset(2)");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasIndex(e => new { e.AccountNumber, e.SortCode })
                    .HasName("IX_BankAccount_AccountNumberAndSortCode");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreditLimit).HasColumnType("money");

                entity.Property(e => e.SortCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.BankAccount)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankAccount__Account");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.BankAccount)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankAccount__Bank");
            });

            modelBuilder.Entity<BankSpecificTransactionType>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.AccountTransactionType)
                    .WithMany(p => p.BankSpecificTransactionType)
                    .HasForeignKey(d => d.AccountTransactionTypeId)
                    .HasConstraintName("FK_BankSpecificTransactionType__AccountTransactionType");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.BankSpecificTransactionType)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BankSpecificTransactionType__Bank");
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<BudgetItem>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Budget)
                    .WithMany(p => p.BudgetItem)
                    .HasForeignKey(d => d.BudgetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BudgetItem__Budget");

                entity.HasOne(d => d.TimePeriod)
                    .WithMany(p => p.BudgetItem)
                    .HasForeignKey(d => d.TimePeriodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BudgetItem__TimePeriod");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Fund>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Fund)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fund__Account");
            });

            modelBuilder.Entity<Household>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Household)
                    .WithMany(p => p.Person)
                    .HasForeignKey(d => d.HouseholdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Person__Houshold");
            });

            modelBuilder.Entity<TimePeriod>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.Make)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Registration)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.Vehicle)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicle__VehicleType");
            });

            modelBuilder.Entity<VehicleMaintenanceLog>(entity =>
            {
                entity.Property(e => e.MaintenanceDateTime).HasColumnType("datetimeoffset(2)");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleMaintenanceLog)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleMaintenanceLog__Vehicle");
            });

            modelBuilder.Entity<VehicleMaintenanceLogType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VehicleMileageLog>(entity =>
            {
                entity.Property(e => e.From)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Purpose)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.To)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.TripDateTime).HasColumnType("datetimeoffset(2)");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleMileageLog)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleMileageLog__Vehicle");

                entity.HasOne(d => d.VehicleTravelPurposeType)
                    .WithMany(p => p.VehicleMileageLog)
                    .HasForeignKey(d => d.VehicleTravelPurposeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleMileageLog__VehicleTravelPurposeType");
            });

            modelBuilder.Entity<VehiclePart>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.VehiclePart)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehiclePart__VehicleType");
            });

            modelBuilder.Entity<VehiclePartsReplacementLog>(entity =>
            {
                entity.HasOne(d => d.VehicleMaintenanceLog)
                    .WithMany(p => p.VehiclePartsReplacementLog)
                    .HasForeignKey(d => d.VehicleMaintenanceLogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehiclePartsReplacementLog__VehicleMaintenanceLog");

                entity.HasOne(d => d.VehiclePart)
                    .WithMany(p => p.VehiclePartsReplacementLog)
                    .HasForeignKey(d => d.VehiclePartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehiclePartsReplacementLog__VehiclePart");
            });

            modelBuilder.Entity<VehicleRefuelLog>(entity =>
            {
                entity.Property(e => e.FuelLitres).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.VehicleRefuelLog)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VehicleRefuelLog");
            });

            modelBuilder.Entity<VehicleTravelPurposeType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });
        }
    }
}
