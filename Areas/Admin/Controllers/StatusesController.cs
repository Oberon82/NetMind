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
    public class StatusesController : Controller
    {
        private readonly ApplicationContext _context;

        public StatusesController(ApplicationContext context)
        {
            _context = context;
            //if (_context.Statuses.Count() == 0)
            //{
            //    _context.Statuses.AddRange(
            //        new Status() { IsClosed = false, IsDefault = true, Name  = "Open", Order = 1},
            //        new Status() { IsClosed = false, IsDefault = false, Name = "In Progress", Order = 2},
            //        new Status() { IsClosed = false, IsDefault = false, Name = "To be tested", Order = 3},
            //        new Status() { IsClosed = true, IsDefault = false, Name = "Closed", Order = 4},
            //        new Status() { IsClosed = false, IsDefault = false, Name = "Reopen", Order = 5},
            //        new Status() { IsClosed = true, IsDefault = false, Name = "Rejected", Order = 6}
            //        );
            //    _context.SaveChanges();
            //}
        }

        // GET: Admin/Statuses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Statuses.OrderBy(p => p.Order).ToListAsync());
        }

        // GET: Admin/Statuses/Create
        public IActionResult Create()
        {
            return View();
        }

        private async Task ClearIsDefault(Status status)
        {
            await Task.Run(async () =>
            {
                if (status.IsDefault)
                {
                    Status _status = await _context.Statuses.Where(p => p.IsDefault & p.StatusID != status.StatusID).FirstOrDefaultAsync();
                    if (_status is not null)
                    {
                        _status.IsDefault = false;
                        _context.Statuses.Update(_status);
                    }

                }
            }
                );
        }

        // POST: Admin/Statuses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusID,Name,IsDefault,IsClosed,Order")] Status status)
        {
            if (ModelState.IsValid)
            {
                await ClearIsDefault(status);
                status.Order = 1;
                var _status = await _context.Statuses.OrderBy(p => p.Order).LastOrDefaultAsync();
                if (_status is not null)
                {
                    status.Order = _status.Order + 1;
                }
                _context.Add(status);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(status);
        }

        // GET: Admin/Statuses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.Statuses.FindAsync(id);
            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        // POST: Admin/Statuses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusID,Name,IsDefault,IsClosed,Order")] Status status)
        {
            if (id != status.StatusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ClearIsDefault(status);
                    _context.Update(status);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusExists(status.StatusID))
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
            return View(status);
        }

        // GET: Admin/Statuses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.Statuses
                .FirstOrDefaultAsync(m => m.StatusID == id);
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // POST: Admin/Statuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var status = await _context.Statuses.FindAsync(id);
            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusExists(int id)
        {
            return _context.Statuses.Any(e => e.StatusID == id);
        }

        [Route("Admin/Status/Sort/{id?}/{sort?}")]
        public async Task<IActionResult> Sort(int? id, SortTo sort)
        {
            if (id is not null)
            {
                Status status = await _context.Statuses.FirstOrDefaultAsync(p => p.StatusID == id);
                if (status != null)
                {
                    await SortOrderHelper<Status>.SortOrder(_context, _context.Statuses, sort, status);
                    //List<Status> tmpList = null;

                    //switch (sort)
                    //{
                    //    // Moving to the first position 
                    //    case 0:
                    //        tmpList = await _context.Statuses.Where(p => p.Order < status.Order).OrderBy(p => p.Order).ToListAsync();
                    //        tmpList.Insert(0, status);
                    //        int i = 0;
                    //        foreach (Status _status in tmpList)
                    //        {
                    //            _status.Order = ++i;
                    //            _context.Statuses.Update(_status);
                    //        }
                    //        await _context.SaveChangesAsync();
                    //        break;
                    //    // Moving one position up
                    //    case 1:
                    //        var lastStatus = await _context.Statuses.Where(p => p.Order < status.Order).OrderBy(p => p.Order).LastOrDefaultAsync();
                    //        if (lastStatus is not null)
                    //        {
                    //            i = lastStatus.Order;
                    //            lastStatus.Order = status.Order;
                    //            status.Order = i;
                    //            _context.Statuses.Update(lastStatus);
                    //            _context.Statuses.Update(status);
                    //            await _context.SaveChangesAsync();
                    //        }
                    //        break;
                    //    // Moving one position down
                    //    case 2:
                    //        var nextstatus = await _context.Statuses.Where(p => p.Order > status.Order).OrderBy(p => p.Order).FirstOrDefaultAsync();
                    //        if (nextstatus is not null)
                    //        {
                    //            i = nextstatus.Order;
                    //            nextstatus.Order = status.Order;
                    //            status.Order = i;
                    //            _context.Statuses.Update(status);
                    //            _context.Statuses.Update(nextstatus);
                    //            await _context.SaveChangesAsync();
                    //        }
                    //        break;
                    //    // Moving to the last position
                    //    case 3:
                    //        tmpList = await _context.Statuses.Where(p => p.Order > status.Order).OrderBy(p => p.Order).ToListAsync();
                    //        tmpList.Add(status);
                    //        int j = status.Order;
                    //        foreach (Status _status in tmpList)
                    //        {
                    //            _status.Order = j++;
                    //            _context.Statuses.Update(_status);
                    //        }
                    //        await _context.SaveChangesAsync();
                    //        break;
                    //}
                }
            }

            return RedirectToAction("Index");
        }
    }
}
