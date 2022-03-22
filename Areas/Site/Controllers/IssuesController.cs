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
            var netMindContext = _context.Issue;//.Include(i => i.Assignee).Include(i => i.Creator).Include(i => i.Priority).Include(i => i.Project).Include(i => i.Status).Include(i => i.Tracker);
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
                .Include(i => i.Project)
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
            ViewData["Assignee"] = new SelectList(_context.Users, "UserID", "Name");
            ViewData["Creator"] = new SelectList(_context.Users, "UserID", "Name");
            ViewData["Priority"] = new SelectList(_context.Priorities, "PriorityID", "Name");
            ViewData["Project"] = new SelectList(_context.Projects, "ProjectID", "Name");
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

                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Assignee"] = new SelectList(_context.Users, "UserID", "Name", issue.AssigneeID);
            ViewData["Creator"] = new SelectList(_context.Users, "UserID", "Name", issue.CreatorID);
            ViewData["Priority"] = new SelectList(_context.Priorities, "PriorityID", "Name", issue.PriorityID);
            ViewData["Project"] = new SelectList(_context.Projects, "ProjectID", "Name", issue.ProjectID);
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
            ViewData["AssigneeID"] = new SelectList(_context.Users, "UserID", "Name", issue.AssigneeID);
            ViewData["CreatorID"] = new SelectList(_context.Users, "UserID", "Name", issue.CreatorID);
            ViewData["PriorityID"] = new SelectList(_context.Priorities, "PriorityID", "Name", issue.PriorityID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Name", issue.ProjectID);
            ViewData["StatusID"] = new SelectList(_context.Statuses, "StatusID", "Name", issue.StatusID);
            ViewData["TrackerID"] = new SelectList(_context.Trackers, "TrackerID", "Name", issue.TrackerID);
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
            ViewData["AssigneeID"] = new SelectList(_context.Users, "UserID", "Name", issue.AssigneeID);
            ViewData["CreatorID"] = new SelectList(_context.Users, "UserID", "Name", issue.CreatorID);
            ViewData["PriorityID"] = new SelectList(_context.Priorities, "PriorityID", "Name", issue.PriorityID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Name", issue.ProjectID);
            ViewData["StatusID"] = new SelectList(_context.Statuses, "StatusID", "Name", issue.StatusID);
            ViewData["TrackerID"] = new SelectList(_context.Trackers, "TrackerID", "Name", issue.TrackerID);
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
                .Include(i => i.Project)
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
