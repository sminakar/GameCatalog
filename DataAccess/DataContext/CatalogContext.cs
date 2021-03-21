using DataAccessLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace DataAccess.DataContext
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("DataAccessLibrary.Models.Entities.Catalog", b =>
            {
                b.Property<Guid>("ID")
                    .HasColumnType("uniqueidentifier");

                b.Property<long>("ClusterID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<Guid?>("CompanyID")
                    .IsRequired()
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreateDate")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("datetime2");

                b.Property<int?>("GenreID")
                    .IsRequired()
                    .HasColumnType("int");

                b.Property<DateTime?>("ModifyDate")
                    .HasColumnType("datetime2");

                b.Property<decimal>("Price")
                    .HasColumnType("DECIMAL(18,2)");

                b.Property<int?>("Rate")
                    .HasColumnType("tinyint");

                b.Property<DateTime?>("ReleaseDate")
                    .HasColumnType("datetime2");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("ID");

                b.HasIndex("CompanyID");

                b.HasIndex("GenreID");

                b.ToTable("Catalogs");
            });

            modelBuilder.Entity("DataAccessLibrary.Models.Entities.Developer", b =>
            {
                b.Property<Guid>("ID")
                    .HasColumnType("uniqueidentifier");

                b.Property<long>("ClusterID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("CompanyName")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("nvarchar(30)");

                b.Property<string>("Country")
                    .HasMaxLength(25)
                    .HasColumnType("nvarchar(25)");

                b.Property<DateTime>("CreateDate")
                    .HasColumnType("datetime2");

                b.Property<int?>("EstablishYear")
                    .HasColumnType("int");

                b.Property<DateTime?>("ModifyDate")
                    .HasColumnType("datetime2");

                b.HasKey("ID");

                b.ToTable("Developers");
            });

            modelBuilder.Entity("DataAccessLibrary.Models.Entities.Genre", b =>
            {
                b.Property<int>("ID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnType("nvarchar(25)");

                b.HasKey("ID");

                b.ToTable("Genres");
            });

            modelBuilder.Entity("DataAccessLibrary.Models.Entities.Catalog", b =>
            {
                b.HasOne("DataAccessLibrary.Models.Entities.Developer", "Company")
                    .WithMany()
                    .HasForeignKey("CompanyID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("DataAccessLibrary.Models.Entities.Genre", "Genre")
                    .WithMany()
                    .HasForeignKey("GenreID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Company");

                b.Navigation("Genre");
            });
        }
    }
}
