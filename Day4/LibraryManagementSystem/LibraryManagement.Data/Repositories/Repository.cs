using LibraryManagement.Core.Interfaces;
using LibraryManagement.Core.Models;
using LibraryManagementSystem.LibraryManagement.Core.Models;
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
    private readonly string _filepath;

    public Repository(Func<T, int> idSelector, string filepath)
    {
        _idSelector = idSelector;
        _filepath = filepath;

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

    // Async file handling: reading/ writing shouldnm't bplck the console thresd.
    
    public async Task SaveToFileAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filepath, json);
        }
        catch(IOException ex)
        {
            Console.WriteLine($"Failed to save {typeof(T).Name} data:{ex.Message}");
        }
    }

    public async Task LoadFromFileAsync()
    {
        if (!File.Exists(_filepath)) return;

        try
        {
            await using var stream = File.OpenRead(_filepath);
            var loaded = await JsonSerializer.DeserializeAsync<List<T>>(stream);
            if(loaded is not null)
            {
                _items.Clear();
                _items.AddRange(loaded);
            }
        }
        catch(JsonException ex)
        {
            Console.WriteLine($"Corrupt data file for {typeof(T).Name}: {ex.Message} ");
        }
        finally
        {
            Console.WriteLine($"Load attempt finished for {typeof(T).Name}")
        }

    }
}