using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;
using SuperdarkLists.DomainModel.Database;
using SuperdarkLists.DomainModel.Database.Model;

namespace SuperdarkLists.Test.Common.Database;

public class MockDatabaseContext : DatabaseContext
{
    public MockDatabaseContext() : 
        base(new MockDatabaseConnectionStringProvider())
    {
        SaveChangesCalled = false;
        
        mockItemCategories = CreateMockDbSet(BackingItemCategories);
        Categories = mockItemCategories.Object;
    }

    public bool SaveChangesCalled { get; private set; }
    
    public List<ItemCategory> BackingItemCategories { get; } = new();
    public readonly Mock<DbSet<ItemCategory>> mockItemCategories;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    public override DbSet<Item> Items { get; set; }
    public sealed override DbSet<ItemCategory> Categories { get; set; }
    
    public override DbSet<ShoppingList> ShoppingLists { get; set; }
    
    public override DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SaveChangesCalled = true;
        return Task.FromResult(0);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SaveChangesCalled = true;
        return Task.FromResult(0);
    }

    public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity is ItemCategory itemCategory)
        {
            BackingItemCategories.Add(itemCategory);
        }

        return new ValueTask<EntityEntry<TEntity>>();
    }

    public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> backing) 
        where T : class
    {
        return backing.AsQueryable().BuildMockDbSet();
    }
}