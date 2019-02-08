using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ProSchool.Data.Models.Mapping;

namespace ProSchool.Data.Models
{
    public abstract class ProSchoolContext : DbContext
    {
        static ProSchoolContext()
        {
            Database.SetInitializer<ProSchoolContext>(null);
        }

        public ProSchoolContext(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public DbSet<Batch> Batches { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<FeeFrequency> FeeFrequencies { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<StudentProgramInvoice> StudentProgramInvoices { get; set; }
        public DbSet<StudentProgram> StudentPrograms { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BatchMap());
            modelBuilder.Configurations.Add(new DivisionMap());
            modelBuilder.Configurations.Add(new FeeFrequencyMap());
            modelBuilder.Configurations.Add(new FeeMap());
            modelBuilder.Configurations.Add(new InquiryMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new StudentProgramInvoiceMap());
            modelBuilder.Configurations.Add(new StudentProgramMap());
            modelBuilder.Configurations.Add(new StudentMap());
            modelBuilder.Configurations.Add(new SubjectMap());
            modelBuilder.Configurations.Add(new UserClaimMap());
            modelBuilder.Configurations.Add(new UserLoginMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
