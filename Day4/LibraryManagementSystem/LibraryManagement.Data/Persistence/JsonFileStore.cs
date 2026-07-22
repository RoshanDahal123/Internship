using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LibraryManagement.Data.Persistence;


public class JsonFileStore<T> where T : class
{
    private readonly string _filePath;

    public JsonFileStore(string filePath)
    {
        _filePath = filePath;
    }

    public async Task SaveAsync(IEnumerable<T> items)
    {
        try
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);

        }
        catch (IOException ex)
        {
            Console.WriteLine($"Failed to save {typeof(T).Name} data: {ex.Message}");
        }
    }


    public async Task<List<T>> LoadAsync()
    {
        if (!File.Exists(_filePath))
            return new List<T>();

        try
        {
            await using var stream = File.OpenRead(_filePath);

            var loaded = await JsonSerializer.DeserializeAsync<List<T>>(stream);
            return loaded ?? new List<T>();
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Corrupt data file for {typeof(T).Name}: {ex.Message}");
            return new List<T>();
        }
        finally
        {
            Console.WriteLine($" Load attempt finished for {typeof(T).Name}.");

        }
    }
}