using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }
        public async Task<IActionResult> Index()
        {
            // 1. ViewData is a Dictionary Object (introduced in ASP.NET Framework 3.5)
            //      => It helps us to transfer the data from controller to view
            ViewData["Message"] = "Hello ViewData";
            // 2. ViewBag is a Dynamic Property (introduced in ASP.NET Framework 4.0 based on dynamic Feature)
            //      => It helps us to transfer the data from controller to view
            ViewBag.Message = "Hello ViewBag";

            TempData.Keep(); // Keep TempData in Session Storage
            return View(await departmentRepository.GetAll());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var Department = await departmentRepository.Get(id);
            if (Department == null)
                return NotFound();
            return View(Department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                await departmentRepository.Add(department);
                // 3. TempData is a Dictionary Object (introduced in ASP.NET Framework 3.5)
                //  =>  It is used to pass data between two consecutive requests.
                TempData["Message"] = "Department is created successfully";
                return RedirectToAction("Index");
            }
            return View(department);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var Department = await departmentRepository.Get(id);
            if (Department == null)
                return NotFound();
            return View(Department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    await departmentRepository.Update(department);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(department);
                }
            }
            return View(department);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var Department = await departmentRepository.Get(id);
            if (Department == null)
                return NotFound();
            return View(Department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, Department department)
        {
            if (id != department.Id)
                return NotFound();
            try
            {
                await departmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(department);
            }
        }
    }
}
