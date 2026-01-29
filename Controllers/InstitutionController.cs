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

            if (response is null)
            {
                TempData["ErrorMessage"] = "Institution is not found.";
                return RedirectToAction(nameof(Index));
            }

            var command = new UpdateInstitutionCommand
            {
                Id = response.Id,
                Name = response.Name,
                Email = response.Email,
                Description = response.Description,
                Address = response.Address,
                ContactNo = response.ContactNo,
                PhoneNo = response.PhoneNo,
                IsActive = response.IsActive
            };

            return PartialView("_EditInstitution",command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateInstitutionCommand command)
        {
            if (!ModelState.IsValid)
                return PartialView("_EditInstitution", command);

            var response = await _institutionService.UpdateInstitutionAsync(command);

            if (!response.Success)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                return PartialView("_EditInstitution", command);
            }

            return Json(new { success = true, message = response.Message });
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _institutionService.GetInstitutionByIdAsync(id);

            if (response is null)
                return BadRequest(response);

            return PartialView("_InstitutionDetails", response);
        }

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
