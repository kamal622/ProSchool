using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProSchool.Data.Models.Mapping
{
    public class BatchMap : EntityTypeConfiguration<Batch>
    {
        public BatchMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("Batches");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SubjectId).HasColumnName("SubjectId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Capacity).HasColumnName("Capacity");
            this.Property(t => t.Description).HasColumnName("Description");

            // Relationships
            this.HasRequired(t => t.Subject)
                .WithMany(t => t.Batches)
                .HasForeignKey(d => d.SubjectId);

        }
    }
}
