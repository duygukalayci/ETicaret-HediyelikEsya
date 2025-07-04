using GiftShop.Data;
using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactMessagesController : Controller
    {
        private readonly DatabaseContext _context;

        public ContactMessagesController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.ContactMessages);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMessage = await _context.ContactMessages
                .FirstOrDefaultAsync(m => m.ContactMessageID == id);

            if (contactMessage == null)
            {
                return NotFound();
            }

            // Eğer daha önce okunmadıysa, okundu olarak işaretle
            if (!contactMessage.IsRead)
            {
                contactMessage.IsRead = true;
                _context.Update(contactMessage);
                await _context.SaveChangesAsync();
            }

            return View(contactMessage);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ContactMessages = await _context.ContactMessages.FindAsync(id);
            if (ContactMessages == null)
            {
                return NotFound();
            }
            return View(ContactMessages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactMessage ContactMessage)
        {
            if (id != ContactMessage.ContactMessageID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ContactMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(ContactMessage.ContactMessageID))
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
            return View(ContactMessage);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ContactMessages = await _context.ContactMessages.FindAsync(id);
            if (ContactMessages == null)
            {
                return NotFound();
            }
            return View(ContactMessages);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ContactMessages = await _context.ContactMessages.FindAsync(id);
            if (ContactMessages != null)
            {
                _context.ContactMessages.Remove(ContactMessages);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ContactExists(int id)
        {
            return _context.ContactMessages.Any(e => e.ContactMessageID == id);
        }


    }
}
