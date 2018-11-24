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
    public class SocialLinkController : Controller
    {
        private readonly DBModelContext _context;

        public SocialLinkController(DBModelContext context)
        {
            _context = context;
        }

        // GET: SocialLinks
        public async Task<IActionResult> Index()
        {
            return View(await _context.SocialLinks.ToListAsync());
        }

        // GET: SocialLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialLink = await _context.SocialLinks
                .FirstOrDefaultAsync(m => m.SocialLinkId == id);
            if (socialLink == null)
            {
                return NotFound();
            }

            return View(socialLink);
        }

        // GET: SocialLinks/Create
       /*
       public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var socialLink = new SocialLink
            {
                AttendeeId = (int)id
            };
            return View(socialLink);
        }*/

        // GET: SocialLinks/Create
        public async Task<IActionResult> Create([Bind("AttendeeId,URL")] SocialLink socialLink)
        {
            if (socialLink == null)
            {
                return NotFound();
            }
            _context.Add(socialLink);
            await _context.SaveChangesAsync();
            return View(socialLink);
        }

        // POST: SocialLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SocialLinkId,AttendeeId,URL")] SocialLink socialLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(socialLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(socialLink);
        }*/

        // GET: SocialLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialLink = await _context.SocialLinks.FindAsync(id);
            if (socialLink == null)
            {
                return NotFound();
            }
            return View(socialLink);
        }

        // POST: SocialLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SocialLinkId,URL")] SocialLink socialLink)
        {
            if (socialLink == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(socialLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialLinkExists(socialLink.SocialLinkId))
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
            return View(socialLink);
        }

        // GET: SocialLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialLink = await _context.SocialLinks
                .FirstOrDefaultAsync(m => m.SocialLinkId == id);
            if (socialLink == null)
            {
                return NotFound();
            }

            return View(socialLink);
        }

        // POST: SocialLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var socialLink = await _context.SocialLinks.FindAsync(id);
            _context.SocialLinks.Remove(socialLink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocialLinkExists(int id)
        {
            return _context.SocialLinks.Any(e => e.SocialLinkId == id);
        }
    }
}