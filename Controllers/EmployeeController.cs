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
                    StageId = employees.StageId,
                    MarrigeStatus = employees.MarrigeStatus,
                };
                _context.Employees.Add(emp);

                var employee = _context.SaveChanges();

                var sal = new Salaries()
                {
                    EmployeeId = emp.Id,
                    Employee = emp,
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
                    ScientificTitles = _context.ScientificTitles.Find(employees.ScientificTitleId),
                    VacationDiff = (int)employees.VacationDiff,
                    TotalAmount = 0
                };
                _context.Salaries.Add(sal);
                var salary = _context.SaveChanges();
                sal.IncomeTax = this.calculateTaxIncome(sal);
                sal.MarrigeAllotments = this.calculateMarrigeAllotments(emp.MarrigeStatus);
                sal.KidsAllotments = this.calculateKidsAllotments(emp.KidsNumber);
                sal.TotalAmount = (int)Math.Round((decimal)(sal.InitialSalary + sal.UniAllotments
                    + sal.DegreeAllotments + sal.PositionAllotments
                    + sal.MarrigeAllotments + sal.KidsAllotments
                    + sal.TransportationAllotments + sal.ScientificTitles.Income
                    - sal.IncomeTax
                    - sal.RetirementSubtraction - sal.OtherSubtractions - sal.VacationDiff));
                _context.Salaries.Update(sal);
                salary = _context.SaveChanges();
                return RedirectToAction("Index", "Salary");
            }
            return View(employees);
        }

        private int? calculateKidsAllotments(short kidsNumber)
        {
            return kidsNumber > 0 ? kidsNumber * 10000 : 0;
        }

        private int calculateTaxIncome(Salaries salary)
        {
            // todo : kids income
            var oneMillion = 1000000;
            // Salary of the year
            var taxIncome = (int)salary.InitialSalary * 12;
            // taking Retirement rate 
            var retirement = taxIncome * 0.1;
            taxIncome = (int)(taxIncome - retirement);
            // Income of Marriage status
            taxIncome = (int)(taxIncome - this.calculateMarrigeAllotments(salary.Employee.MarrigeStatus));

            var kids = salary.Employee.KidsNumber;
            if (salary.Employee.KidsNumber > 0)
                taxIncome -= (kids * 200000);

            if (this.calculateAge(salary.Employee.Birthdate) > 63)
                taxIncome -= 300000;

            // taxIncome = (int)(Math.Round((decimal)(taxIncome + this.calculateMarrigeAllotments(salary))));
            if (taxIncome > oneMillion)
            {
                taxIncome = (int)(taxIncome - oneMillion);
            }
            // Calculate TaxIncome
            if (taxIncome <= 250000)
                taxIncome = (int)(taxIncome - (Math.Round(taxIncome * 0.03)));
            else if (taxIncome > 250000 || taxIncome <= 500000)
                taxIncome = (int)(taxIncome - (Math.Round(taxIncome * 0.05)));
            else if (taxIncome > 500000 || taxIncome <= oneMillion)
                taxIncome = (int)(taxIncome - (Math.Round(taxIncome * 0.1)));
            else if (taxIncome > oneMillion)
                taxIncome = (int)(taxIncome - (Math.Round(taxIncome * 0.15)));
            taxIncome += 70000;
            taxIncome = (int)Math.Round((decimal)(taxIncome / 12));


            // final equation
            return taxIncome;
        }
        private int? calculateMarrigeAllotments(short marrigeStatus)
        {
            // if employee is single 2500000 || wife employee and husband is retired
            // || husband employee and his wife is employee || wife employee and her husband is employee
            if (marrigeStatus == 0 || marrigeStatus == 1 || marrigeStatus == 2 || marrigeStatus == 3)
                return 2500000;
            // husband employee and his wife housewife 4500000 || wife employee and her husband works freelance 
            else if (marrigeStatus == 4 || marrigeStatus == 5)
                return 4500000;
            // divorsed female employee 3200000 || widow female 
            else if (marrigeStatus == 8 || marrigeStatus == 7)
                return 3200000;

            // error
            return 0;
        }
        private int calculateAge(DateTime birthdate)
        {
            return DateTime.Now.AddYears(-birthdate.Year).Year;

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
                        StageId = employees.StageId,
                        MarrigeStatus = employees.MarrigeStatus,
                    };

                    _context.Employees.Update(emp);
                    var emplo = _context.SaveChanges();

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
                        IncomeTax = salary.IncomeTax,
                        OtherSubtractions = employees.OtherSubtractions,
                        Description = employees.Description,
                        ScientificTitleId = salary.ScientificTitleId,
                        ScientificTitles = _context.ScientificTitles.Find(salary.ScientificTitleId),
                        VacationDiff = (int)employees.VacationDiff,
                        TotalAmount = 0
                    };
                    sal.IncomeTax = this.calculateTaxIncome(sal);
                    sal.MarrigeAllotments = this.calculateMarrigeAllotments(emp.MarrigeStatus);
                    sal.KidsAllotments = this.calculateKidsAllotments(emp.KidsNumber);
                    sal.TotalAmount = (int)Math.Round((decimal)(sal.InitialSalary + sal.UniAllotments
                        + sal.DegreeAllotments + sal.PositionAllotments
                        + sal.MarrigeAllotments + sal.KidsAllotments
                        + sal.TransportationAllotments + sal.ScientificTitles.Income
                        - sal.IncomeTax
                        - sal.RetirementSubtraction - sal.OtherSubtractions - sal.VacationDiff));
                    _context.Salaries.Update(sal);
                    var salar = _context.SaveChanges();
                    return RedirectToAction("Index", "Salary");
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
