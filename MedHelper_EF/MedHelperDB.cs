using Microsoft.EntityFrameworkCore;

namespace MedHelper_EF.Models
{
    public class MedHelperDB: DbContext
    {
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<Contraindication> Contraindications { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Username=postgres;Password=0000;Host=localhost;Port=5432;Database=medhelper;");
            // optionsBuilder.UseNpgsql("Username=abtzgkeydswopw;Password=b408e7a2349d119021f6589c00f3d2a520bfa6cb6c77e32679ea1d9a9b6f208c;Host=ec2-54-155-87-214.eu-west-1.compute.amazonaws.com;Port=5432;Database=deqn4bsv53j18b;SSL Mode=Require;Trust Server Certificate=true");
        }
    }
}
