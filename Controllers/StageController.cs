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
    public class StageController : Controller
    {
        private readonly SalariesContext _context;

        public StageController(SalariesContext context)
        {
            _context = context; 
        }

        // GET: Stage
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stages.Where(s => s.IsDeleted != true).ToListAsync());
        }

        // GET: Stage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stages = await _context.Stages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stages == null || stages.IsDeleted == true)
            {
                return NotFound();
            }

            return View(stages);
        }

        // GET: Stage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StageName,IsDeleted")] Stages stages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stages);
        }

        // GET: Stage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stages = await _context.Stages.FindAsync(id);
            if (stages == null || stages.IsDeleted == true)
            {
                return NotFound();
            }
            return View(stages);
        }

        // POST: Stage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StageName,IsDeleted")] Stages stages)
        {
            if (id != stages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StagesExists(stages.Id))
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
            return View(stages);
        }

        // GET: Stage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stages = await _context.Stages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stages == null || stages.IsDeleted == true)
            {
                return NotFound();
            }

            return View(stages);
        }

        // POST: Stage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stages = await _context.Stages.FindAsync(id);
            // _context.Stages.Remove(stages);
            stages.IsDeleted = true;
            _context.Stages.Update(stages);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StagesExists(int id)
        {
            return _context.Stages.Any(e => e.Id == id);
        }
    }
}
