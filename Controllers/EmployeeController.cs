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
            ViewData["ScientificTitleId"] = new SelectList(_context.ScientificTitles, "Id", "ScientificTitle");
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
                sal.TotalAmount = this.calculateFinalAmount(sal);
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
            var EMP = _context.Employees.Find(salary.EmployeeId);
            // Income of Marriage status
            taxIncome = (int)(taxIncome - this.calculateMarrigeAllotments(EMP.MarrigeStatus));

            var kids = EMP.KidsNumber;
            if (kids > 0)
                taxIncome -= (kids * 200000);

            if (this.calculateAge(EMP.Birthdate) > 63)
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
        public IActionResult Edit(int id)
        {

            var SAL = _context.Salaries.Find(id);
            var emp = _context.Employees.Where(s => s.Id == SAL.EmployeeId).First();
            if (emp == null)
            {
                return NotFound();
            }
            var employee = new EmployeeCreate()
            {
                FullName = emp.FullName,
                Birthdate = emp.Birthdate,
                IdentityNumber = emp.IdentityNumber,
                DepartmentId = emp.DepartmentId,
                Section = emp.Section,
                IsRatired = emp.IsRatired,
                KidsNumber = emp.KidsNumber,
                GradeId = emp.GradeId,
                StageId = emp.StageId,
                MarrigeStatus = emp.MarrigeStatus,
                InitialSalary = SAL.InitialSalary,
                UniAllotments = SAL.UniAllotments,
                DegreeAllotments = SAL.DegreeAllotments,
                PositionAllotments = SAL.PositionAllotments,
                MarrigeAllotments = SAL.MarrigeAllotments,
                KidsAllotments = SAL.KidsAllotments,
                TransportationAllotments = SAL.TransportationAllotments,
                RetirementSubtraction = SAL.RetirementSubtraction,
                OtherSubtractions = SAL.OtherSubtractions,
                Description = SAL.Description,
                ScientificTitleId = (int)SAL.ScientificTitleId,
                VacationDiff = emp.Salaries.FirstOrDefault().VacationDiff
            };
            
            ViewData["GradeId"] = new SelectList(_context.Grades, "Id", "GradeName", employee.GradeId);
            ViewData["StageId"] = new SelectList(_context.Stages, "Id", "StageName", employee.StageId);
            ViewData["ScientificTitleId"] = new SelectList(_context.ScientificTitles, "Id", "ScientificTitle", emp.Salaries.First().ScientificTitleId);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeCreate employees)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var SAL = _context.Salaries.Find(id);
                    var emp = _context.Employees.Where(s => s.Id == SAL.EmployeeId).First();
                    var emplo = new Employees()
                    {
                        Id = emp.Id,
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


                   
                    var salar = new Salaries()
                    {
                        Id = SAL.Id,
                        EmployeeId = employees.Id,
                        InitialSalary = employees.InitialSalary,
                        UniAllotments = employees.UniAllotments,
                        DegreeAllotments = employees.DegreeAllotments,
                        PositionAllotments = employees.PositionAllotments,
                        MarrigeAllotments = employees.MarrigeAllotments,
                        KidsAllotments = employees.KidsAllotments,
                        TransportationAllotments = employees.TransportationAllotments,
                        RetirementSubtraction = employees.RetirementSubtraction,
                        IncomeTax = SAL.IncomeTax,
                        OtherSubtractions = employees.OtherSubtractions,
                        Description = employees.Description,
                        ScientificTitleId = SAL.ScientificTitleId,
                        ScientificTitles = _context.ScientificTitles.Find(SAL.ScientificTitleId),
                        VacationDiff = (int)employees.VacationDiff,
                        TotalAmount = 0
                    };
                    salar.IncomeTax = this.calculateTaxIncome(salar);
                    salar.TotalAmount = this.calculateFinalAmount(salar);
                    _context.SaveChanges();
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

        private int? calculateFinalAmount(Salaries sal)
        {
            return (int)Math.Round((decimal)(sal.InitialSalary + sal.UniAllotments
                        + sal.DegreeAllotments + sal.PositionAllotments
                        + sal.MarrigeAllotments + sal.KidsAllotments
                        + sal.TransportationAllotments + sal.ScientificTitles.Income
                        - sal.IncomeTax
                        - sal.RetirementSubtraction - sal.OtherSubtractions - sal.VacationDiff));
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
