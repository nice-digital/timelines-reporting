﻿using Microsoft.EntityFrameworkCore;

#nullable disable

namespace NICE.Timelines.DB.Models
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\REPORTINSTANCE;Database=Timelines;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<TimelineTask>(entity =>
            {
                entity.ToTable("TimelineTask");

                entity.Property(e => e.ACID).HasColumnName("ACID");
                
                entity.Property(e => e.StepDescription)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StageDescription)
	                .IsRequired()
	                .HasMaxLength(255);

                entity.Property(e => e.ClickUpSpaceId).HasMaxLength(255);
                entity.Property(e => e.ClickUpFolderId).HasMaxLength(255);
                entity.Property(e => e.ClickUpListId).HasMaxLength(255);
                entity.Property(e => e.ClickUpTaskId).HasMaxLength(255);

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
