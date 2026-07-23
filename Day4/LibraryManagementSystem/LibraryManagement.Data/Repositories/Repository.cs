using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;
using LibraryManagement.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
namespace LibraryManagement.Data.Repositories;


// Generic class: one implementation reused for Book, Student, Loan, etc.
// Func<T,int> lets the caller tell the repository how to read the id off any T

public class Repository<T> : IRepository<T> where T : class
{
    private readonly List<T> _items = new();//collections
    private readonly Func<T, int> _idSelector;  //Delegate stored as a field
    private readonly JsonFileStore<T> _store;

    public Repository(Func<T, int> idSelector, JsonFileStore<T> store)
    {
        _idSelector = idSelector;
        _store = store;

    }


    public void Add(T item) => _items.Add(item);
    public void Remove(int id)
    {
        var target = GetById(id);
        if (target is not null) _items.Remove(target);

    }
    public T? GetById(int id) =>
        _items.FirstOrDefault(x => _idSelector(x) == id); //LINQ + lambda

    public IEnumerable<T> GetAll() => _items;

    public int GetNextId() => _items.Any() ? _items.Max(_idSelector) + 1 : 1;
    public void Update(T item)
    {
        var id = _idSelector(item);
        var index = _items.FindIndex(x => _idSelector(x) == id);
        if (index == -1)
        {
            throw new InvalidOperationException($"{typeof(T).Name}with id{id} not found");
        }
        _items[index] = item;

    }

    public Task SaveToFileAsync() => _store.SaveAsync(_items);


    public async Task LoadFromFileAsync()
    {
        var loaded = await _store.LoadAsync();
        _items.Clear();
        _items.AddRange(loaded);
    }


}