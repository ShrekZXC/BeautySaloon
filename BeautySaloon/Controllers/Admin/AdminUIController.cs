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
        public async Task<IActionResult> EditMain(MainSettingsEntity model, 
            IFormFile? HeaderBackgroundImageFile,
            IFormFile? MainBackgroundImageFile,
            IFormFile? FooterBackgroundImageFile,
            string? CurrentHeaderBackgroundImage,
            string? CurrentMainBackgroundImage,
            string? CurrentBackgroundImageFooter)
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
                else
                {
                    model.BackgroundImageHeader = CurrentHeaderBackgroundImage;
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
                else
                {
                    model.MainBackgroundImage = CurrentMainBackgroundImage;
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
                else
                {
                    model.BackgroundImageFooter = CurrentBackgroundImageFooter;
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
    }
}
