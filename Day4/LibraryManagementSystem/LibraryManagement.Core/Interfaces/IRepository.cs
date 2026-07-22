using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Interfaces;

public interface IRepository<T> where T : class
{
    void Add(T item);
    void Remove(int id);
    T? GetById(int id);
    IEnumerable<T> GetAll();
}