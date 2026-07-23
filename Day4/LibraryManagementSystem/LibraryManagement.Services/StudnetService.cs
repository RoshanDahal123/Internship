using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagement.Core.Exceptions;
using LibraryManagement.Core.Models;
using LibraryManagement.Data.Repositories;


namespace LibraryManagement.Services;


public class StudentService
{
    private readonly Repository<Student> _students;

    public StudentService(Repository<Student> students) => _students = students;

    public Student AddStudent(string name, string email, string rollNumber)
    {
        if (_students.GetAll().Any(s => s.RollNumber == rollNumber))
        {
            throw new DuplicateEntityException($" ROll number {rollNumber} is already in use");

        }
        var student = new Student(_students.GetNextId(), name, email, rollNumber);
        _students.Add(student);
        return student;
    }

   public void UpdateStudent(int id, string name , string email)
    {
        var student = _students.GetById(id) ?? throw new MemberNotFoundException(id);
        student.UpdateContactInfo(name, email);
        _students.Update(student);

    }

    public void DeleteStudent(int id)
    {
        var student = _students.GetById(id) ?? throw new MemberNotFoundException(id);
        if (student.BorrowedBooksIds.Count > 0)
            throw new InvalidOperationException("Cannot delete a student with active loans.");
        _students.Remove(id);
    }

    public Student? GetStudent(int id) => _students.GetById(id);

    public IEnumerable<Student> GetAllStudents() => _students.GetAll();

    public IEnumerable<Student> Search(string keyword)=>
        _students.GetAll().Where(s=>
        s.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
        s.RollNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase));
}
