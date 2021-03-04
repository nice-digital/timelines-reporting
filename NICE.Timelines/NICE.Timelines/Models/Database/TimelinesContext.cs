using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace NICE.Timelines.Models.Database
{
    public partial class TimelinesContext : DbContext
    {
        public TimelinesContext()
        {
        }

        public TimelinesContext(DbContextOptions<TimelinesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TimelineTask> TimelineTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\REPORTINSTANCE;Database=Timelines;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<TimelineTask>(entity =>
            {
                entity.ToTable("TimelineTask");

                entity.Property(e => e.Acid).HasColumnName("ACID");

                entity.Property(e => e.DateTypeDescription)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
