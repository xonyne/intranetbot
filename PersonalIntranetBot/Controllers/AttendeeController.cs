using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalIntranetBot.Models;

namespace EFGetStarted.AspNetCore.NewDb.Controllers
{
    public class AttendeeController : Controller
    {
        private readonly DBModelContext _context;

        public AttendeeController(DBModelContext context)
        {
            _context = context;
        }

        // GET: SocialLinks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Attendees.ToListAsync());
        }

        // GET: SocialLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendees.Include(s => s.SocialLinks)
                .FirstOrDefaultAsync(m => m.AttendeeId == id);
            if (attendee == null)
            {
                return NotFound();
            }

            return View(attendee);
        }

        // GET: SocialLinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: SocialLinks/Create
        public IActionResult CreateSocialLink(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            return View(new SocialLink { AttendeeId = (int)id});
        }

        // GET: SocialLinks/Create
        public async Task<IActionResult> OnSaveSocialLink([Bind("AttendeeId,URL")] SocialLink socialLink)
        {
            if (socialLink == null)
            {
                return NotFound();
            }
            _context.Add(socialLink);
            await _context.SaveChangesAsync();
            return View("Details");
        }

        // POST: SocialLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendeeId,EmailAddress")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendee);
        }

        // GET: SocialLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendees.FindAsync(id);
            if (attendee == null)
            {
                return NotFound();
            }
            return View(attendee);
        }

        // POST: SocialLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttendeeId,EmailAddress")] Attendee attendee)
        {
            if (attendee == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendeeExists(attendee.AttendeeId))
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
            return View(attendee);
        }

        // GET: SocialLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(m => m.AttendeeId == id);
            if (attendee == null)
            {
                return NotFound();
            }

            return View(attendee);
        }

        // POST: SocialLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendee = await _context.Attendees.FindAsync(id);
            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendeeExists(int id)
        {
            return _context.Attendees.Any(e => e.AttendeeId == id);
        }
    }
}