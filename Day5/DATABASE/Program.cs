

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MySqlCrudApp;

class Program
{
    static async Task Main(string[] args)
    {
        //Build configuration from appsetting.json
        IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

        // read the connection string

        string connString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found in app settings.json");

        var builder = new SqlConnectionStringBuilder(connString)
        {
            InitialCatalog = "master"
        };
        string masterConnString = builder.ConnectionString;
        var repo = new EmployeeRepository(connString);
        bool running = true;
        var masterRepo = new EmployeeRepository(masterConnString);

        while (running)
        {
            Console.WriteLine("\n===== Employee CRUD Menu =====");
            Console.WriteLine("1. Create Database EmployDb");
            Console.WriteLine("2.Create Employee");
            Console.WriteLine("3. Add Employee");
            Console.WriteLine("4.GetAllEmployee");
            Console.WriteLine("5. GetEmployeewithId");
            Console.WriteLine("6. DeleteEmployeewithId");
            Console.WriteLine("7. UpdateEmployeewithId");
            Console.WriteLine("8. Exit");
            //Console.WriteLine("4. Update Employee");
            //Console.WriteLine("5. Delete Employee");
            //Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {

                case "1":
                    await CreateDatabase(masterRepo);
                    break;
                case "2":
                    await CreateEmployee(repo);
                    break;
                case "3":
                    await AddEmployee(repo);
                    break;
                case "4":
                    await GetAllEmployee(repo);
                    break;
                case "5":
                    await GetEmployeeWithId(repo);
                    break;
                case "6":
                    await DeleteEmployee(repo);
                    break;
                case "7":
                    await UpdateEmployee(repo);
                    break;
                case "8":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine(" Invalid option, try again.");
                    break;
            }
        }
        static async Task CreateDatabase(EmployeeRepository masterRepo)
        {
            try
            {
                await masterRepo.CreateDatabaseAsync();
                Console.WriteLine("Database created");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task CreateEmployee(EmployeeRepository repo)
        {
            await repo.CreateEmployeeAsync();
            Console.WriteLine("Create the employee table");
        }

        static async Task AddEmployee(EmployeeRepository repo)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Enter Department: ");
            string dept = Console.ReadLine() ?? "";

            Console.Write("Enter Salary: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
            {
                Console.WriteLine(" Invalid salary value.");
                return;
            }
            try
            {
                await repo.AddEmployeeAsync(new Employee { Name = name, Department = dept, Salary = salary });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($" Database error:{ex.Message}");
            }
        }

        static async Task GetAllEmployee(EmployeeRepository repo)
        {
            var employees = await repo.GetAllEmployeeAsync();

            if (employees.Count == 0)
            {
                Console.WriteLine("No Employees found");
                return;
            }

            foreach (var emp in employees)
            {
                Console.WriteLine($"[{emp.Id}] {emp.Name,-15} {emp.Department,-12} {emp.Salary:C}");
            }
        }
        static async Task GetEmployeeWithId(EmployeeRepository repo)
        {

            Console.Write("Enter Employee ID: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Invalid ID format. Please enter a valid number.");
                return;
            }

            List<Employee> employeeList = await repo.GetEmployeeWithIdAsync(id);
            // 2. Extract the first employee from the list (or null if empty)
            var emp = employeeList?.FirstOrDefault();

            // 3. Now 'emp' is a single Employee object, so this check works perfectly
            if (emp == null)
            {
                Console.WriteLine("No employee found with that ID.");
                return;
            }

            // 4. This will no longer throw CS1061 errors
            Console.WriteLine($"[{emp.Id}] {emp.Name,-15} {emp.Department,-12} {emp.Salary:C}");
        }
    }

    static async Task DeleteEmployee(EmployeeRepository repo)
    {
        Console.Write("Enter Employee ID to delete: ");
        string? input = Console.ReadLine();


        if(!int.TryParse(input,out int id))
        {
            Console.WriteLine("Invalid Id format");
            return;
        }
        int rowsAffected = await repo.DeleteEmployeeAsync(id);
        if (rowsAffected > 0)
        {
            Console.WriteLine("Employee deleted successfully!");
        }
        else
        {
            Console.WriteLine("No employee found with that ID to delete.");
        }
    }



    static async Task UpdateEmployee(EmployeeRepository repo)
    {
        Console.Write("Enter Employee ID to update: ");
        string? input = Console.ReadLine();

        if(!int.TryParse(input,out int id))
        {
            Console.WriteLine("Invalid ID format");
            return;
        }

        var list = await repo.GetEmployeeWithIdAsync(id);
        var emp = list?.FirstOrDefault();

        if (emp == null)
        {
            Console.WriteLine("employee not found");
            return;
        }

        //Display current info and ask for updates
    Console.WriteLine($"Current Name: {emp.Name}");
    Console.Write("Enter New Name (leave blank to keep current): ");
        string ? newName = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(newName)) emp.Name = newName;

        Console.WriteLine($"Current Department: {emp.Department}");
        Console.Write("Enter New Department (leave blank to keep current): ");
        string? newDept = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newDept)) emp.Department = newDept;
        Console.Write("Enter New Salary (leave blank to keep current): ");
        string? newSalaryStr = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newSalaryStr) && decimal.TryParse(newSalaryStr, out decimal newSalary))
        {
            emp.Salary = newSalary;
        }

        //Save changes to the database

        int rowsAffected = await repo.UpdateEmployeeWithId(emp);
        if (rowsAffected > 0)
        {
            Console.WriteLine("Employee updated successfully!");
        }
        else
        {
            Console.WriteLine("Failed to update employee.");
        }

    }


}