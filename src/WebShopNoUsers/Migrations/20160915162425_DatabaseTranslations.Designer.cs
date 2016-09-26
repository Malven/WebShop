using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebShopNoUsers.Models;

namespace WebShopNoUsers.Migrations
{
    [DbContext(typeof(WebShopRepository))]
    [Migration("20160915162425_DatabaseTranslations")]
    partial class DatabaseTranslations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebShopNoUsers.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductCategoryId");

                    b.HasKey("ProductId");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WebShopNoUsers.Models.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProductCategoryName");

                    b.HasKey("ProductCategoryId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("WebShopNoUsers.Models.ProductTranslation", b =>
                {
                    b.Property<int>("ProductId");

                    b.Property<string>("Language");

                    b.Property<string>("ProductDescription")
                        .IsRequired();

                    b.Property<string>("ProductName")
                        .IsRequired();

                    b.Property<float>("ProductPrice");

                    b.HasKey("ProductId", "Language");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductTranslations");
                });

            modelBuilder.Entity("WebShopNoUsers.Models.Product", b =>
                {
                    b.HasOne("WebShopNoUsers.Models.ProductCategory", "ProductCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebShopNoUsers.Models.ProductTranslation", b =>
                {
                    b.HasOne("WebShopNoUsers.Models.Product")
                        .WithMany("Translations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
