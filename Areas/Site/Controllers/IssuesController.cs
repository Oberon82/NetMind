using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetMind.Areas.Site.Models;
using NetMind.Data;
using NetMind.Models;
using NetMind.Areas.Admin.Models;

namespace NetMind.Areas.Site.Controllers
{
    [Area("Site")]
    public class IssuesController : Controller
    {
        private readonly ApplicationContext _context;

        public IssuesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Site/Issues
        public async Task<IActionResult> Index()
        {
            var netMindContext = _context.Issue.Include(i => i.Assignee).Include(i => i.Creator).Include(i => i.Priority).Include(i => i.Status).Include(i => i.Tracker).
                Where(p => p.IsClosed == false);
            return View(await netMindContext.ToListAsync());
        }

        // GET: Site/Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .Include(i => i.Assignee)
                .Include(i => i.Creator)
                .Include(i => i.Priority)
                .Include(i => i.Status)
                .Include(i => i.Tracker)
                .FirstOrDefaultAsync(m => m.IssueID == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Site/Issues/Create
        public IActionResult Create()
        {
            if (_context.Trackers.Count() == 0)
            {
                return RedirectToAction("Create", "Trackers", new { area = "Admin" });
            }
            else if (_context.Priorities.Count() == 0)
            {
                return RedirectToAction("Create", "Priorities", new { area = "Admin" });
            }
            else if (_context.Statuses.Count() == 0)
            {
                return RedirectToAction("Create", "Statuses", new { area = "Admin" });
            }

            ViewData["Assignee"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["Priority"] = new SelectList(_context.Priorities, "PriorityID", "Name");
            ViewData["Status"] = new SelectList(_context.Statuses, "StatusID", "Name");
            ViewData["Tracker"] = new SelectList(_context.Trackers, "TrackerID", "Name");
            return View();
        }

        // POST: Site/Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IssueID,ProjectID,CreatorID,AssigneeID,Title,Description,StatusID,TrackerID,PriorityID")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                issue.CreatedOn = DateTime.Now;
                issue.UpdatedOn = DateTime.Now;
                string tmpid = User.FindFirst(x => x.Type == "id").Value;

                issue.CreatorID = int.Parse(tmpid);

                Status status = _context.Statuses.FirstOrDefault(p => p.StatusID == issue.StatusID);

                if (status is not null)
                {
                    issue.IsClosed = status.IsClosed;
                }

                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Assignee"] = new SelectList(_context.Users, "Id", "UserName", issue.AssigneeID);
            ViewData["Priority"] = new SelectList(_context.Priorities, "PriorityID", "Name", issue.PriorityID);
            ViewData["Status"] = new SelectList(_context.Statuses, "StatusID", "Name", issue.StatusID);
            ViewData["Tracker"] = new SelectList(_context.Trackers, "TrackerID", "Name", issue.TrackerID);
            return View(issue);
        }

        // GET: Site/Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }           

            var issue = await _context.Issue.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }
            ViewData["Assignee"] = new SelectList(_context.Users, "Id", "UserName", issue.AssigneeID);
            ViewData["Priority"] = new SelectList(_context.Priorities, "PriorityID", "Name", issue.PriorityID);
            ViewData["Status"] = new SelectList(_context.Statuses, "StatusID", "Name", issue.StatusID);
            ViewData["Tracker"] = new SelectList(_context.Trackers, "TrackerID", "Name", issue.TrackerID);
            return View(issue);
        }

        // POST: Site/Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IssueID,ProjectID,CreatorID,CreatedOn,AssigneeID,UpdatedOn,Title,Description,StatusID,TrackerID,PriorityID")] Issue issue)
        {
            if (id != issue.IssueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Status status = _context.Statuses.FirstOrDefault(p => p.StatusID == issue.StatusID);

                    if (status is not null)
                    {
                        issue.IsClosed = status.IsClosed;
                    }

                    _context.Update(issue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(issue.IssueID))
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
            ViewData["Assignee"] = new SelectList(_context.Users, "Id", "UserName", issue.AssigneeID);
            ViewData["Priority"] = new SelectList(_context.Priorities, "PriorityID", "Name", issue.PriorityID);
            ViewData["Status"] = new SelectList(_context.Statuses, "StatusID", "Name", issue.StatusID);
            ViewData["Tracker"] = new SelectList(_context.Trackers, "TrackerID", "Name", issue.TrackerID);
            return View(issue);
        }

        // GET: Site/Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .Include(i => i.Assignee)
                .Include(i => i.Creator)
                .Include(i => i.Priority)
                .Include(i => i.Status)
                .Include(i => i.Tracker)
                .FirstOrDefaultAsync(m => m.IssueID == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // POST: Site/Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issue = await _context.Issue.FindAsync(id);
            _context.Issue.Remove(issue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
            return _context.Issue.Any(e => e.IssueID == id);
        }
    }
}
