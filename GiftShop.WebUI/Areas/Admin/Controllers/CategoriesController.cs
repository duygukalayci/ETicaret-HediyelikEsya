using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftShop.Data;
using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GiftShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly DatabaseContext _context;

        public CategoriesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public async Task<IActionResult>Create()
        {
            ViewBag.CategoryList = new SelectList(
                await _context.Categories.ToListAsync(),
                            "CategoryID", "Name");
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreateDate = DateTime.Now;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(
                await _context.Categories.ToListAsync(),
                "CategoryID", "Name");

            return View(category);
        }


        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _context.Categories
                         .Include(c => c.ParentCategory)
                         .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
                return NotFound();

            ViewBag.CategoryList = new SelectList(
                await _context.Categories
                    .Where(c => c.CategoryID != id) // kendi kendine atanmasın
                    .ToListAsync(),
                "CategoryID", "Name");

            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Category category)
        {
            if (id != category.CategoryID)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(
                await _context.Categories
                    .Where(c => c.CategoryID != id)
                    .ToListAsync(),
                "CategoryID", "Name");

            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
    }
}
