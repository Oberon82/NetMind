using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetMind.Areas.Admin.Models;
using NetMind.Data;

namespace NetMind.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PrioritiesController : Controller
    {
        private readonly ApplicationContext _context;
        private async Task ClearIsDefault(Priority priority)
        {
            await Task.Run(async () =>
            {
                if (priority.IsDefault)
                {
                    Priority _priority = await _context.Priorities.Where(p => p.IsDefault & p.PriorityID != priority.PriorityID).FirstOrDefaultAsync();
                    if (_priority is not null)
                    {
                        _priority.IsDefault = false;
                        _context.Priorities.Update(_priority);
                    }

                }
            });
        }

        public PrioritiesController(ApplicationContext context)
        {
            _context = context;
            //if (_context.Priorities.Count() == 0)
            //{
            //    _context.AddRange(
            //        new Priority() { Active = true, IsDefault = false, Name = "Low", Order = 1 },
            //        new Priority() { Active = true, IsDefault = true, Name = "Normal", Order = 2},
            //        new Priority() { Active = true, IsDefault = false, Name = "High", Order = 3}
            //        );
            //     _context.SaveChanges();
            //}
        }

        // GET: Admin/Priorities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Priorities.OrderBy(p => p.Order).ToListAsync());
        }

        // GET: Admin/Priorities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Priorities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PriorityID,Name,IsDefault,Active")] Priority priority)
        {
            if (ModelState.IsValid)
            {
                await ClearIsDefault(priority);
                priority.Order = 1;
                var _priority = await _context.Priorities.OrderBy(p => p.Order).LastOrDefaultAsync();
                if (_priority is not null)
                {
                    priority.Order = _priority.Order + 1;
                }
                _context.Add(priority);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(priority);
        }

        // GET: Admin/Priorities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priority = await _context.Priorities.FindAsync(id);
            if (priority == null)
            {
                return NotFound();
            }
            return View(priority);
        }

        // POST: Admin/Priorities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PriorityID,Name,IsDefault,Active,Order")] Priority priority)
        {
            if (id != priority.PriorityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ClearIsDefault(priority);
                    _context.Update(priority);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriorityExists(priority.PriorityID))
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
            return View(priority);
        }

        // GET: Admin/Priorities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priority = await _context.Priorities
                .FirstOrDefaultAsync(m => m.PriorityID == id);
            if (priority == null)
            {
                return NotFound();
            }

            return View(priority);
        }

        // POST: Admin/Priorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priority = await _context.Priorities.FindAsync(id);
            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriorityExists(int id)
        {
            return _context.Priorities.Any(e => e.PriorityID == id);
        }

        [Route("Admin/Priorities/Sort/{id?}/{sort?}")]
        public async Task<IActionResult> Sort(int? id, SortTo sort)
        {
            if (id is not null)
            {
                Priority priority = await _context.Priorities.FirstOrDefaultAsync(p => p.PriorityID == id);
                if (priority != null)
                {
                    await SortOrderHelper<Priority>.SortOrder(_context, _context.Priorities, sort, priority);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
