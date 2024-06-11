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
        public async Task<IActionResult> EditMain()
        {
            var headerSettings = await _dbRepository.Get<MainSettingsEntity>().FirstOrDefaultAsync();
            return View("~/Views/Admin/UI/EditMain.cshtml", headerSettings ?? new MainSettingsEntity());
        }

        [HttpPost]
        public async Task<IActionResult> EditMain(MainSettingsEntity model, IFormFile? HeaderBackgroundImageFile,
            IFormFile? MainBackgroundImageFile, IFormFile? FooterBackgroundImageFile)
        {
            if (ModelState.IsValid)
            {
                if (HeaderBackgroundImageFile != null && HeaderBackgroundImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(HeaderBackgroundImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HeaderBackgroundImageFile.CopyToAsync(stream);
                    }

                    model.BackgroundImageHeader = $"/images/{fileName}";
                }

                if (MainBackgroundImageFile != null && MainBackgroundImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(MainBackgroundImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await MainBackgroundImageFile.CopyToAsync(stream);
                    }

                    model.MainBackgroundImage = $"/images/{fileName}";
                }

                if (FooterBackgroundImageFile != null && FooterBackgroundImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(FooterBackgroundImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FooterBackgroundImageFile.CopyToAsync(stream);
                    }

                    model.BackgroundImageFooter = $"/images/{fileName}";
                }

                var settings = await _dbRepository.Get<MainSettingsEntity>().FirstOrDefaultAsync();
                if (settings == null)
                {
                    await _dbRepository.Add(model);
                }
                else
                {
                    settings.SiteName = model.SiteName;
                    settings.ColorBackgroundHeader = model.ColorBackgroundHeader;
                    settings.ColorTextHeader = model.ColorTextHeader;
                    settings.BackgroundImageHeader = model.BackgroundImageHeader;
                    settings.MainText = model.MainText;
                    settings.ColorMainText = model.ColorMainText;
                    settings.MainBackgroundImage = model.MainBackgroundImage;
                    settings.ColorFooterText = model.ColorFooterText;
                    settings.BackgroundImageFooter = model.BackgroundImageFooter;
                    settings.ColorBackgroundMain = model.ColorBackgroundMain;
                    settings.ColorBackgroundFooter = model.ColorBackgroundFooter;
                    await _dbRepository.Update(settings);
                }

                await _dbRepository.SaveChangesAsync();
                return RedirectToAction("EditMain");
            }

            return View("~/Views/Admin/UI/EditMain.cshtml", model);
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
