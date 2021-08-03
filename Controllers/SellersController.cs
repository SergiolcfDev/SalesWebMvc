using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

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

        public async Task<IActionResult> Index()
        {
            var list = await _sellersService.FindAllAsync();
            return  View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departaments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departaments = departments };
                return View(viewModel);
            }
            await _sellersService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var seller =await _sellersService.FindByIdAsync(id.Value);
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
        public async Task<IActionResult> Delete(int id)
        {
            await _sellersService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                var seller = await _sellersService.FindByIdAsync(id.Value);
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var seller = await _sellersService.FindByIdAsync(id.Value);
                if (seller != null)
                {
                    List<Department> departments = await _departmentService.FindAllAsync();
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
        public async Task<IActionResult> Edit (int id, Seller seller)
        {
            if(id == seller.ID)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        var departments = await _departmentService.FindAllAsync();
                        var viewModel = new SellerFormViewModel { Seller = seller, Departaments = departments };
                        return View(viewModel);
                    }
                    await _sellersService.UpdateAsync(seller);
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
