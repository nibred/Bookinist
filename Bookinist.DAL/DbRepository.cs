using Bookinist.DAL.Context;
using Bookinist.DAL.Entities;
using Bookinist.DAL.Entities.Base;
using Bookinist.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.DAL;

internal class DbRepository<T> : IRepository<T> where T : Entity, new()
{
    private readonly BookinistDB _db;
    private readonly DbSet<T> _dbSet;
    public DbRepository(BookinistDB db)
    {
        _db = db;
        _dbSet = db.Set<T>();
    }
    public virtual IQueryable<T> Items => _dbSet;

    public T Add(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Added;
        _db.SaveChanges();
        return item;
    }

    public async Task<T> AddAsync(T item, CancellationToken cancellationToken = default)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Added;
        await _db.SaveChangesAsync();
        return item;
    }

    public T Get(int id) => Items.SingleOrDefault(i => i.Id == id);

    public async Task<T> GetAsync(int id, CancellationToken cancellationToken = default) => await Items
        .SingleOrDefaultAsync(i => i.Id == id)
        .ConfigureAwait(false);

    public void Remove(int id)
    {
        var item = _dbSet.Local.FirstOrDefault(i => i.Id == id) ?? new T { Id = id };
        _db.Remove(item);
        _db.SaveChanges();
    }

    public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = _dbSet.Local.FirstOrDefault(i => i.Id == id) ?? new T { Id = id };
        _db.Remove(item);
        await _db.SaveChangesAsync();
    }

    public void Update(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Modified;
        _db.SaveChanges();
    }

    public async Task UpdateAsync(T item, CancellationToken cancellationToken = default)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }
}

class BookRepository : DbRepository<Book>
{
    public override IQueryable<Book> Items => base.Items.Include(i => i.Category);
    public BookRepository(BookinistDB db) : base(db)
    {
    }
}

class DealRepository : DbRepository<Deal>
{
    public override IQueryable<Deal> Items => base.Items
        .Include(i => i.Book)
        .Include(i => i.Buyer)
        .Include(i => i.Seller);
    public DealRepository(BookinistDB db) : base(db)
    {
    }
}