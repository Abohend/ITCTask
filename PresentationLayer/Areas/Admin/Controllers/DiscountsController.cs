using EntityLayer.Models;
using EntityLayer.Repositories;
using EntityLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Utilities;

namespace PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "admin")]
    public class DiscountsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var Discounts = _unitOfWork.Discount.GetAll(null, "Product");
            return View(Discounts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var products = _unitOfWork.Product.GetAll(p => p.Discount == null, "Discount")
                .Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                });
            var DiscountVM = new DiscountVm()
            {
                Products = products
            };
            return View(DiscountVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(DiscountVm DiscountVm)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Discount.Add(DiscountVm.Discount);
                _unitOfWork.Complete();
                TempData["toast"] = "Discount has been created sucessfully";
                TempData["toastType"] = "success";
                return RedirectToAction("Index");
            }
            DiscountVm.Products = _unitOfWork.Product.GetAll()
                .Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                });
            return View(DiscountVm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View(_unitOfWork.Discount.GetOne(c => c.Id == id));
        }

        [ValidateAntiForgeryToken, HttpPost]
        public IActionResult Edit(Discount Discount)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Discount.Update(Discount);
                _unitOfWork.Complete();
                TempData["toast"] = "Discount has been updated sucessfully";
                TempData["toastType"] = "info";
                return RedirectToAction("Index");
            }
            return View(Discount);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_unitOfWork.Discount.GetOne(c => c.Id == id));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Discount Discount)
        {
            _unitOfWork.Discount.Remove(Discount);
            _unitOfWork.Complete();
            TempData["toast"] = "Discount has been deleted sucessfully";
            TempData["toastType"] = "danger";
            return RedirectToAction("Index");
        }
    }
}
