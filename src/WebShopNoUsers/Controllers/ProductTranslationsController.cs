using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShopNoUsers.Models;

namespace WebShopNoUsers.Controllers
{
    public class ProductTranslationsController : Controller
    {
        private readonly WebShopRepository _context;

        public ProductTranslationsController(WebShopRepository context)
        {
            _context = context;    
        }

        // GET: ProductTranslations
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductTranslations.ToListAsync());
        }

        // GET: ProductTranslations/Details/5
        public async Task<IActionResult> Details(int? id, string language)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTranslation = await _context.ProductTranslations.SingleOrDefaultAsync(m => m.ProductId == id && m.Language == language);
            if (productTranslation == null)
            {
                return NotFound();
            }

            return View(productTranslation);
        }

        // GET: ProductTranslations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductTranslations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Language,ProductDescription,ProductName,ProductPrice")] ProductTranslation productTranslation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productTranslation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productTranslation);
        }

        // GET: ProductTranslations/Edit/5/Language
        public async Task<IActionResult> Edit(int? id, string language)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTranslation = await _context.ProductTranslations.SingleOrDefaultAsync(m => m.ProductId == id && m.Language == language);
            if (productTranslation == null)
            {
                return NotFound();
            }
            return View(productTranslation);
        }

        // POST: ProductTranslations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Language,ProductDescription,ProductName,ProductPrice")] ProductTranslation productTranslation)
        {
            if (id != productTranslation.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productTranslation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTranslationExists(productTranslation.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(productTranslation);
        }

        // GET: ProductTranslations/Delete/5
        public async Task<IActionResult> Delete(int? id, string language)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTranslation = await _context.ProductTranslations.SingleOrDefaultAsync(m => m.ProductId == id && m.Language == language);
            if (productTranslation == null)
            {
                return NotFound();
            }

            return View(productTranslation);
        }

        // POST: ProductTranslations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string language)
        {
            var productTranslation = await _context.ProductTranslations.SingleOrDefaultAsync(m => m.ProductId == id && m.Language == language);
            _context.ProductTranslations.Remove(productTranslation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductTranslationExists(int id)
        {
            return _context.ProductTranslations.Any(e => e.ProductId == id);
        }
    }
}
