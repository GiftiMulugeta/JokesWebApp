using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace JokesWebApp.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }
      
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(account accounts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accounts);
                await _context.SaveChangesAsync();
                //return Content("Registration successful!");
                return RedirectToAction("Login","Jokes");
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        void connectionString()
        {
            con.ConnectionString = "server=.; database=JokesWeb; integrated security = true; TrustServerCertificate=True";
        }
        [HttpPost]
        public IActionResult Verify(account acc)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from Accounts where username = '" + acc.username + "' and password='" + acc.password + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                //HttpContext.Session.SetString("StuId", acc.StuId);
                //HttpContext.Session.SetString("user", acc.Role = "");

                return RedirectToAction("Index", "Jokes");


            }
            else
            {
                con.Close();
                return View("Login");
            }
        }
       
        public async Task<IActionResult> Index()
        {
            return _context.Jokes != null ?
                        View(await _context.Jokes.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Jokes'  is null.");
        }
        //i created another controller to selectively display what i ant on showsearchform i erased edit and delete from the previous index view
        public async Task<IActionResult> ShowIndex()
        {
            return _context.Jokes != null ?
                        View(await _context.Jokes.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Jokes'  is null.");
        }
        public async Task<IActionResult> ShowSearchForm()
        {
            return  View();
        }
     public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("ShowIndex", await _context.Jokes.Where(j => j.JokeQuestion.Contains(SearchPhrase)).ToListAsync());
        }
        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jokes == null)
            {
                return NotFound();
            }

            var joke = await _context.Jokes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // GET: Jokes/Create
       // [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> Create([Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
        {
            if (ModelState.IsValid)
            {
                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: Jokes/Edit/5
        //[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jokes == null)
            {
                return NotFound();
            }

            var joke = await _context.Jokes.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
        {
            if (id != joke.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.Id))
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
            return View(joke);
        }

        // GET: Jokes/Delete/5
        //[Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jokes == null)
            {
                return NotFound();
            }

            var joke = await _context.Jokes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jokes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Jokes'  is null.");
            }
            var joke = await _context.Jokes.FindAsync(id);
            if (joke != null)
            {
                _context.Jokes.Remove(joke);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
          return (_context.Jokes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
