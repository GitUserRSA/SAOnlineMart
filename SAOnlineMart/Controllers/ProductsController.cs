using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SAOnlineMart.Data;
using SAOnlineMart.Models;
using SAOnlineMart.Services.Interface;

namespace SAOnlineMart.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IFileService _fileService;
        private readonly IProductRepoService _productRepoService;

        public ProductsController(AppDbContext context, IFileService fileService, IProductRepoService productRepoService)
        {
            _context = context;
            _fileService = fileService;
            _productRepoService = productRepoService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productsModel == null)
            {
                return NotFound();
            }

            return View(productsModel);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm][Bind("Id,ProductName,PriceIncents,ProductImage,ItemDescription,IsAvailableToBuy,CreateAt,UpdateAt")] ProductsModel productsModel, IFormFile ProductImage)
        {
            if (ProductImage != null) //If the image is not null then save it
            {
                var fileResult = _fileService.SaveImage(ProductImage);

                if (fileResult.Item1 == 1) // if the file result returns true then set the product image
                {
                    productsModel.ProductImage = fileResult.Item2; //Get name of image
                }
            }
            var productResult = _productRepoService.Add(productsModel); //Return weather the product has been added or not

            if (productResult) //Debugging block
            {
                Console.WriteLine("Added successfully");
            }
            else
            {
                Console.WriteLine("Add Failed");
            }
            if (ModelState.IsValid) //If the submitted values are all filled in 
            {

                productsModel.Id = Guid.NewGuid(); //Generate a product ID
                productsModel.CreateAt = DateTime.Now; //Set the created at date

                _context.Add(productsModel); //Add to database
                await _context.SaveChangesAsync(); //Save those changes

            }
            return RedirectToAction(nameof(Index)); //Return the user back to the product management page


            // return View(productsModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await _context.Products.FindAsync(id);
            if (productsModel == null)
            {
                return NotFound();
            }
            return View(productsModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProductName,PriceIncents,ProductImage,ItemDescription,IsAvailableToBuy,CreateAt,UpdateAt")] ProductsModel productsModel, IFormFile ProductImage)
        {
            if (id != productsModel.Id)
            {
                return NotFound();
            }

            //Model state is returning false for these attributes, remove them from the model state check
            ModelState.Remove("ImageFile");
            ModelState.Remove("ProductImage");

            if (ModelState.IsValid) //If the required values are all present
            {
                if (ProductImage == null) //In the event the product image does not need to be changed
                {
                    var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

                    productsModel.ProductImage = existingProduct.ProductImage; //Set the product image to what we have saved already
                }
                else //In the even a new image has been chosen
                {
                    var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id); //Grab our existing product

                    _fileService.DeleteImage(existingProduct.ProductImage); //Delete the image file

                    var fileResult = _fileService.SaveImage(ProductImage); //Save the new image

                    if (fileResult.Item1 == 1)
                    {
                        productsModel.ProductImage = fileResult.Item2; //Get name of image
                    }
                    var productResult = _productRepoService.Add(productsModel); //Add the edited product back
                }

                try
                {
                    var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id); //Get our existing product

                    
                    productsModel.CreateAt = existingProduct.CreateAt; //Set created at value to the existing products value
                    
                    productsModel.UpdateAt = DateTime.Now;

                    _context.Update(productsModel);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsModelExists(productsModel.Id))
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
            return View(productsModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsModel = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id); //Get product ID
            if (productsModel == null)
            {
                return NotFound();
            }

            return View(productsModel);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productsModel = await _context.Products.FindAsync(id); //Find product id
            
            if (productsModel != null) //If it exists
            {
                var fileResult = _fileService.DeleteImage(productsModel.ProductImage);//Delete the image file

                _context.Products.Remove(productsModel); //Remove from database
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProductsModelExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
