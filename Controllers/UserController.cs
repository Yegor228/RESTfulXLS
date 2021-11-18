using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RESTfulXLS.Contexts;
using RESTfulXLS.Models;
using RESTfulXLS.Controllers;

namespace RESTfulXLS.Controllers
{
    [Controller]
    public class UserController : Controller
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> UserlistShow()
        {
            return View(await _context.Users.ToListAsync());
        }
        
        public IActionResult UserAuthorization()
        {
            var tenders = new List<Tender>();
            return View(tenders);
        }


        [HttpPost]
        public IActionResult UserAuthorization(string name, string password)
        {
            var parser = new XlsWorker("Data/Data.xlsx");
            var tenders = parser.XlsParser();
            var bytes = Encoding.ASCII.GetBytes(name + password);
            string base64Code = Convert.ToBase64String(bytes);

            if (_context.Users.Any(x => x.Base64Code == base64Code))
                return View(tenders);
            return Redirect("~/Tender/Index");
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string password)
        {
            User user = new User();
            user.Name = name;
            user.Password = password;
            var bytes = Encoding.ASCII.GetBytes(user.Name + user.Password);
            user.Base64Code = Convert.ToBase64String(bytes);
            _context.Users.Add(user);
            //_context.SaveChanges();
            return Redirect("UserlistShow");
        }
    }
}
