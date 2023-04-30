using DogalEmlak.Web.Entities;
using DogalEmlak.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DogalEmlak.Web.Controllers
{
    public class HomeController : Controller
    {
		private readonly DatabaseContext databaseContext;
		private readonly IConfiguration configuration;
		private readonly ILogger<HomeController> _logger;

		public HomeController(DatabaseContext databaseContext, IConfiguration configuration, ILogger<HomeController> logger)
		{
			this.databaseContext = databaseContext;
			this.configuration = configuration;
			_logger = logger;
		}

        public async Task<IActionResult> Index()
        {
            List<PropertySummaryModel> models = new List<PropertySummaryModel>();
            List<Property> properties = databaseContext.Properties.ToList();
            foreach (Property property in properties)
            {
                models.Add(property.ToSummarty());
            }
            return View(models);
        }

        public IActionResult Ilan(Guid id)
        {
            Property property = databaseContext.Properties.FirstOrDefault(p => p.Id == id);
            List<Byte[]> images = (List<Byte[]>)databaseContext.PropertyImages.Where(p => p.PropertyId == id).Select(p => p.ImageData).ToList();
            ShowPropertyModel model = new ShowPropertyModel(property.ToModel(), images);
            return View(model);
        }

		public IActionResult AccessDenied()
		{
			return View();
		}





		//[HttpPost]
		//public async Task<IActionResult> Index(IFormFile file)
		//{
		//    if (file == null || file.Length == 0)
		//    {
		//        return BadRequest("Please select an image file");
		//    }

		//    if (!file.ContentType.StartsWith("image/"))
		//    {
		//        return BadRequest("Only image files are allowed");
		//    }

		//    using (var stream = new MemoryStream())
		//    {
		//        await file.CopyToAsync(stream);

		//        var propertyImage = new PropertyImage
		//        {
		//            ImageData = stream.ToArray()
		//        };
		//        List<Property> properties = databaseContext.Properties.ToList();
		//        propertyImage.PropertyId = properties[0].Id;
		//        databaseContext.PropertyImages.Add(propertyImage);
		//        await databaseContext.SaveChangesAsync();

		//        return RedirectToAction("Create", "Personel");
		//    }
		//}



		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}