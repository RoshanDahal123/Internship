using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace MySqlCrudApp;

class Program
{

    static async Task Main(string[] args)
    {

        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string connString = config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found in appsettings.json");

        using var context = new AppDbContext(connString);
        var repo = new EmployeeRepository(context);

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== Employee CRUD Menu (EF Core) =====");
            Console.WriteLine("1. Create Database/Table");
            Console.WriteLine("2. Add Employee");
            Console.WriteLine("3. View All Employees");
            Console.WriteLine("4. View Employee by Id");
            Console.WriteLine("5. Update Employee");
            Console.WriteLine("6. Delete Employee");
            Console.WriteLine("7. Sorted Employees");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await repo.EnsureDatabaseCreatedAsync();
                    break;
                case "2":
                    await AddEmployee(repo);
                    break;
                case "3":
                    await ViewAllEmployees(repo);
                    break;
                case "4":
                    await ViewEmployeeById(repo);
                    break;
                case "5":
                    await UpdateEmployee(repo);
                    break;
                case "6":
                    await DeleteEmployee(repo);
                    break;
                case "7":
                    await ViewAllEmployeesSorted(repo);
                    break;
                case "8":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("❌ Invalid option, try again.");
                    break;
            }
        }

        // ---------- Local functions ----------

        async Task AddEmployee(EmployeeRepository repo)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Enter Department: ");
            string dept = Console.ReadLine() ?? "";

            Console.Write("Enter Salary: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
            {
                Console.WriteLine("❌ Invalid salary value.");
                return;
            }

            await repo.AddEmployeeAsync(new Employee { Name = name, Department = dept, Salary = salary });
        }

        async Task ViewAllEmployees(EmployeeRepository repo)
        {
            var employees = await repo.GetAllEmployeesAsync();

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                return;
            }

            foreach (var emp in employees)
                Console.WriteLine(emp);
        }
        async Task ViewAllEmployeesSorted(EmployeeRepository repo)
        {
            var employees = await repo.GetAllEmployeesSortedAsync();

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found");
                return;
            }
            foreach (var emp in employees)
                Console.WriteLine(emp);
        }

        async Task ViewEmployeeById(EmployeeRepository repo)
        {
            Console.Write("Enter Employee Id: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Invalid Id.");
                return;
            }

            var emp = await repo.GetEmployeeByIdAsync(id);
            Console.WriteLine(emp != null ? emp.ToString() : "❌ No employee found with that Id.");
        }

        async Task UpdateEmployee(EmployeeRepository repo)
        {
            Console.Write("Enter Employee Id to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Invalid Id.");
                return;
            }

            var existing = await repo.GetEmployeeByIdAsync(id);
            if (existing == null)
            {
                Console.WriteLine("❌ No employee found with that Id.");
                return;
            }

            Console.Write($"Enter new Name ({existing.Name}): ");
            string name = Console.ReadLine() is { Length: > 0 } n ? n : existing.Name;

            Console.Write($"Enter new Department ({existing.Department}): ");
            string dept = Console.ReadLine() is { Length: > 0 } d ? d : existing.Department ?? "";

            Console.Write($"Enter new Salary ({existing.Salary}): ");
            string? salaryInput = Console.ReadLine();
            decimal salary = decimal.TryParse(salaryInput, out decimal parsed) ? parsed : existing.Salary;

            bool success = await repo.UpdateEmployeeAsync(id, name, dept, salary);
            Console.WriteLine(success ? "✅ Employee updated successfully." : "❌ Update failed.");
        }

        async Task DeleteEmployee(EmployeeRepository repo)
        {
            Console.Write("Enter Employee Id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Invalid Id.");
                return;
            }

            bool success = await repo.DeleteEmployeeAsync(id);
            Console.WriteLine(success ? "✅ Employee deleted successfully." : "❌ No employee found with that Id.");
        }
    }
}