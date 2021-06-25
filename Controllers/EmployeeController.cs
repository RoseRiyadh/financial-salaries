using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ZulfieP.Models.Binding;
using ZulfieP.Models.Entities;

namespace ZulfieP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly SalariesContext _context;

        public EmployeeController(SalariesContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var salariesContext = _context.Employees.Include(e => e.Grade).Include(e => e.Stage).Where(s => s.IsDeleted != true);
            return View(await salariesContext.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.Grade)
                .Include(e => e.Stage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "GradeName");
            ViewData["StageId"] = new SelectList(_context.Stages, "Id", "StageName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeCreate employees)
        {
            if (ModelState.IsValid)
            {
                var emp = new Employees() { 
                    FullName = employees.FullName,
                    Birthdate = employees.Birthdate,
                    IdentityNumber = employees.IdentityNumber,
                    DepartmentId = employees.DepartmentId,
                    Section = employees.Section,
                    IsRatired = employees.IsRatired,
                    KidsNumber = employees.KidsNumber,
                    GradeId = employees.GradeId,
                    MarrigeStatus = employees.MarrigeStatus,
                };
                _context.Add(emp);

                var employee = _context.SaveChangesAsync();

                var sal = new Salaries()
                {
                    EmployeeId = employee.Id,
                    InitialSalary = employees.InitialSalary,
                    UniAllotments = employees.UniAllotments,
                    DegreeAllotments = employees.DegreeAllotments,
                    PositionAllotments = employees.PositionAllotments,
                    MarrigeAllotments = employees.MarrigeAllotments,
                    KidsAllotments = employees.KidsAllotments,
                    TransportationAllotments = employees.TransportationAllotments,
                    RetirementSubtraction = employees.RetirementSubtraction,
                    IncomeTax = 0,
                    OtherSubtractions = employees.OtherSubtractions,
                    Description = employees.Description,
                    ScientificTitleId = employees.ScientificTitleId,
                    VacationDiff = employees.VacationDiff,
                    TotalAmount = 0
                };

                _context.Add(sal);
                _context.SaveChangesAsync();
                return RedirectToAction("Index", "Salary");
            }
            return View(employees);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.FindAsync(id);

            if (employees == null)
            {
                return NotFound();
            }
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "GradeName", employees.GradeId);
            ViewData["StageId"] = new SelectList(_context.Stages, "Id", "StageName", employees.StageId);
            return View(employees);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeCreate employees)
        {
            if (id != employees.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var emp = new Employees()
                    {
                        Id = employees.Id,
                        FullName = employees.FullName,
                        Birthdate = employees.Birthdate,
                        IdentityNumber = employees.IdentityNumber,
                        DepartmentId = employees.DepartmentId,
                        Section = employees.Section,
                        IsRatired = employees.IsRatired,
                        KidsNumber = employees.KidsNumber,
                        GradeId = employees.GradeId,
                        MarrigeStatus = employees.MarrigeStatus,
                    };

                    _context.Update(employees);
                    await _context.SaveChangesAsync();

                    var salary = _context.Salaries.Find(emp.Salaries.Where(s => s.EmployeeId == emp.Id));
                    var sal = new Salaries()
                    {
                        Id = salary.Id,
                        EmployeeId = employees.Id,
                        InitialSalary = employees.InitialSalary,
                        UniAllotments = employees.UniAllotments,
                        DegreeAllotments = employees.DegreeAllotments,
                        PositionAllotments = employees.PositionAllotments,
                        MarrigeAllotments = employees.MarrigeAllotments,
                        KidsAllotments = employees.KidsAllotments,
                        TransportationAllotments = employees.TransportationAllotments,
                        RetirementSubtraction = employees.RetirementSubtraction,
                        IncomeTax = 0,
                        OtherSubtractions = employees.OtherSubtractions,
                        Description = employees.Description,
                        ScientificTitleId = employees.ScientificTitleId,
                        VacationDiff = employees.VacationDiff,
                        TotalAmount = 0
                    };

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employees.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Salary"); ;
            }
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "GradeName", employees.GradeId);
            ViewData["StageId"] = new SelectList(_context.Stages, "Id", "StageName", employees.StageId);
            return View(employees);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.Grade)
                .Include(e => e.Stage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employees = await _context.Employees.FindAsync(id);
            var salaries = await _context.Salaries.FindAsync(id);
            // _context.Employees.Remove(employees);
            employees.IsDeleted = true;
            salaries.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Salary");
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
