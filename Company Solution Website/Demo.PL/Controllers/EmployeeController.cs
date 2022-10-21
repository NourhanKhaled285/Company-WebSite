using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }

        //private readonly IEmployeeRepository employeeRepository;
        public EmployeeController(/*IEmployeeRepository employeeRepository*/IUnitOfWork unitOfWork, IMapper mapper)
        {
            //this.employeeRepository = employeeRepository;
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue = "")
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var Emps = await UnitOfWork.EmployeeRepository.GetAll();
                var MappedEmps = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Emps);
                return View(MappedEmps);
            }
            else
            {

                var Emps = await UnitOfWork.EmployeeRepository.Search(SearchValue);
                var MappedEmps = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Emps);
                return View(MappedEmps);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var employee = await UnitOfWork.EmployeeRepository.Get(id);
            if (employee == null)
                return NotFound();
            var mappedEmployee = Mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(mappedEmployee);

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await UnitOfWork.DepartmentRepository.GetAll();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                /// Manual Mapping
                /// var MappedEmp =  new Employee()
                ///{
                ///    Id = employee.Id,
                ///    Name = employee.Name,
                ///    Address = employee.Address,
                ///    Age = employee.Age,
                ///    DepartmentId = employee.DepartmentId,
                ///    Email = employee.Email,
                ///    HireDate = employee.HireDate,
                ///    IsActive = employee.IsActive,
                ///    PhoneNumber = employee.PhoneNumber,
                ///    Salary = employee.Salary
                ///};
                
                employee.ImageName = DocumentSettings.UploadFile(employee.Image, "Imgs");
                var MappedEmp = Mapper.Map<EmployeeViewModel, Employee>(employee);
                await UnitOfWork.EmployeeRepository.Add(MappedEmp);
                return RedirectToAction("Index");
            }
            ViewBag.Departments = await UnitOfWork.DepartmentRepository.GetAll();
            return View(employee);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var employee = await UnitOfWork.EmployeeRepository.Get(id);
            if (employee == null)
                return NotFound();
            var MappedEmp = Mapper.Map<Employee, EmployeeViewModel>(employee);
            ViewBag.Departments = await UnitOfWork.DepartmentRepository.GetAll();
            return View(MappedEmp);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel employee)
        {
            if (id != employee.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmp = Mapper.Map<EmployeeViewModel, Employee>(employee);

                    await UnitOfWork.EmployeeRepository.Update(MappedEmp);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(employee);
                }
            }
            ViewBag.Departments = await UnitOfWork.DepartmentRepository.GetAll();
            return View(employee);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var employee = await UnitOfWork.EmployeeRepository.Get(id);
            if (employee == null)
                return NotFound();
            var MappedEmp = Mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(MappedEmp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, EmployeeViewModel employee)
        {
            if (id != employee.Id)
                return NotFound();
            try
            {

                var MappedEmp = Mapper.Map<EmployeeViewModel, Employee>(employee);
                DocumentSettings.DeleteFile("Imgs", employee.ImageName);
                await UnitOfWork.EmployeeRepository.Delete(MappedEmp);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(employee);
            }
        }
    }
}
