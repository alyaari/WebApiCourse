using System.Xml.Linq;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public static class EmployeeService
    {
        static List<Employee> Employees { get; }
        static int nextId = 5;

        static EmployeeService()
        {
            Employees = new List<Employee>
        {
            new Employee{ Id = 1,
                Name="Mohammed Alyaari",
                Phone="7777232",
                Email="Alyaari@gmail.com"
                },
                 new Employee{ Id = 2,
                Name="Ahamed Alyaari",
                Phone="7777232",
                Email="Alyaari2@gmail.com"
                },
                  new Employee{ Id = 3,
                Name="AbdUllah Altwiti",
                Phone="777723234322",
                Email="AbdUllah@gmail.com"
                },

                   new Employee{ Id = 4,
                Name="Shadi AlQubati",
                Phone="734777232",
                Email="AlQubati@gmail.com"
                }
        };
        }

        public static List<Employee> GetAll()
        {
            return Employees;
        }

        public static Employee? Get(int id)
        {
            return Employees.FirstOrDefault(e => e.Id == id);
        }

        public static void Add(Employee employee)
        {
            employee.Id = nextId++;
            Employees.Add(employee);
        }

        public static void Delete(int id)
        {
            var employee = Get(id);
            if (employee is null)
                return;

            Employees.Remove(employee);
        }

        public static void Update(Employee employee)
        {
            var index = Employees.FindIndex(e => e.Id == employee.Id);
            if (index == -1)
                return;

            Employees[index] = employee;
        }
    }

}
