using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreshMart.Models;

namespace FreshMart.Controllers
{
    public class AdmItemsController : Controller
    {
        private readonly DbFreshMartContext _context;

        public AdmItemsController(DbFreshMartContext context)
        {
            _context = context;
        }

        // GET: AdmItems
        public async Task<IActionResult> Index()
        {
            var dbFreshMartContext = _context.Items.Include(i => i.CategoryF);
            return View(await dbFreshMartContext.ToListAsync());
        }

        // GET: AdmItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.CategoryF)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: AdmItems/Create
        public IActionResult Create()
        {
            ViewData["CategoryFid"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: AdmItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item, IFormFile pic)
        {
            string FileName = Path.GetFileName(pic.FileName);
            string Ext = Path.GetExtension(pic.FileName).ToLower();
            if (Ext == ".jpg" || Ext == ".png" || Ext == ".bmp" || Ext == ".jpeg" || Ext == ".tiff" || Ext == ".tif")
            {
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\DataFiles", FileName);
                using (var fs = new FileStream(FilePath, FileMode.Create))
                {
                    await pic.CopyToAsync(fs);
                    item.Image = FileName;
                }

            }
            else
            {
                TempData["Title"] = "Error";
                TempData["Message"] = "Please Select a valid image file (jpg,png,...)";
                TempData["Icon"] = "error";
                return View(item);
            }
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryFid"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", item.CategoryFid);
            return View(item);
        }

        // GET: AdmItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoryFid"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", item.CategoryFid);
            return View(item);
        }

        // POST: AdmItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item item, IFormFile pic)
        {
            if (pic != null)
            {
                string FileName = Path.GetFileName(pic.FileName);
                string Ext = Path.GetExtension(pic.FileName).ToLower();
                if (Ext == ".jpg" || Ext == ".png" || Ext == ".bmp" || Ext == ".jpeg" || Ext == ".tiff" || Ext == ".tif")
                {
                    string FilePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\DataFiles", FileName);
                    using (var fs = new FileStream(FilePath, FileMode.Create))
                    {
                        await pic.CopyToAsync(fs);
                        item.Image = FileName;
                    }

                }
                else
                {
                    TempData["Title"] = "Error";
                    TempData["Message"] = "Please Select a valid image file (jpg,png,...)";
                    TempData["Icon"] = "error";
                    return View(item);
                }
            }
            if (id != item.ItemId)
            {
                return NotFound();
            }


            try
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(item.ItemId))
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

        // GET: AdmItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.CategoryF)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: AdmItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}
