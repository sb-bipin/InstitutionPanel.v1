using InstitutionPanel.Core.Interfaces;
using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.Entities;
using InstitutionPanel.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ImageGalleryController : Controller
{
    private readonly IImageGalleryService _service;
    private readonly ICurrentUserService _currentUser;

    public ImageGalleryController(IImageGalleryService service, ICurrentUserService currentUser)
    {
        _service = service;
        _currentUser = currentUser;
    }

    public async Task<IActionResult> Index()
    {
        if (!Guid.TryParse(_currentUser.InstitutionId, out Guid institutionId))
            return View(new List<ImageGalleryVM>());

        var response = await _service.GetAllAsync(institutionId);

        if (!response.Success)
            ViewBag.ErrorMessage = response.Message;

        return View(response.Data ?? new List<ImageGalleryVM>());
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Action = "Create";
        ViewBag.Title = "Add Image";

        return PartialView("_CreateEdit", new ImageGalleryVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ImageGalleryVM vm)
    {
        if (!ModelState.IsValid)
            return PartialView("_CreateEdit", vm);

        if (!Guid.TryParse(_currentUser.InstitutionId, out Guid institutionId))
            return Json(ApiResponse<bool>.FailureResponse("Invalid institution"));

        var response = await _service.CreateAsync(vm, institutionId);
        return Json(response);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ImageGalleryVM vm)
    {
        if (!ModelState.IsValid)
            return PartialView("_CreateEdit", vm);

        var response = await _service.UpdateAsync(vm);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _service.GetByIdAsync(id);
        if (!response.Success) return NotFound();
        return PartialView("_Details", response.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var response = await _service.DeleteAsync(id);
        return Json(response);
    }
}
