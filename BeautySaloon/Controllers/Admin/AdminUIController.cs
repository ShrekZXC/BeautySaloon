using BeautySaloon.DAL.Entity;
using BeautySaloon.DAL.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace BeautySaloon.Controllers.Admin
{
    public class AdminUiController : Controller
    {
        private readonly IDbRepository _dbRepository;

        public AdminUiController(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        [HttpGet]
        public async Task<IActionResult> EditHeader()
        {
            var headerSettings = await _dbRepository.Get<HeaderSettingsEntity>().FirstOrDefaultAsync();
            return View("~/Views/Admin/UI/EditHeader.cshtml", headerSettings ?? new HeaderSettingsEntity());
        }

        [HttpPost]
        public async Task<IActionResult> EditHeader(HeaderSettingsEntity model, IFormFile BackgroundImageFile)
        {
            if (ModelState.IsValid)
            {
                if (BackgroundImageFile != null && BackgroundImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(BackgroundImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await BackgroundImageFile.CopyToAsync(stream);
                    }

                    model.BackgroundImage = $"/images/{fileName}";
                }

                var headerSettings = await _dbRepository.Get<HeaderSettingsEntity>().FirstOrDefaultAsync();
                if (headerSettings == null)
                {
                    await _dbRepository.Add(model);
                }
                else
                {
                    headerSettings.SiteName = model.SiteName;
                    headerSettings.ColorHeader = model.ColorHeader;
                    headerSettings.BackgroundImage = model.BackgroundImage;
                    await _dbRepository.Update(headerSettings);
                }

                await _dbRepository.SaveChangesAsync();
                return RedirectToAction("EditHeader");
            }

            return View("~/Views/Admin/UI/EditHeader.cshtml", model);
        }

[HttpGet]
        public async Task<IActionResult> EditFooter()
        {
            var footerSettings = await _dbRepository.Get<FooterSettingsEntity>().FirstOrDefaultAsync();
            return View("~/Views/Admin/UI/EditFooter.cshtml", footerSettings ?? new FooterSettingsEntity());
        }

        [HttpPost]
        public async Task<IActionResult> EditFooter(FooterSettingsEntity model, IFormFile BackgroundImageFile)
        {
            if (ModelState.IsValid)
            {
                if (BackgroundImageFile != null && BackgroundImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(BackgroundImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await BackgroundImageFile.CopyToAsync(stream);
                    }

                    model.BackgroundImage = $"/images/{fileName}";
                }

                var footerSettings = await _dbRepository.Get<FooterSettingsEntity>().FirstOrDefaultAsync();
                if (footerSettings == null)
                {
                   await  _dbRepository.Add(model);
                }
                else
                {
                    footerSettings.SocialMediaName = model.SocialMediaName;
                    footerSettings.SocialMediaLink = model.SocialMediaLink;
                    footerSettings.FooterColor = model.FooterColor;
                    footerSettings.WorkingHours = model.WorkingHours;
                    footerSettings.PhoneNumber = model.PhoneNumber;
                    footerSettings.Email = model.Email;
                    footerSettings.BackgroundImage = model.BackgroundImage;
                    await _dbRepository.Update(footerSettings);
                }

                await _dbRepository.SaveChangesAsync();
                return RedirectToAction("EditFooter");
            }

            return View("~/Views/Admin/UI/EditFooter.cshtml", model);
        }

        [HttpGet]
        public IActionResult EditBody()
        {
            return View("~/Views/Admin/UI/EditBody.cshtml");
        }
    }
}
