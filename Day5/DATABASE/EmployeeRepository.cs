
using System;
using System.Collections.Generic;

using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
namespace MySqlCrudApp;
public class EmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(string ConnectionString)=>_connectionString = ConnectionString;

    //create EmployeeDb database for the first time, if alredy created end it

    public async Task CreateDatabaseAsync()
    {
        const string query = @"
         IF DB_ID('Emp') IS NULL
            BEGIN
        CREATE DATABASE Emp;
        END";

        using var conn = new SqlConnection(_connectionString);

        using var cmd = new SqlCommand(query, conn);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        Console.WriteLine("Emp database is ready");
    }


    //Create Elmployee table
    public async Task CreateEmployeeAsync ()
    {
        const string query = @"
                  IF OBJECT_ID('Employees', 'U') IS NULL
                BEGIN
                    CREATE TABLE Employees (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(100) NOT NULL,
                        Department NVARCHAR(100),
                        Salary DECIMAL(10,2)
                    );
                END";
        try
        {

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        Console.WriteLine(" Employees table is ready;");
        }
        catch(Exception ex){
            Console.WriteLine($"Error {ex.Message}");
        }

    }
    /*Uses using → ensures connection is disposed
✔ Uses parameterized query → prevents SQL Injection ✅*/
    
    public async Task AddEmployeeAsync(Employee emp)
    {
        const string query = " INSERT INTO Employees(Name, Department, Salary) VALUES(@Name, @Department,@Salary)";
        try
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            //cmd.Parameters.AddWithValue("@Name", emp.Name);
            //cmd.Parameters.AddWithValue("@Department", emp.Department);
            //cmd.Parameters.AddWithValue("@Salary", emp.Salary);


            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = emp.Name;
            cmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = emp.Department;

            var salaryParam = cmd.Parameters.Add("@Salary", SqlDbType.Decimal);
            salaryParam.Precision = 10;
            salaryParam.Scale = 2;
            salaryParam.Value = emp.Salary;
            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();
            Console.WriteLine(rows > 0 ? " Employee added successfully." : " Insert failed.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception Occured:{ex.Message}");
        }
    }

    public async Task<List<Employee>> GetAllEmployeeAsync()
    {
        var employees = new List<Employee>();
        const string query = "SELECT * FROM Employees";

        try
        {

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

          while(await reader. ReadAsync())
            {
                employees.Add(new Employee
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? "" 
                    :reader.GetString(reader.GetOrdinal("Department")),
                    Salary = reader.GetDecimal(reader.GetOrdinal("Salary"))
                });
                
            }
            return employees;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occured: {ex.Message}");
            return employees;
        }

    }


    public async Task<List<Employee>> GetEmployeeWithIdAsync(int ID)
    {
        var employee = new List<Employee>();
        // 1. Use a parameterized placeholder (@ID) instead of ID=ID
        const string query = "SELECT * FROM Employees WHERE ID = @ID";
        try
        {

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ID", ID);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employee.Add(new Employee
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? ""
                    : reader.GetString(reader.GetOrdinal("Department")),
                    Salary = reader.GetDecimal(reader.GetOrdinal("Salary"))
                });

            }
            return employee;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return employee;
        }
    }


    public async Task<int> DeleteEmployeeAsync(int ID)
    {
        const string query = "DELETE FROM Employees WHERE ID =@ID";
        try
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query,conn);
            cmd.Parameters.AddWithValue("@ID", ID);
            await conn.OpenAsync();

            return await cmd.ExecuteNonQueryAsync();//Return number of rows deleted
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting employee: {ex.Message}");
            return 0;
        }
    }



    public async Task<int> UpdateEmployeeWithId(Employee emp)
    {
        const string query = "UPDATE Employees SET NAME=@Name,Department = @Department, Salary = @Salary WHERE ID = @ID";
        try
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ID", emp.Id);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Department", emp.Department);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);


            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();//returns number of rows updated
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error updating employee:{ex.Message}");
            return 0;
        }
    }
}






