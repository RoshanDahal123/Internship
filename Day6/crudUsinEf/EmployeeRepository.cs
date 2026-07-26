

using Microsoft.EntityFrameworkCore;

namespace MySqlCrudApp;

public class EmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    //create database + table
    public async Task EnsureDatabaseCreatedAsync()
    {
        try
        {
            bool created = await _context.Database.EnsureCreatedAsync();
            Console.WriteLine(created
               ? "✅ Database and Employees table created."
               : "ℹ️ Database already exists — nothing to do.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


    // create(insert)

    public async Task AddEmployeeAsync(Employee emp)
    {
        try
        {
            _context.Employees.Add(emp);//stage the insert (not sent yet)
            int rows = await _context.SaveChangesAsync();//this is what actually runs the sql

            Console.WriteLine(rows > 0 ? "✅ Employee added successfully." : "❌ Insert failed.");


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");

        }
    }


    //read all

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        try
        {
            return await _context.Employees.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return new List<Employee>();
        }
    }
    public async Task<List<Employee>> GetAllEmployeesSortedAsync()
    {
        try
        {
            return await _context.Employees.OrderByDescending(e => e.Salary).ToListAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return new List<Employee>();
        }
    }


    //read ny id

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        try
        {
            return await _context.Employees.FindAsync(id);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception Occured:{ex.Message}");
            return null;
        }
    }


    //update

    public async Task<bool> UpdateEmployeeAsync(int id, string name, string department, decimal salary)
    {
        try
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null)
            {
                return false;
            }
                
            existing.Name = name;
            existing.Department = department;
            existing.Salary = salary;

            int rows = await _context.SaveChangesAsync();
            return rows > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        try
        {
            var existing = await _context.Employees.FindAsync(id);
            if (existing == null)
            {
                return false;
            }
            _context.Employees.Remove(existing);//stage the delete

            int rows = await _context.SaveChangesAsync();//executes it 
            return rows > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return false;
        }
    }
}

