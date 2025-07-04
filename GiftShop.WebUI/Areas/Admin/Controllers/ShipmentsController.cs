using GiftShop.Data;
using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiftShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ShipmentsController : Controller
    {
        private readonly DatabaseContext _context;

        public ShipmentsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Shipments
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Shipments.Include(s => s.Order);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Admin/Shipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.Order)
                .FirstOrDefaultAsync(m => m.ShipmentID == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // GET: Admin/Shipments/Create
        public IActionResult Create()
        {
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "Status");
            return View();
        }

        // POST: Admin/Shipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShipmentID,OrderID,Carrier,TrackingNumber,ShippedDate,DeliveredDate")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "Status", shipment.OrderID);
            return View(shipment);
        }

        // GET: Admin/Shipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var shipment = await _context.Shipments
                .Include(s => s.Order)
                .ThenInclude(o => o.AppUser)
                .FirstOrDefaultAsync(s => s.ShipmentID == id);

            if (shipment == null) return NotFound();

            return View(shipment);
        }

        // POST: Admin/Shipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipmentID,OrderID,Carrier,TrackingNumber,ShippedDate,DeliveredDate")] Shipment shipment)
        {
            if (id != shipment.ShipmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.ShipmentID))
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
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "Status", shipment.OrderID);
            return View(shipment);
        }

        // GET: Admin/Shipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.Order)
                .FirstOrDefaultAsync(m => m.ShipmentID == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // POST: Admin/Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment != null)
            {
                _context.Shipments.Remove(shipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipments.Any(e => e.ShipmentID == id);
        }
    }
}
