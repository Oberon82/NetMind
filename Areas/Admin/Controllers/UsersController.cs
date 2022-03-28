using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetMind.Data;

namespace NetMind.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {

        private readonly ApplicationContext _context;
        public UsersController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Users.OrderBy(p=>p.Id).ToList());
        }
    }
}
