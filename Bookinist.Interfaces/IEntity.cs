﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bookinist.Interfaces;

public interface IEntity
{
    int Id { get; set; }
}

public interface IRepository<T> where T : class, IEntity, new()
{
    IQueryable<T> Items { get; }
    T Get(int id);
    Task<T> GetAsync(int id, CancellationToken cancellationToken = default);
    T Add(T item);
    Task<T> AddAsync(T item, CancellationToken cancellationToken = default);
    void Update(T item);
    Task UpdateAsync(T item, CancellationToken cancellationToken = default);
    void Remove(int id);
    Task RemoveAsync(int id, CancellationToken cancellationToken = default);
}
