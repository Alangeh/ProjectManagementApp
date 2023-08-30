using ManagementApp.Data;
using ManagementApp.Models.Domain;
using ManagementApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            return RedirectToAction("Add");
        }
    }
}
