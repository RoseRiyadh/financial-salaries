using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZulfieP.Models.Binding;
using ZulfieP.Models.Entities;
using ZulfieP.Services;

namespace ZulfieP.Controllers
{
    public class UserController : Controller
    {
        private readonly SalariesContext _context;
        private readonly Cryptography _cryptography;

        public UserController(SalariesContext context, Cryptography cryptography)
        {
            _context = context;
            _cryptography = cryptography;
        }
        // GET: User
        public async Task<IActionResult> Index()
        {
            var salariesContext = _context.Users.Where(s => s.IsDeleted == false);
            return View(await salariesContext.ToListAsync());
        }
        // done
        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users =  _context.Users
                .Where(m => m.Id == id)
                .First();

            if (users == null)
            {
                return NotFound();
            }
            
            return View(users);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUser user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new Users() { 
                    FullName = user.FullName,
                    Username = user.UserName
                };

                var password_salt = Guid.NewGuid().ToString();
                var newPassword = new Passwords()
                {
                    PasswordSalt = password_salt,
                    HashedPassword = _cryptography.HashSHA256(user.Password + password_salt)
                };

                newUser.Password = newPassword;

                _context.Add(newUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            

            var users = await _context.Users.FindAsync(id);
            if (users == null || users.IsDeleted == true)
            {
                return NotFound();
            }
            var userDetails = new CreateUser()
            {
                Id = users.Id,
                UserName = users.Username,
                FullName = users.FullName,
                
            };
            
            return View(userDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateUser users)
        {

            if (id != users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
                    user.Username = users.UserName != null ? users.UserName : user.Username;
                    user.FullName = users.FullName != null ? users.FullName : user.FullName;
                    if (users.Password != null)
                    {
                        var password_salt = Guid.NewGuid().ToString();
                        var newPassword = new Passwords()
                        {
                            PasswordSalt = password_salt,
                            HashedPassword = _cryptography.HashSHA256(users.Password + password_salt)
                        };
                        user.Password = newPassword;
                    }
                    var p = _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.Id))
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
            
            return View(users);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (users == null || users.IsDeleted == true)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            users.IsDeleted = true;
            _context.Users.Update(users);
            //_context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
