using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Core.Interfaces;

public interface IRepository<T> where T : class
{
    void Add(T item);
    void Remove(int id);
    void Update(T item);
    T? GetById(int id);
    IEnumerable<T> GetAll();
    int GetNextId();
    //New - lets services generated ids without the caller guessing
}