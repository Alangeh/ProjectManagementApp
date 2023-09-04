using ManagementApp.Data;
using ManagementApp.Models.Domain;
using ManagementApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ManagementDbContext managementDbContext;

        public EmployeesController(ManagementDbContext managementDbContext)
        {
            this.managementDbContext = managementDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
           var employees = await managementDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployee addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await managementDbContext.Employees.AddAsync(employee);
            await managementDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await managementDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployee()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };

                return await Task.Run(() => View("View",viewModel));
            }

           
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployee updateEmployeeRequest)
        {
            //var employee = await managementDbContext.Employees.FirstOrDefaultAsync(emp => emp.Id == updateEmployeeRequest.Id);
            var employee = await managementDbContext.Employees.FindAsync(updateEmployeeRequest.Id);

            if (employee != null)
            {
                employee.Name = updateEmployeeRequest.Name;
                employee.Email = updateEmployeeRequest.Email;
                employee.Department = updateEmployeeRequest.Department;
                employee.DateOfBirth = updateEmployeeRequest.DateOfBirth;
                employee.Salary = updateEmployeeRequest.Salary;

                await managementDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteEmployee(UpdateEmployee deleteEmployee)
        {
            var employee = await managementDbContext.Employees.FindAsync(deleteEmployee.Id);

            if (employee != null)
            {
                managementDbContext.Employees.Remove(employee);
                await managementDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}