using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class EmployeeController : Controller
    {
        AppDbContext _db;
        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IList<EmployeeViewModel> model = new List<EmployeeViewModel>();
            var data = _db.Procedures.UspGetEmployees();
            if(data != null)
            {
                foreach(var item in data)
                {
                    EmployeeViewModel employee = new EmployeeViewModel();
                    employee.EmployeeId = item.EmployeeId;
                    employee.Name = item.Name;
                    employee.Address = item.Address;
                    employee.DepartmentId = item.DepartmentId;
                    employee.DepartmentName = item.DepartmentName;
                    model.Add(employee);
                }
            }
            return View(model);
        }
        public IActionResult Create() 
        {
            ViewBag.Departments = _db.Departments.ToList(); 
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeViewModel model)
        {
            ModelState.Remove("EmployeeID");
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    Name = model.Name,
                    Address = model.Address,
                    DepartmentId = model.DepartmentId
                };
                _db.Procedures.UspAddEmployee(employee);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Edit(int id) 
        {
            EmployeeViewModel model = new EmployeeViewModel();
            Employee data = _db.Procedures.UspGetEmployee(id);
            if(data != null)
            {
                model.EmployeeId = data.EmployeeId;
                model.Name = data.Name;
                model.Address = data.Address;
                model.DepartmentId = data.DepartmentId;
            }
            ViewBag.Departments = _db.Departments;
            return View("Create", model);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if(ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    EmployeeId = model.EmployeeId,
                    Name = model.Name,
                    Address = model.Address,
                    DepartmentId = model.DepartmentId,
                };
                _db.Employees.Update(employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = _db.Departments.ToList();
            return View("Create", model);
        }
        public ActionResult Delete(int id)
        {
            Employee model = _db.Employees.Find(id);
            if(model != null)
            {
                _db.Employees.Remove(model);
                _db.SaveChanges();                
            }
            return RedirectToAction("Index");
        }
    }
}
