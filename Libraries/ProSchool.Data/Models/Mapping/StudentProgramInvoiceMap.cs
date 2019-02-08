using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProSchool.Data.Models.Mapping
{
    public class StudentProgramInvoiceMap : EntityTypeConfiguration<StudentProgramInvoice>
    {
        public StudentProgramInvoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.InvoiceNo)
                .HasMaxLength(128);

            this.Property(t => t.chequeNo)
                .HasMaxLength(50);

            this.Property(t => t.BankName)
                .HasMaxLength(512);

            this.Property(t => t.IFSCCode)
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("StudentProgramInvoices");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StudentProgramId).HasColumnName("StudentProgramId");
            this.Property(t => t.InvoiceNo).HasColumnName("InvoiceNo");
            this.Property(t => t.PaymentStatus).HasColumnName("PaymentStatus");
            this.Property(t => t.PaymentType).HasColumnName("PaymentType");
            this.Property(t => t.chequeNo).HasColumnName("chequeNo");
            this.Property(t => t.PaymentDate).HasColumnName("PaymentDate");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.IFSCCode).HasColumnName("IFSCCode");
            this.Property(t => t.IsChequeClear).HasColumnName("IsChequeClear");
            this.Property(t => t.InvoiceDate).HasColumnName("InvoiceDate");
            this.Property(t => t.InvoiceAmount).HasColumnName("InvoiceAmount");
            this.Property(t => t.PeriodStartDate).HasColumnName("PeriodStartDate");
            this.Property(t => t.PeriodEndDate).HasColumnName("PeriodEndDate");

            // Relationships
            this.HasRequired(t => t.StudentProgram)
                .WithMany(t => t.StudentProgramInvoices)
                .HasForeignKey(d => d.StudentProgramId);

        }
    }
}
