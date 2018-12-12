namespace Metacube.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Metacube.Models;
    using sample.model;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class EmployeeController : Controller
    {
        private readonly EmployeeContext _context;
        private readonly IConfiguration _configuration;  
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IConfiguration configuration, EmployeeContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger; 

            // Adding default items.
            if (_context.Employees.Count() == 0)
            {
                _context.Employees.Add(new Employee { Name = "Chirag", FatherName = "Anil Goyal", Age=25 });
                _context.Employees.Add(new Employee { Name = "AbC", FatherName = "ABCD 2", Age=20 });
                _context.Employees.Add(new Employee { Name = "AbC 2", FatherName = "F ABC 2", Age=19 });
                _context.SaveChanges();
            }
        }    

        public IActionResult Index(string msg = ""){
            _logger.LogInformation($"Index called with {msg}");
            ViewBag.FlashMessage = msg;
            
            return this.View(_context.Employees);
        }

        public IActionResult GetDetails(long index){
            _logger.LogInformation($"get details called for index: {index}");
            var employee = _context.Employees.FirstOrDefault(emp => emp.Id == index);
            return this.View(employee);
        }

        [HttpPost]
        public IActionResult Update(Employee employee){
            _logger.LogInformation($"update called for index: {employee.Id}");
            bool itemUpdated = false;
            string msg = string.Empty;

            foreach(var emp in _context.Employees){
                if(emp.Id == employee.Id){
                    emp.Name = employee.Name;
                    emp.FatherName = employee.FatherName;
                    emp.Age = employee.Age;

                    itemUpdated = true;
                    break;
                }
            }

            if(itemUpdated){
                _context.SaveChanges();
                msg= "Employee Updated Successfully";
                _logger.LogInformation($"update successfull for index: {employee.Id}");
            }
            else{
                _logger.LogWarning($"update failed for index: {employee.Id}");
                msg = "Update Failed";
            }

            return RedirectToAction("Index","Employee", new { msg = msg });
        }

        [HttpGet]
        public IActionResult Create(){
            _logger.LogInformation($"create new employee");
             return this.View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee){
            _logger.LogInformation($"new employee created with id: {employee.Id}");
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return RedirectToAction("Index","Employee", new { msg = "Employee Added Successfully" });
        }
        
        [HttpPost]
        public IActionResult Delete(long Id){
            _logger.LogInformation($"delete called for index: {Id}");
            var msg = "Employee Not Found";
            var employee = _context.Employees.FirstOrDefault(emp => emp.Id == Id);
            if(employee != null){
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                msg = "Employee Deleted Successfully";
            }

            _context.SaveChanges();
            return RedirectToAction("Index","Employee", new { msg = msg });
        }

        public IActionResult Error(){
            return this.View();
        }

        public IActionResult ThrowError(){
            _logger.LogWarning($"Manual Error thrown");
            throw new Exception();
        }
    }
}