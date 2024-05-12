﻿using BeautySaloon.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using BeautySaloon.DAL.Entity;

namespace BeautySaloon.DAL;

public class BeautySaloonDbContext : DbContext
{
    public BeautySaloonDbContext(DbContextOptions<BeautySaloonDbContext> options) : base(options)
    {
        
    }
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<ServiceEntity> Services { get; set; }
    
    public DbSet<SessionEntity> Sessions { get; set; }
    
    public DbSet<UserTokenEntity> userTokens { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    public DbSet<T> DbSet<T>() where T : class
    {
        return Set<T>();
    }

    public new IQueryable<T> Query<T>() where T : class
    {
        return Set<T>();
    }
}