using BeautySaloon.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using BeautySaloon.DAL.Entity;

namespace BeautySaloon.DAL;

public class BeautySaloonDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}