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
            return View(employee);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateEmployee updateEmployeeRequest)
        {
            var employee = await managementDbContext.Employees.SingleAsync(emp => emp.Id == updateEmployeeRequest.Id);

            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;
            employee.Department = updateEmployeeRequest.Department;
            employee.DateOfBirth = updateEmployeeRequest.DateOfBirth;
            employee.Salary = updateEmployeeRequest.Salary;

            await managementDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}