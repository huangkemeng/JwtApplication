using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using JwtAudience;

namespace JwtAudience.Migrations
{
    [DbContext(typeof(AudienceDbContext))]
    [Migration("20170510012135_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
