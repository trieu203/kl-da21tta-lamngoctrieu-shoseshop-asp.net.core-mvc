﻿using khoaLuan_webGiay.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using khoaLuan_webGiay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList.Extensions;

namespace khoaLuan_webGiay.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/home")]
    public class HomeAdminController : Controller
    {
        private readonly KhoaLuanContext _context;
        private readonly ILogger<HomeAdminController> _logger;

        public HomeAdminController(KhoaLuanContext context, ILogger<HomeAdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/Home/Index
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Truy cập trang chính Admin.");
            return View();
        }

        // GET: Admin/Home/Category
        [Route("category")]
        public async Task<IActionResult> Category()
        {
            _logger.LogInformation("Truy cập danh mục sản phẩm.");
            try
            {
                var categories = await _context.Categories.ToListAsync();
                _logger.LogInformation("Lấy danh sách danh mục thành công. Số lượng: {Count}", categories.Count);
                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách danh mục.");
                return StatusCode(500, "Đã xảy ra lỗi, vui lòng thử lại sau.");
            }

        }

        //Create Category
        [Route("CreateCategory")]
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }

        [Route("CreateCategory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category, IFormFile? ImageFile)
        {
            // Kiểm tra xem các trường bắt buộc đã được nhập hay chưa
            if (string.IsNullOrEmpty(category.CategoryName) || string.IsNullOrEmpty(category.Description))
            {
                ModelState.AddModelError("", "Tên danh mục và Mô tả không được để trống.");
            }

            // Kiểm tra ModelState
            if (ModelState.IsValid)
            {
                // Xử lý upload hình ảnh
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Sinh chuỗi ngẫu nhiên 6 ký tự
                    var randomString = Path.GetRandomFileName().Replace(".", "").Substring(0, 6);

                    // Lấy đuôi file (ví dụ .jpg)
                    var fileExtension = Path.GetExtension(ImageFile.FileName);

                    // Tạo tên file mới (đảm bảo duy nhất nhưng ngắn gọn)
                    var fileName = $"{randomString}{fileExtension}";

                    // Đường dẫn lưu ảnh
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "categories");
                    var filePath = Path.Combine(uploadPath, fileName);

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Lưu hình ảnh vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Gán tên file vào thuộc tính ImageUrl (chỉ lưu tên file, không có đường dẫn)
                    category.ImageUrl = fileName;
                }
                else
                {
                    // Nếu không chọn hình, gán hình ảnh mặc định
                    category.ImageUrl = "new.jpg";
                }

                // Thêm CreatedDate mặc định là ngày hiện tại
                category.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

                // Lưu vào cơ sở dữ liệu
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return RedirectToAction("Category");
            }

            return View(category);
        }


        //Edit Category
        [Route("EditCategory")]
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            // Truy vấn danh mục cũ từ CSDL
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [Route("EditCategory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, Category updatedCategory, IFormFile? ImageFile)
        {
            var existingCategory = await _context.Categories.FindAsync(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            // Kiểm tra giá trị nhập vào, nếu rỗng thì giữ lại giá trị cũ
            existingCategory.CategoryName = string.IsNullOrEmpty(updatedCategory.CategoryName)
                ? existingCategory.CategoryName
                : updatedCategory.CategoryName;

            existingCategory.Description = string.IsNullOrEmpty(updatedCategory.Description)
                ? existingCategory.Description
                : updatedCategory.Description;

            // Xử lý hình ảnh
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Sinh chuỗi ngẫu nhiên 6 ký tự
                var randomString = Path.GetRandomFileName().Replace(".", "").Substring(0, 6);

                // Lấy đuôi file (ví dụ .jpg)
                var fileExtension = Path.GetExtension(ImageFile.FileName);

                // Tạo tên file mới (ngẫu nhiên 6 ký tự + đuôi file)
                var fileName = $"{randomString}{fileExtension}";

                // Đường dẫn lưu file
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "categories");
                var filePath = Path.Combine(uploadPath, fileName);

                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Lưu file ảnh mới
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Xóa hình ảnh cũ nếu tồn tại và không phải ảnh mặc định
                if (!string.IsNullOrEmpty(existingCategory.ImageUrl) && existingCategory.ImageUrl != "new.jpg")
                {
                    var oldFilePath = Path.Combine(uploadPath, existingCategory.ImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Gán tên file mới
                existingCategory.ImageUrl = fileName;
            }

            // Lưu cập nhật vào CSDL
            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Category");
        }


        //Delete Category
        [Route("DeleteCategory")]
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // Lấy danh mục từ CSDL
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                TempData["Message"] = "Danh mục không tồn tại.";
                return RedirectToAction("Category");
            }

            return View(category);
        }

        [Route("DeleteCategory")]
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Truy vấn danh mục từ CSDL
                var category = await _context.Categories
                                             .Include(c => c.Products) // Load các sản phẩm liên quan
                                             .FirstOrDefaultAsync(c => c.CategoryId == id);

                if (category == null)
                {
                    TempData["Message"] = "Danh mục không tồn tại.";
                    return RedirectToAction("Category");
                }

                // Kiểm tra nếu danh mục có sản phẩm con
                if (category.Products != null && category.Products.Any())
                {
                    TempData["Message"] = "Không thể xóa danh mục vì vẫn còn sản phẩm liên quan.";
                    return RedirectToAction("Category");
                }

                // Xóa hình ảnh nếu tồn tại
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "categories", category.ImageUrl);

                    try
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                            _logger.LogInformation($"Đã xóa hình ảnh: {filePath}");
                        }
                        else
                        {
                            _logger.LogWarning($"Hình ảnh không tồn tại: {filePath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Lỗi khi xóa hình ảnh: {filePath}");
                        TempData["Message"] = "Danh mục đã được xóa, nhưng có lỗi khi xóa hình ảnh.";
                        return RedirectToAction("Category");
                    }
                }

                // Xóa danh mục khỏi CSDL
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Danh mục đã được xóa thành công.";
                return RedirectToAction("Category");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                _logger.LogError(ex, "Lỗi khi xóa danh mục");
                TempData["Message"] = "Có lỗi xảy ra khi xóa danh mục.";
                return RedirectToAction("Category");
            }
        }


        //Detail Categry
        [Route("DetailCategory")]
        [HttpGet]
        public async Task<IActionResult> DetailCategory(int id)
        {
            // Tìm danh mục theo ID
            var category = await _context.Categories
                                         .Include(c => c.Products) // Bao gồm sản phẩm liên quan
                                         .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                TempData["Message"] = "Danh mục không tồn tại.";
                return RedirectToAction("Category");
            }

            return View(category);
        }


        //Product
        [Route("product")]
        public IActionResult Product(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var lstProduct = _context.Products
                                     .Include(p => p.Category)
                                     .OrderBy(p => p.ProductName)
                                     .ToPagedList(pageNumber, pageSize);

            return View(lstProduct);
        }

        //Detail Product
        [Route("DetailProduct")]
        [HttpGet]
        public async Task<IActionResult> DetailProduct(int id)
        {
            // Tìm sản phẩm theo ID
            var product = await _context.Products
                                        .Include(p => p.Category) // Bao gồm danh mục liên quan
                                        .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Product");
            }

            return View(product);
        }

        //Create Product
        [Route("CreateProduct")]
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.CategoryName = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        [Route("CreateProduct")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product, IFormFile? ImageFile)
        {
            // Kiểm tra xem các trường bắt buộc đã được nhập hay chưa
            if (string.IsNullOrEmpty(product.ProductName) || string.IsNullOrEmpty(product.Description))
            {
                ModelState.AddModelError("", "Tên sản phẩm và Mô tả không được để trống.");
            }

            // Kiểm tra giá trị Discount
            if (product.Discount > 100 || product.Discount < 0)
            {
                ModelState.AddModelError("Discount", "Giá giảm phải nằm trong khoảng từ 0 đến 100.");
            }

            // Kiểm tra ModelState
            if (ModelState.IsValid)
            {
                // Xử lý upload hình ảnh
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Sinh chuỗi ngẫu nhiên 6 ký tự
                    var randomString = Path.GetRandomFileName().Replace(".", "").Substring(0, 6);

                    // Lấy đuôi file (ví dụ .jpg, .png)
                    var fileExtension = Path.GetExtension(ImageFile.FileName);

                    // Tạo tên file mới (ngẫu nhiên 6 ký tự + đuôi file)
                    var fileName = $"{randomString}{fileExtension}";

                    // Đường dẫn lưu file
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "products");
                    var filePath = Path.Combine(uploadPath, fileName);

                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Lưu file ảnh vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = fileName; // Lưu tên file vào CSDL
                }
                else
                {
                    // Nếu không chọn hình, gán hình ảnh mặc định
                    product.ImageUrl = "default.jpg";
                }

                // Thêm CreatedDate mặc định là ngày hiện tại
                product.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

                // Lưu vào cơ sở dữ liệu
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction("Product");
            }

            // Cung cấp lại danh sách danh mục để hiển thị trong View
            ViewBag.CategoryName = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName");

            return View(product); // Trả lại View với dữ liệu người dùng đã nhập và danh mục
        }


        //Edit Product
        [Route("EditProduct")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            // Truy vấn sản phẩm từ CSDL
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryName = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(product);
        }

        [Route("EditProduct")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, Product updatedProduct, IFormFile? ImageFile)
        {
            var existingProduct = await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Kiểm tra giá trị Discount
            if (updatedProduct.Discount > 100 || updatedProduct.Discount < 0)
            {
                ModelState.AddModelError("Discount", "Giá giảm phải nằm trong khoảng từ 0 đến 100.");
            }

            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                return View(updatedProduct); // Trả lại view với thông báo lỗi
            }

            // Kiểm tra giá trị nhập vào, nếu rỗng thì giữ lại giá trị cũ
            existingProduct.ProductName = string.IsNullOrEmpty(updatedProduct.ProductName)
                ? existingProduct.ProductName
                : updatedProduct.ProductName;

            existingProduct.Description = string.IsNullOrEmpty(updatedProduct.Description)
                ? existingProduct.Description
                : updatedProduct.Description;

            existingProduct.Price = updatedProduct.Price > 0
                ? updatedProduct.Price
                : existingProduct.Price;

            // Cập nhật giá giảm
            existingProduct.Discount = updatedProduct.Discount;

            // Xử lý hình ảnh
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileExtension = Path.GetExtension(ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";

                // Đường dẫn lưu file
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "products");
                var filePath = Path.Combine(uploadPath, fileName);

                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Lưu file ảnh mới
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Xóa hình ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                {
                    var oldFilePath = Path.Combine(uploadPath, existingProduct.ImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Gán tên file mới
                existingProduct.ImageUrl = fileName;
            }

            // Cập nhật danh mục
            if (updatedProduct.CategoryId.HasValue && updatedProduct.CategoryId != existingProduct.CategoryId)
            {
                var newCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == updatedProduct.CategoryId.Value);
                if (newCategory != null)
                {
                    existingProduct.CategoryId = newCategory.CategoryId;
                    existingProduct.Category = newCategory;
                }
            }

            // Lưu cập nhật vào CSDL
            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Product");
        }

        // Delete Product
        [Route("DeleteProduct")]
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Tìm sản phẩm theo ID
            var product = await _context.Products
                                         .Include(p => p.Category) // Bao gồm thông tin danh mục
                                         .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                TempData["Message"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Product");
            }

            return View(product);
        }


        [Route("DeleteProduct")]
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            try
            {
                // Truy vấn sản phẩm từ CSDL
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductSizes) // Bao gồm thông tin kích thước
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    TempData["Message"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction("Product");
                }

                // Kiểm tra tổng số lượng sản phẩm
                int totalQuantity = product.ProductSizes?.Sum(ps => ps.Quantity) ?? 0;
                if (totalQuantity > 0)
                {
                    TempData["Message"] = "Không thể xóa sản phẩm vì vẫn còn số lượng trong kho.";
                    return RedirectToAction("Product");
                }

                // Xóa hình ảnh nếu tồn tại
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "products", product.ImageUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Xóa sản phẩm khỏi CSDL
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Sản phẩm đã được xóa thành công.";
                return RedirectToAction("Product");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                _logger.LogError(ex, "Lỗi khi xóa sản phẩm.");
                TempData["Message"] = "Có lỗi xảy ra khi xóa sản phẩm.";
                return RedirectToAction("Product");
            }
        }

        //ProductSize
        [Route("productsize")]
        public IActionResult ProductSize(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var lstProductSize = _context.ProductSizes
                                         .Include(ps => ps.Product)
                                         .OrderBy(ps => ps.ProductSizeId)
                                         .ToPagedList(pageNumber, pageSize);

            return View(lstProductSize);
        }

        // Create ProductSize
        [Route("CreateProductSize")]
        [HttpGet]
        public async Task<IActionResult> CreateProductSize()
        {
            ViewBag.ProductName = new SelectList(await _context.Products.ToListAsync(), "ProductId", "ProductName");
            return View();
        }

        [Route("CreateProductSize")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductSize(ProductSize productSize)
        {
            if (!ModelState.IsValid)
            {
                // Giữ lại dữ liệu khi load lại trang
                ViewBag.ProductName = new SelectList(await _context.Products.ToListAsync(), "ProductId", "ProductName", productSize.ProductId);
                return View(productSize);
            }

            _context.ProductSizes.Add(productSize);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Kích thước sản phẩm đã được tạo thành công.";
            return RedirectToAction("ProductSize");
        }


        // Edit ProductSize
        [Route("EditProductSize")]
        [HttpGet]
        public async Task<IActionResult> EditProductSize(int id)
        {
            try
            {
                // Tìm ProductSize theo ID
                var producSize = await _context.ProductSizes
                    .Include(ps => ps.Product) // Tải dữ liệu liên quan nếu cần
                    .FirstOrDefaultAsync(ps => ps.ProductSizeId == id);

                if (producSize == null)
                {
                    _logger.LogWarning("Không tìm thấy ProductSize với ID: {Id}", id);
                    return NotFound();
                }

                // Lấy danh sách sản phẩm
                var products = await _context.Products.ToListAsync();
                if (!products.Any())
                {
                    _logger.LogWarning("Danh sách sản phẩm trống. Không thể chỉnh sửa ProductSize.");
                    TempData["Message"] = "Không có sản phẩm nào trong hệ thống.";
                    return RedirectToAction("ProductSize");
                }

                // Tạo dropdown danh sách sản phẩm
                ViewBag.ProductName = new SelectList(products, "ProductId", "ProductName", producSize.ProductId);

                return View(producSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi truy xuất ProductSize với ID: {Id}", id);
                TempData["Message"] = "Đã xảy ra lỗi khi tải dữ liệu.";
                return RedirectToAction("ProductSize");
            }
        }

        [Route("EditProductSize")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductSize(int id, ProductSize updatedProductSize)
        {
            try
            {
                // Tìm ProductSize hiện tại trong cơ sở dữ liệu
                var existingProductSize = await _context.ProductSizes
                    .FirstOrDefaultAsync(ps => ps.ProductSizeId == id);

                if (existingProductSize == null)
                {
                    _logger.LogWarning("Không tìm thấy ProductSize với ID: {Id}", id);
                    TempData["Message"] = "Không tìm thấy kích thước sản phẩm.";
                    return RedirectToAction("ProductSize");
                }

                // Ghi log trạng thái trước khi cập nhật
                _logger.LogInformation("Trước cập nhật: {existingProductSize}", existingProductSize);

                // Cập nhật các thuộc tính từ `updatedProductSize`
                existingProductSize.ProductId = updatedProductSize.ProductId ?? existingProductSize.ProductId;
                existingProductSize.Size = string.IsNullOrEmpty(updatedProductSize.Size)
                    ? existingProductSize.Size
                    : updatedProductSize.Size;
                existingProductSize.Quantity = updatedProductSize.Quantity >= 0
                    ? updatedProductSize.Quantity
                    : existingProductSize.Quantity;
                existingProductSize.PriceAtTime = updatedProductSize.PriceAtTime > 0
                    ? updatedProductSize.PriceAtTime
                    : existingProductSize.PriceAtTime;

                // Ghi log trạng thái sau khi cập nhật
                _logger.LogInformation("Sau cập nhật: {existingProductSize}", existingProductSize);

                // Lưu các thay đổi vào cơ sở dữ liệu
                _context.ProductSizes.Update(existingProductSize); // Đánh dấu thực thể là đã thay đổi
                await _context.SaveChangesAsync(); // Lưu vào cơ sở dữ liệu

                TempData["Message"] = "Cập nhật kích thước sản phẩm thành công.";
                return RedirectToAction("ProductSize");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Lỗi khi lưu dữ liệu vào cơ sở dữ liệu.");
                TempData["Message"] = "Có lỗi xảy ra khi lưu dữ liệu. Vui lòng thử lại.";
                return RedirectToAction("EditProductSize", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi cập nhật ProductSize với ID: {Id}", id);
                TempData["Message"] = "Đã xảy ra lỗi khi chỉnh sửa kích thước sản phẩm. Vui lòng thử lại.";
                return RedirectToAction("EditProductSize", new { id });
            }
        }


        //Delete ProductSize
        [Route("DeleteProductSize")]
        [HttpGet]
        public async Task<IActionResult> DeleteProductSize(int id)
        {
            // Truy vấn kích thước sản phẩm từ CSDL
            var productSize = await _context.ProductSizes
                .Include(ps => ps.Product) // Bao gồm thông tin sản phẩm liên quan
                .FirstOrDefaultAsync(ps => ps.ProductSizeId == id);

            if (productSize == null)
            {
                TempData["Message"] = "Kích thước sản phẩm không tồn tại.";
                return RedirectToAction("ProductSize");
            }

            return View(productSize);
        }

        [Route("DeleteProductSize")]
        [HttpPost, ActionName("DeleteProductSize")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductSizeConfirmed(int id)
        {
            try
            {
                // Truy vấn kích thước sản phẩm từ CSDL
                var productSize = await _context.ProductSizes.FindAsync(id);

                if (productSize == null)
                {
                    TempData["Message"] = "Kích thước sản phẩm không tồn tại.";
                    return RedirectToAction("ProductSize");
                }

                // Xóa kích thước sản phẩm khỏi CSDL
                _context.ProductSizes.Remove(productSize);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Kích thước sản phẩm đã được xóa thành công.";
                return RedirectToAction("ProductSize");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                _logger.LogError(ex, "Lỗi khi xóa kích thước sản phẩm.");
                TempData["Message"] = "Có lỗi xảy ra khi xóa kích thước sản phẩm.";
                return RedirectToAction("ProductSize");
            }
        }


        // Detail ProductSize
        [Route("DetailProductSize")]
        [HttpGet]
        public async Task<IActionResult> DetailProductSize(int id)
        {
            var productSize = await _context.ProductSizes
                .Include(ps => ps.Product)
                .FirstOrDefaultAsync(ps => ps.ProductSizeId == id);

            if (productSize == null)
            {
                TempData["Message"] = "Kích thước sản phẩm không tồn tại.";
                return RedirectToAction("ProductSize");
            }

            return View(productSize);
        }

        //Index Review
        [Route("review")]
        public async Task<IActionResult> Review(int? page)
        {
            int pageSize = 10; // Số lượng bản ghi trên mỗi trang
            int pageNumber = page ?? 1; // Nếu `page` là null thì mặc định là trang 1

            try
            {
                // Lấy danh sách đánh giá với thông tin User và Product
                var reviews = await _context.Reviews
                    .Include(r => r.User) // Bao gồm thông tin người dùng
                    .Include(r => r.Product) // Bao gồm thông tin sản phẩm
                    .OrderByDescending(r => r.ReviewDate) // Sắp xếp theo ngày đánh giá
                    .Skip((pageNumber - 1) * pageSize) // Bỏ qua các bản ghi của trang trước
                    .Take(pageSize) // Lấy số lượng bản ghi theo kích thước trang
                    .ToListAsync();

                // Tính tổng số đánh giá
                var totalReviews = await _context.Reviews.CountAsync();

                // Gán thông tin phân trang cho ViewBag
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalReviews / pageSize);
                ViewBag.CurrentPage = pageNumber;

                return View(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đánh giá.");
                return StatusCode(500, "Đã xảy ra lỗi, vui lòng thử lại sau.");
            }
        }


    }
}
