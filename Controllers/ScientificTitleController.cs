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
    public class ScientificTitleController : Controller
    {
        private readonly SalariesContext _context;

        public ScientificTitleController(SalariesContext context)
        {
            _context = context;
        }

        // GET: ScientificTitle
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScientificTitles.Where(s => s.IsDeleted != true).ToListAsync());
        }

        // GET: ScientificTitle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientificTitles = await _context.ScientificTitles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scientificTitles == null || scientificTitles.IsDeleted == true)
            {
                return NotFound();
            }

            return View(scientificTitles);
        }

        // GET: ScientificTitle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScientificTitle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,ScientificTitle,Income,IsDeleted")] ScientificTitles scientificTitles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scientificTitles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scientificTitles);
        }

        // GET: ScientificTitle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scientificTitles = await _context.ScientificTitles.FindAsync(id);
            if (scientificTitles == null || scientificTitles.IsDeleted == true)
            {
                return NotFound();
            }
            return View(scientificTitles);
        }

        // POST: ScientificTitle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,ScientificTitle,Income,IsDeleted")] ScientificTitles scientificTitles)
        {
            if (id != scientificTitles.Id || scientificTitles.IsDeleted == true)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scientificTitles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScientificTitlesExists(scientificTitles.Id))
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
            return View(scientificTitles);
        }

        // GET: ScientificTitle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var scientificTitles = await _context.ScientificTitles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scientificTitles == null || scientificTitles.IsDeleted == true)
            {
                return NotFound();
            }

            return View(scientificTitles);
        }

        // POST: ScientificTitle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scientificTitles = await _context.ScientificTitles.FindAsync(id);

            // _context.ScientificTitles.Remove(scientificTitles);
            scientificTitles.IsDeleted = true;
            _context.ScientificTitles.Update(scientificTitles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScientificTitlesExists(int id)
        {
            return _context.ScientificTitles.Any(e => e.Id == id);
        }
    }
}
