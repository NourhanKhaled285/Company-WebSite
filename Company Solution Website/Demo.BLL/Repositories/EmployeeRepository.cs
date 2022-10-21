using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MVCAppDbContext context;

        public EmployeeRepository(MVCAppDbContext context) : base(context)
        {
            this.context = context;
        }

        public Task<IEnumerable<Employee>> GetEmployeeByDepartment(string Name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Employee>> Search(string Name)
            => await context.Employees.Where(E => E.Name.Contains(Name)).ToListAsync();

        //public int Add(Employee employee)
        //{
        //    context.Employees.Add(employee);
        //    return context.SaveChanges();
        //}

        //public int Delete(Employee employee)
        //{
        //    context.Employees.Remove(employee);
        //    return context.SaveChanges();
        //}

        //public Employee Get(int? id)
        //    => context.Employees.FirstOrDefault(D => D.Id == id);


        //public IEnumerable<Employee> GetAll()
        //    => context.Employees.ToList();

        //public int Update(Employee employee)
        //{
        //    context.Employees.Update(employee);
        //    return context.SaveChanges();
        //}
    }
}
