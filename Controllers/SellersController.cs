using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {

        private readonly SellerService _sellersService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellersService, DepartmentService departmentService)
        {
            _sellersService = sellersService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellersService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departaments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            _sellersService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var seller = _sellersService.FindById(id.Value);
                if (seller != null)
                {
                    return View(seller);
                }
                else
                {
                    return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
                }
            }
            else
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" }); 
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellersService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id != null)
            {
                var seller = _sellersService.FindById(id.Value);
                if (seller != null)
                {
                    return View(seller);
                }
                else
                {
                    return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
                }
            }
            else
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" }); 
            }
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var seller = _sellersService.FindById(id.Value);
                if (seller != null)
                {
                    List<Department> departments = _departmentService.FindAll();
                    SellerFormViewModel sellerFormViewModel = new SellerFormViewModel { Seller = seller, Departaments = departments };
                    return View(sellerFormViewModel);
                }
                else
                {
                    return RedirectToAction(nameof(Error), new { message = "Id not provided!" }); 
                }
            }
            else
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found!" }); 
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (int id, Seller seller)
        {
            if(id == seller.ID)
            {
                try
                {
                    _sellersService.Update(seller);
                    return RedirectToAction(nameof(Index));
                }
                catch (ApplicationException e)
                {

                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
             
            }
            else
            {
                return RedirectToAction(nameof(Error), new { message = "Id miss match!" });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
