using GiftShop.Data;
using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class OrdersController : Controller
    {
        private readonly DatabaseContext _context;

        public OrdersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.AppUser)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            return View(orders);
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            ViewData["AppUserID"] = new SelectList(_context.AppUsers, "AppUserID", "Email");
            return View();
        }

        // POST: Admin/Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,OrderDate,TotalAmount,Status,AppUserID")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AppUserID"] = new SelectList(_context.AppUsers, "AppUserID", "Email", order.AppUserID);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return NotFound();

            LoadStatusList(order.Status);
            // Sipariş durumları listesi
            return View(order);
        }
        // POST: Admin/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,Status")] Order updatedOrder)
        {
            if (id != updatedOrder.OrderID)
                return NotFound();

            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null) return NotFound();

            existingOrder.Status = updatedOrder.Status;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sipariş durumu başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Güncelleme sırasında hata oluştu: " + ex.Message);
            }

            LoadStatusList(existingOrder.Status);
            return View(existingOrder);
        }


        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Sipariş durumu seçenekleri
        private void LoadStatusList(string selectedStatus = null)
        {
            ViewData["StatusList"] = new SelectList(new[]
            {
                "Yeni",
                "Hazırlanıyor",
                "Kargoya Verildi",
                "Teslim Edildi",
                "İptal Edildi"
            }, selectedStatus);
        }


        // Sipariş var mı kontrolü
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
