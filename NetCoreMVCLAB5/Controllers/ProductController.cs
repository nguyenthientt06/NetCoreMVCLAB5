using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetCoreMVCLAB5.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVCLAB5.Controllers
{
    public class ProductController : Controller
    {
        // Giả lập danh sách Category và Product tạm thời để test
        private static List<Category> _categories = new List<Category>
        {
            new Category { Id = 1, Name = "Điện thoại" },
            new Category { Id = 2, Name = "Laptop" },
            new Category { Id = 3, Name = "Phụ kiện" }
        };

        private static List<Product> _products = new List<Product>();

        // 1. INDEX: Hiển thị danh sách sản phẩm
        public ActionResult Index()
        {
            return View(_products);
        }

        // 2. DETAILS: Xem chi tiết sản phẩm
        public ActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // 3. CREATE (GET): Hiển thị form thêm mới
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categories, "Id", "Name");
            return View();
        }

        // 4. CREATE (POST): Xử lý lưu sản phẩm mới và upload ảnh
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                product.Image = "/products/" + uniqueFileName;
            }
            else
            {
                ModelState.AddModelError("Image", "Vui lòng chọn ảnh sản phẩm");
            }

            if (ModelState.IsValid)
            {
                product.Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;
                _products.Add(product);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // 5. EDIT (GET): Hiển thị form chỉnh sửa sản phẩm
        public ActionResult Edit(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // 6. EDIT (POST): Xử lý cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile imageFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "products");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                existingProduct.Image = "/products/" + uniqueFileName;
            }

            if (ModelState.IsValid)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.SalePrice = product.SalePrice;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;

                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // 7. DELETE (GET): Xác nhận xóa sản phẩm
        public ActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // 8. DELETE (POST): Thực hiện xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}