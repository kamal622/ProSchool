using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProSchool.Data.Models.Mapping
{
    public class FeeFrequencyMap : EntityTypeConfiguration<FeeFrequency>
    {
        public FeeFrequencyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("FeeFrequency");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Frequency).HasColumnName("Frequency");
        }
    }
}
