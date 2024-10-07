using EntityLayer.Models;
using EntityLayer.Repositories;
using EntityLayer.ViewModels;
using PresentationLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utilities;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileService _fileService;

        public ProductsController(IUnitOfWork unitOfWork
            , IWebHostEnvironment webHostEnvironment
            , FileService fileService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            var products = _unitOfWork.Product.GetAll();
            return Json(new { data = products });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    product.ImagePath = _fileService.UploadFile("products", file);
                }
                _unitOfWork.Product.Add(product);
                _unitOfWork.Complete();
                TempData["toast"] = "Product has been created sucessfully";
                TempData["toastType"] = "success";
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _unitOfWork.Product.GetOne(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            
            return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    _fileService.DeleteFile(product.ImagePath);
                    product.ImagePath = _fileService.UploadFile("products", file);
                }
                _unitOfWork.Product.Update(product);
                _unitOfWork.Complete();
                TempData["toast"] = "Product has been updated sucessfully";
                TempData["toastType"] = "info";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.Product.GetOne(p => p.Id == id);
            if (product == null)
            {
                return Json(new { success = false });
            }
            _fileService.DeleteFile(product.ImagePath);
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Complete();
            return Json(new {success = true});
        }
    }
}
