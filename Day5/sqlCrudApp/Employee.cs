
namespace MySqlCrudApp;
public class Employee
{


    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }


    public override string ToString()
    {
        return $"[{Id}] {Nmae,-15},{Department,-12},{Salary:C}";
    }
}