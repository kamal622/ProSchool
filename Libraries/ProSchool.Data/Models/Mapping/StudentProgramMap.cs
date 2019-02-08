using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProSchool.Data.Models.Mapping
{
    public class StudentProgramMap : EntityTypeConfiguration<StudentProgram>
    {
        public StudentProgramMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("StudentPrograms");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StudentId).HasColumnName("StudentId");
            this.Property(t => t.DivisionId).HasColumnName("DivisionId");
            this.Property(t => t.SubjectId).HasColumnName("SubjectId");
            this.Property(t => t.BatchId).HasColumnName("BatchId");
            this.Property(t => t.FeeId).HasColumnName("FeeId");
            this.Property(t => t.RegistrationDate).HasColumnName("RegistrationDate");
            this.Property(t => t.NextDueDate).HasColumnName("NextDueDate");
            this.Property(t => t.IsActive).HasColumnName("IsActive");

            // Relationships
            this.HasRequired(t => t.Batch)
                .WithMany(t => t.StudentPrograms)
                .HasForeignKey(d => d.BatchId);
            this.HasRequired(t => t.Division)
                .WithMany(t => t.StudentPrograms)
                .HasForeignKey(d => d.DivisionId);
            this.HasRequired(t => t.Fee)
                .WithMany(t => t.StudentPrograms)
                .HasForeignKey(d => d.FeeId);
            this.HasRequired(t => t.Student)
                .WithMany(t => t.StudentPrograms)
                .HasForeignKey(d => d.StudentId);
            this.HasRequired(t => t.Subject)
                .WithMany(t => t.StudentPrograms)
                .HasForeignKey(d => d.SubjectId);

        }
    }
}
