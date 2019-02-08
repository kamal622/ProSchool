using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ProSchool.Data.Models.Mapping
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RegistrationNo)
                .HasMaxLength(128);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.MiddleName)
                .HasMaxLength(128);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Gender)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Address)
                .IsRequired()
                .HasMaxLength(1028);

            this.Property(t => t.Mobile)
                .IsRequired()
                .HasMaxLength(16);

            this.Property(t => t.Email)
                .HasMaxLength(512);

            this.Property(t => t.School)
                .HasMaxLength(512);

            this.Property(t => t.College)
                .HasMaxLength(512);

            this.Property(t => t.SCAddress)
                .HasMaxLength(1028);

            this.Property(t => t.ContactName)
                .HasMaxLength(512);

            this.Property(t => t.ContactNumber)
                .HasMaxLength(32);

            this.Property(t => t.ContactRelation)
                .HasMaxLength(128);

            this.Property(t => t.OriginalFileName)
                .HasMaxLength(512);

            this.Property(t => t.SystemFileName)
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("Students");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InquiryId).HasColumnName("InquiryId");
            this.Property(t => t.RegistrationNo).HasColumnName("RegistrationNo");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.MiddleName).HasColumnName("MiddleName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.DOB).HasColumnName("DOB");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Remarks).HasColumnName("Remarks");
            this.Property(t => t.School).HasColumnName("School");
            this.Property(t => t.College).HasColumnName("College");
            this.Property(t => t.SCAddress).HasColumnName("SCAddress");
            this.Property(t => t.ContactName).HasColumnName("ContactName");
            this.Property(t => t.ContactNumber).HasColumnName("ContactNumber");
            this.Property(t => t.ContactRelation).HasColumnName("ContactRelation");
            this.Property(t => t.RelativePath).HasColumnName("RelativePath");
            this.Property(t => t.OriginalFileName).HasColumnName("OriginalFileName");
            this.Property(t => t.SystemFileName).HasColumnName("SystemFileName");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.IsRegistered).HasColumnName("IsRegistered");

            // Relationships
            this.HasRequired(t => t.Inquiry)
                .WithMany(t => t.Students)
                .HasForeignKey(d => d.InquiryId);

        }
    }
}
