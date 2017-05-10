using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using JwtAudience;

namespace JwtAudience.Migrations
{
    [DbContext(typeof(AudienceDbContext))]
    partial class AudienceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("JwtAudience.BlackRecord", b =>
                {
                    b.Property<string>("Jti")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UserId");

                    b.HasKey("Jti");

                    b.ToTable("BlackRecords");
                });
        }
    }
}
