using InstitutionPanel.Filters;
using InstitutionPanel.Models.DTOs.Institution;
using InstitutionPanel.Models.ViewModels;
using InstitutionPanel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InstitutionPanel.Controllers
{
    public class InstitutionController : Controller
    {
        private readonly IInstitutionService _institutionService;

        public InstitutionController(IInstitutionService institutionService)
        {
            _institutionService = institutionService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _institutionService.GetAllInstitutionAsync();

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;
                return View(new List<InstitutionVM>());
            }

            return View(response.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddInstitutionCommand command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var response = await _institutionService.CreateInstitutionAsync(command);

            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return View(command);
            }

            TempData["SuccessMessage"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _institutionService.GetInstitutionByIdAsync(id);

            if (!response.Success || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var command = new UpdateInstitutionCommand
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                Email = response.Data.Email,
                Description = response.Data.Description,
                Address = response.Data.Address,
                ContactNo = response.Data.ContactNo,
                PhoneNo = response.Data.PhoneNo,
                IsActive = response.Data.IsActive
            };

            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateInstitutionCommand command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var response = await _institutionService.UpdateInstitutionAsync(command);

            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return View(command);
            }

            TempData["SuccessMessage"] = response.Message;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _institutionService.GetInstitutionByIdAsync(id);

            if (!response.Success || response.Data == null)
                return BadRequest(response.Message);

            return PartialView("_InstitutionDetails", response.Data);
        }

        //public async Task<IActionResult> Details(Guid id)
        //{
        //    var response = await _institutionService.GetInstitutionByIdAsync(id);

        //    if (!response.Success || response.Data == null)
        //    {
        //        TempData["ErrorMessage"] = response.Message;
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(response.Data);
        //}

        [CustomAuthorize(AllowedUserTypes = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _institutionService.DeleteInstitutionAsync(id);

            if (!response.Success)
                TempData["ErrorMessage"] = response.Message;
            else
                TempData["SuccessMessage"] = response.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
