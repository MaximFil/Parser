﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parser.DAL;

namespace Parser.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190607143721_AddUserSites")]
    partial class AddUserSites
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Parser.DAL.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<string>("PartContent");

                    b.Property<int>("SiteId");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Parser.DAL.Entities.Site", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Domain");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("Parser.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateSetting");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<byte>("ViewSetting");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Parser.DAL.Entities.UserArticle", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ArticleId");

                    b.Property<bool>("Deleted");

                    b.HasKey("UserId", "ArticleId");

                    b.HasIndex("ArticleId");

                    b.ToTable("UserArticle");
                });

            modelBuilder.Entity("Parser.DAL.Entities.UserSite", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("SiteId");

                    b.HasKey("UserId", "SiteId");

                    b.HasIndex("SiteId");

                    b.ToTable("UserSites");
                });

            modelBuilder.Entity("Parser.DAL.Entities.Article", b =>
                {
                    b.HasOne("Parser.DAL.Entities.Site", "Site")
                        .WithMany("Articles")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Parser.DAL.Entities.UserArticle", b =>
                {
                    b.HasOne("Parser.DAL.Entities.Article", "Article")
                        .WithMany("UserArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Parser.DAL.Entities.User", "User")
                        .WithMany("UserArticles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Parser.DAL.Entities.UserSite", b =>
                {
                    b.HasOne("Parser.DAL.Entities.Site", "Site")
                        .WithMany("UserSites")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Parser.DAL.Entities.User", "User")
                        .WithMany("UserSites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
