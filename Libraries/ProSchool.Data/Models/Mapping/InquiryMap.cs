using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProSchool.Data.Models.Mapping
{
    public class InquiryMap : EntityTypeConfiguration<Inquiry>
    {
        public InquiryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Inquiry");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InquiryDate).HasColumnName("InquiryDate");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.DivisionId).HasColumnName("DivisionId");
            this.Property(t => t.SubjectId).HasColumnName("SubjectId");

            // Relationships
            this.HasRequired(t => t.Division)
                .WithMany(t => t.Inquiries)
                .HasForeignKey(d => d.DivisionId);
            this.HasRequired(t => t.Subject)
                .WithMany(t => t.Inquiries)
                .HasForeignKey(d => d.SubjectId);

        }
    }
}
