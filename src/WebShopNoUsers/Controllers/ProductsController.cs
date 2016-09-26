using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShopNoUsers.Models;
using System.Globalization;
using WebShopNoUsers.ViewModels;
using WebShopNoUsers.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace WebShopNoUsers.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebShopRepository _context;
        private QueryFactory queryFactory;

        public ProductsController(WebShopRepository context)
        {
            _context = context;
            queryFactory = new QueryFactory( _context ); 
        }

        // GET: Products
        public async Task<IActionResult> Index(string id)
        {
            var query = queryFactory.GetAllProducts();

            if( !string.IsNullOrEmpty( id ) ) {                
                var search = query.Where( x => x.ProductName.Contains( id ) );
                return View( await search.ToListAsync() );
            } else
                return View( await query.ToListAsync() );
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await queryFactory.GetProduct( id ).SingleOrDefaultAsync();
            
            if (product == null)
            {
                return NotFound();
            }        
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductDescription,ProductPrice,ProductCategoryId,ProductName")] ProductViewModel pvm)
        {
            var product = new Product();
            var pt = new ProductTranslation();

            pt.Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            pt.ProductDescription = pvm.ProductDescription;
            pt.ProductName = pvm.ProductName;
            pt.ProductPrice = pvm.ProductPrice;

            product.ProductCategoryId = pvm.ProductCategoryId;

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                pt.ProductId = product.ProductId;
                _context.Add( pt );
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName", product.ProductCategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await queryFactory.GetProduct( id ).SingleOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName", product.ProductCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind( "ProductId,ProductDescription,ProductPrice,ProductCategoryId,ProductName" )] ProductViewModel pvm )
        {
            if (id != pvm.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.SingleOrDefaultAsync( m => m.ProductId == id );
                    var pt = queryFactory.GetTranslation( id );

                    pt.ProductDescription = pvm.ProductDescription;
                    pt.ProductName = pvm.ProductName;
                    pt.ProductPrice = pvm.ProductPrice;

                    product.ProductCategoryId = pvm.ProductCategoryId;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _context.Update( pt );
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(pvm.ProductId))
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
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName", pvm.ProductCategoryId);
            return View(pvm);
        }

        //TODO fix add translation
        //Get: Products/AddTranslation/5
        public async Task<IActionResult> AddTranslation(int? id ) {
            if( id == null ) {
                return NotFound();
            }

            var product = await queryFactory.GetProduct( id ).SingleOrDefaultAsync();
            if( product == null ) {
                return NotFound();
            }
            ViewData[ "ProductCategoryId" ] = new SelectList( _context.ProductCategories, "ProductCategoryId", "ProductCategoryName", product.ProductCategoryId );
            return View( product );
        }

        //POST: Products/AddTranslation/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTranslation( int id, [Bind( "ProductId,ProductDescription,ProductPrice,ProductCategoryId,ProductName,Language" )] ProductViewModel pvm ) {
            if( id != pvm.ProductId ) {
                return NotFound();
            }

            if( ModelState.IsValid ) {
                try {
                    if(!TranslationExists(id, pvm.Language ) ) {
                        RedirectToAction( "Index" );
                    }
                    var pt = queryFactory.GetTranslation( id );

                    pt.ProductDescription = pvm.ProductDescription;
                    pt.ProductName = pvm.ProductName;
                    pt.ProductPrice = pvm.ProductPrice;
                    pt.Language = pvm.Language;
                    
                    _context.Add( pt );
                    await _context.SaveChangesAsync();
                } catch( DbUpdateConcurrencyException ) {
                    if( !ProductExists( pvm.ProductId ) ) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction( "Index" );
            }
            ViewData[ "ProductCategoryId" ] = new SelectList( _context.ProductCategories, "ProductCategoryId", "ProductCategoryName", pvm.ProductCategoryId );
            return View( pvm );
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await queryFactory.GetProduct( id ).SingleOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);
            var pt = await _context.ProductTranslations.SingleOrDefaultAsync( m => m.ProductId == product.ProductId );
            _context.Products.Remove(product);
            _context.ProductTranslations.Remove( pt );
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        private bool TranslationExists(int id, string language ) {
            return _context.ProductTranslations.Any( e => e.Language == language && e.ProductId == id );
        }
    }
}
