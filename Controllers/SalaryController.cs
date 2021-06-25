using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZulfieP.Models.Entities;

namespace ZulfieP.Controllers
{
    public class SalaryController : Controller
    {
        private readonly SalariesContext _context;

        public SalaryController(SalariesContext context)
        {
            _context = context;
        }

        // GET: Salary
        public IActionResult Index()
        {

            var salariesContext = _context.Salaries.Include(s => s.Employee).Include(s => s.ScientificTitles).Where(s => s.IsDeleted != true && s.Employee.IsDeleted != true).ToList();
            foreach (var salary in salariesContext)
            {
                salary.IncomeTax = this.calculateTaxIncome(salary);
            }
            return View(salariesContext);
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
             if(taxIncome <= 250000)
                taxIncome = (int)(taxIncome - (Math.Round(taxIncome * 0.03)));
            else if (taxIncome > 250000 || taxIncome <= 500000)
                taxIncome = (int)(taxIncome - (Math.Round(taxIncome * 0.05)));
            else if (taxIncome > 500000 || taxIncome <= oneMillion)
                taxIncome = (int)(taxIncome - (Math.Round(taxIncome * 0.1)));
            else if (taxIncome > oneMillion)
                taxIncome = (int)(taxIncome-(Math.Round(taxIncome * 0.15)));
            taxIncome += 70000;
            taxIncome = (int)Math.Round((decimal)(taxIncome / 12));


            // final equation
            return taxIncome;
        }

        private int calculateAge(DateTime birthdate)
        {
            return DateTime.Now.AddYears(-birthdate.Year).Year;
            
        }

        // todo : calculate age to add 300 000 to every employee older than 63


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

        // GET: Salary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaries = await _context.Salaries
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salaries == null)
            {
                return NotFound();
            }

            return View(salaries);
        }

        // GET: Salary/Create
        public IActionResult Create(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        // POST: Salary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, Salaries salaries)
        {
            if (ModelState.IsValid)
            {
                var employee = _context.Employees.Find(id);
                if (employee == null)
                    return RedirectToAction("create", "Employee");
                salaries.EmployeeId = id;
                salaries.RetirementSubtraction =(int)(Math.Round(salaries.InitialSalary * 0.1));
                salaries.IncomeTax = this.calculateTaxIncome(salaries);
                salaries.KidsAllotments = (int)(salaries.Employee.KidsNumber * 10000);
                salaries.MarrigeAllotments = this.calculateMarrigeAllotments(employee.MarrigeStatus);
                salaries.TotalAmount = (int)Math.Round((decimal)(salaries.InitialSalary + salaries.UniAllotments
                    + salaries.DegreeAllotments + salaries.PositionAllotments
                    + salaries.MarrigeAllotments + salaries.KidsAllotments
                    + salaries.TransportationAllotments + salaries.ScientificTitles.Income
                    - salaries.IncomeTax
                    - salaries.RetirementSubtraction - salaries.OtherSubtractions - salaries.VacationDiff));

                _context.Add(salaries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salaries);
        }
        /**
         * If 
         */
        

        // GET: Salary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaries = await _context.Salaries.FindAsync(id);
            if (salaries == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", salaries.EmployeeId);
            return View(salaries);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Salaries salaries)
        {
            if (id != salaries.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salaries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalariesExists(salaries.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", salaries.EmployeeId);
            return View(salaries);
        }

        // GET: Salary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaries = await _context.Salaries
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salaries == null)
            {
                return NotFound();
            }

            return View(salaries);
        }

        // POST: Salary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaries = await _context.Salaries.FindAsync(id);
            // _context.Salaries.Remove(salaries);
            var salary = _context.Salaries.Find(id);
            salary.IsDeleted = true;
            _context.Update(salary);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalariesExists(int id)
        {
            return _context.Salaries.Any(e => e.Id == id);
        }
    }
}
