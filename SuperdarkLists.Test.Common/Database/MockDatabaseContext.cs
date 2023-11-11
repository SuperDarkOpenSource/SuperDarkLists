using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        mockItemCategories = CreateMockDbSet(BackingItemCategories);
        Categories = mockItemCategories.Object;
    }

    public List<ItemCategory> BackingItemCategories { get; } = new();
    public readonly Mock<DbSet<ItemCategory>> mockItemCategories;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public override DbSet<Item> Items { get; set; }
    public sealed override DbSet<ItemCategory> Categories { get; set; }
    
    public override DbSet<ShoppingList> ShoppingLists { get; set; }
    
    public override DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(0);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(0);
    }

    public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> backing) 
        where T : class
    {
        return backing.AsQueryable().BuildMockDbSet();
    }
}