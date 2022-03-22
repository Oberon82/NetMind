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
    public class TrackersController : Controller
    {
        private readonly ApplicationContext _context;

        public TrackersController(ApplicationContext context)
        {
            _context = context;
            //if (_context.Trackers.Count() == 0)
            //{
            //    _context.Trackers.Add(new Tracker() { Name = "Bug", Order = 1});
            //    _context.SaveChanges();
            //}
        }

        // GET: Admin/Trackers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trackers.OrderBy(p=>p.Order).ToListAsync());
        }

        // GET: Admin/Trackers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Trackers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackerID,Name")] Tracker tracker)
        {
            if (ModelState.IsValid)
            {
                tracker.Order = 1;
                var _tracker = await _context.Trackers.OrderBy(p => p.Order).LastOrDefaultAsync();
                if (_tracker is not null)
                {
                    tracker.Order = _tracker.Order + 1;
                }
                _context.Add(tracker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tracker);
        }

        // GET: Admin/Trackers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tracker = await _context.Trackers.FindAsync(id);
            if (tracker == null)
            {
                return NotFound();
            }
            return View(tracker);
        }

        // POST: Admin/Trackers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrackerID,Name,Order")] Tracker tracker)
        {
            if (id != tracker.TrackerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tracker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackerExists(tracker.TrackerID))
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
            return View(tracker);
        }

        // GET: Admin/Trackers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tracker = await _context.Trackers
                .FirstOrDefaultAsync(m => m.TrackerID == id);
            if (tracker == null)
            {
                return NotFound();
            }

            return View(tracker);
        }

        // POST: Admin/Trackers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tracker = await _context.Trackers.FindAsync(id);
            _context.Trackers.Remove(tracker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackerExists(int id)
        {
            return _context.Trackers.Any(e => e.TrackerID == id);
        }

        [Route("Admin/Trackers/Sort/{id?}/{sort?}")]
        public async Task<IActionResult> Sort(int? id, SortTo sort)
        {
            if (id is not null)
            {
                Tracker tracker = await _context.Trackers.FirstOrDefaultAsync(p => p.TrackerID == id);
                if (tracker != null)
                {
                    await SortOrderHelper<Tracker>.SortOrder(_context, _context.Trackers, sort, tracker);
                    
                    //List<Tracker> tmpList = null;

                    //switch (sort)
                    //{
                    //    // Moving to the first position 
                    //    case 0:
                    //        tmpList = await _context.Trackers.Where(p => p.Order < tracker.Order).OrderBy(p => p.Order).ToListAsync();
                    //        tmpList.Insert(0, tracker);
                    //        int i = 0;
                    //        foreach (Tracker _tracker in tmpList)
                    //        {
                    //            _tracker.Order = ++i;
                    //            _context.Trackers.Update(_tracker);
                    //        }
                    //        await _context.SaveChangesAsync();
                    //        break;
                    //    // Moving one position up
                    //    case 1:
                    //        var lastTracker = await _context.Trackers.Where(p => p.Order < tracker.Order).OrderBy(p => p.Order).LastOrDefaultAsync();
                    //        if (lastTracker is not null)
                    //        {
                    //            i = lastTracker.Order;
                    //            lastTracker.Order = tracker.Order;
                    //            tracker.Order = i;
                    //            _context.Trackers.Update(lastTracker);
                    //            _context.Trackers.Update(tracker);
                    //            await _context.SaveChangesAsync();
                    //        }
                    //        break;
                    //    // Moving one position down
                    //    case 2:
                    //        var nexttracker = await _context.Trackers.Where(p => p.Order > tracker.Order).OrderBy(p => p.Order).FirstOrDefaultAsync();
                    //        if (nexttracker is not null)
                    //        {
                    //            i = nexttracker.Order;
                    //            nexttracker.Order = tracker.Order;
                    //            tracker.Order = i;
                    //            _context.Trackers.Update(tracker);
                    //            _context.Trackers.Update(nexttracker);
                    //            await _context.SaveChangesAsync();
                    //        }
                    //        break;
                    //    // Moving to the last position
                    //    case 3:
                    //        tmpList = await _context.Trackers.Where(p => p.Order > tracker.Order).OrderBy(p => p.Order).ToListAsync();
                    //        tmpList.Add(tracker);
                    //        int j = tracker.Order;
                    //        foreach (Tracker _tracker in tmpList)
                    //        {
                    //            _tracker.Order = j++;
                    //            _context.Trackers.Update(_tracker);
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
