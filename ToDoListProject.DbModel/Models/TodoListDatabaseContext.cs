using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ToDoListProject.DbModel.Models
{
    public partial class TodoListDatabaseContext : DbContext
    {

        public bool IgnoreFilter { get; set; }
        public TodoListDatabaseContext()
        {
        }

        public TodoListDatabaseContext(DbContextOptions<TodoListDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ToDo> ToDos { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=YOUSEF\\SQLExpress;Database=TodoListDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.ToTable("ToDo");

                entity.Property(e => e.Id).UseIdentityColumn();

                entity.Property(e => e.Contents)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");


                entity.Property(e => e.AssignedBy)
                    .HasColumnType("int");

                entity.HasOne(d => d.User)
                       .WithMany(p => p.ToDos)
                       .HasForeignKey(d => d.UserId)
                       .OnDelete(DeleteBehavior.ClientSetNull)
                       .HasConstraintName("FK_AssignBy");

                entity.Property(e => e.IsRead).HasColumnType("bit");

                entity.Property(e => e.IsArchived).HasColumnType("bit");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ToDos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserToDo");
         
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).UseIdentityColumn();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                   .IsUnique();

                entity.HasIndex(e => e.Email, "Email_UNIQUE")
                      .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasDefaultValue("''")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsArcived).HasColumnType("bit");

                entity.Property(e => e.IsAdmin).HasColumnType("bit");

            });
            modelBuilder.Entity<User>().HasQueryFilter(a => !a.IsArcived || IgnoreFilter);
            modelBuilder.Entity<ToDo>().HasQueryFilter(a => !a.IsArchived || IgnoreFilter);
            modelBuilder.Entity<ToDo>().HasQueryFilter(a => !a.IsRead || IgnoreFilter);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
