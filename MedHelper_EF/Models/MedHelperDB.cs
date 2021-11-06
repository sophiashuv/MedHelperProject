using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            optionsBuilder.UseNpgsql("Username=postgres;Password=11662002;Host=localhost;Port=5432;Database=MedHelperDB");
        }
    }
}
