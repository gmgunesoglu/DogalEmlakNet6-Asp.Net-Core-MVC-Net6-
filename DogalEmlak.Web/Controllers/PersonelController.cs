using DogalEmlak.Web.Entities;
using DogalEmlak.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt.Extensions;
using System.Data;
using System.Security.Claims;

namespace DogalEmlak.Web.Controllers
{
    [Authorize(Roles = "Staff")]
    public class PersonelController : Controller
    {
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;

        public PersonelController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            this.databaseContext = databaseContext;
            this.configuration = configuration;
        }

        //(S) kendi menülerini görsün linkleri olsun ((S)=Sayfa gerekiyor)
        //yine menüler olur personelin yapabileceği işlemler için
        public IActionResult Index()
        {
            return View();
        }

        //(S) bir çalışan iş yapabilmesi için giriş yapması gerekir
        //ki erişim yetkisi olsun
        [AllowAnonymous]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
		[AllowAnonymous] //bunu bir daha unutma :)
		public IActionResult Giris(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {
                string userpass = model.Password.MD5();
                //migration ve update-database işlemlerinde kullanıcı klonlanabilir (gokhan ve serkan)
                //burada first or default klaması, sisteme girip silmeme olanak tanıyor
                User user = databaseContext.Users.FirstOrDefault(x => x.UserName == model.UserName && x.Password == userpass);
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                }
                else
                {
                    //kullanıcı ok
                    if (user.Locked == true)
                    {
                        ModelState.AddModelError("", "Kullanıcı bloke edilmiş!");
                    }
                    else
                    {
                        //yetkilendir (cookie yetkilendirme)
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("Id", user.Id.ToString()));
                        claims.Add(new Claim("FirstName", user.FirstName));
                        claims.Add(new Claim("LastName", user.LastName));
                        claims.Add(new Claim("UserName", user.UserName));
                        claims.Add(new Claim("Password", user.Password));
                        List<Role> roles = databaseContext.Roles.Where(x => x.UserId == user.Id).ToList();
                        List<string> authorities = new List<string>();
                        foreach (Role role in roles)
                        {
                            authorities.Add(role.Authority);
                            claims.Add(new Claim(ClaimTypes.Role, role.Authority));
                        }
                        ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync("Cookies", principal);

                        if (authorities.Contains("Admin"))
                        {
							return RedirectToAction("Index", "Admin");
						}

						return RedirectToAction("Index", "Personel");

					}
                }
            }
            return View();
        }

        //sayfası olmayacak çıkış yapıp tekrar login e dönsün
        //o zaman login e izin açmak gerekiyor hatta 
        //oturum gerekiyorsa direk oraya yönlendirilsin
        public IActionResult Cikis()
        {
            HttpContext.SignOutAsync("Cookies");
            return Redirect(nameof(Giris));
        }

        //(S) Kullanıcı kendi bilgilerini görebilsin
        public IActionResult KullaniciBilgileri()
        {
            Guid id = Guid.Parse(HttpContext.User.FindFirstValue("Id"));
            User user = databaseContext.Users.FirstOrDefault(x => x.Id == id);
            UserModel model = new UserModel(user);
            return View(model);
        }


		//(S) Kullanıcı kendi bilgilerini güncelleyebilsin
		//burdan geri tekrar kullanıcı bilgilerine dönsün
		public IActionResult KullaniciBilgileriniGuncelle()
        {
			Guid id = Guid.Parse(HttpContext.User.FindFirstValue("Id"));
            User user = databaseContext.Users.FirstOrDefault(x => x.Id == id);
            UserModel model = new UserModel(user);
            return View(model);
        }

        [HttpPost]
        public IActionResult KullaniciBilgileriniGuncelle(UserModel model)
        {
            //kullanıcı adı kullanılıyorsa...Ama o kullanıcı adı seninkisi ise devam etmeli!
            if (databaseContext.Users.Any(x => x.UserName == model.UserName && x.Id!=model.Id))
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı kullanılıyor!");
                return View(model);
            }
            User user = databaseContext.Users.FirstOrDefault(x => x.Id == model.Id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            if (model.Password != null)
            {
                user.Password = model.Password.MD5();
            }
            user.Locked = model.Locked;
            databaseContext.SaveChanges();


            return RedirectToAction(nameof(KullaniciBilgileri));
        }

        //(S) ilan eklesin
        //(M) -> PropertyModel
        public IActionResult IlanEkle()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> IlanEkle(AddPropertyModel model)
		{
			if (ModelState.IsValid)
			{
                //kontrol
                foreach (IFormFile file in model.Files)
                {
                    if (file == null || file.Length == 0)
                    {
                        return BadRequest("Please select an image file");
                    }

                    if (!file.ContentType.StartsWith("image/"))
                    {
                        return BadRequest("Only image files are allowed");
                    }
                }

                //ekle
                Property property = new Property(model);
                property.DateOfRenewal=DateTime.Now;
                property.DateOfAdded=DateTime.Now;
                databaseContext.Properties.Add(property);
                if (databaseContext.SaveChanges() == 0)
                {
                    ModelState.AddModelError("", "Mülk Eklenemedi!");
                }
                else
                {
					foreach (IFormFile file in model.Files)
					{
						var stream = new MemoryStream();
						await file.CopyToAsync(stream);
                        var propertyImage = new PropertyImage(property.Id, stream.ToArray(),file.FileName);
						databaseContext.PropertyImages.Add(propertyImage);
						
						stream.Dispose();
					}
                    await databaseContext.SaveChangesAsync();
					return Redirect(nameof(IlanlariYonet));
                }
			}
			return View();
        }

        //(S) İlanları yönetebileceği bir sayfa olsun
        //(M) -> PropertyModel ama liste
        // ilan silme/güncelleme
        public IActionResult IlanlariYonet()
        {
            List<PropertySummaryModel> models = new List<PropertySummaryModel>();
            List<Property> properties = databaseContext.Properties.ToList();
            foreach(Property property in properties)
            {
                models.Add(property.ToSummarty());
            }
            return View(models);
        }

		//public async Task<IActionResult> Index(List<IFormFile> files)
		//{
		//	// Dosyaları kaydedin ve PropertyImage nesneleri oluşturun

		//	// PropertyImage nesnelerini veritabanına kaydedin

		//	var propertyImages = await databaseContext.PropertyImages.ToListAsync();
		//	var propertyImageModels = propertyImages.Select(x => x.ToModel());
		//	return View(propertyImageModels);
		//}

		//(S) ilan güncellesin, ilanın güncellenme tarihi otamatik yenilensin
		//(M) -> PropertyModel güncelledikten sonra ilanlari yönete dönsün
		public IActionResult IlanGuncelle(Guid id)
        {
            Property property = databaseContext.Properties.FirstOrDefault(p => p.Id == id);
            PropertyModel propertyModel = property.ToModel();
            List<Byte[]> images = (List<Byte[]>)databaseContext.PropertyImages.Where(p => p.PropertyId == propertyModel.Id).Select(p=>p.ImageData).ToList();
			UpdatePropertyModel model = new UpdatePropertyModel(propertyModel,images);
			return View(model);
        }


        //Resim ekliyor ama silmiyor !!!SİLME EKLENECEK!!!
        [HttpPost]
		public async Task<IActionResult> IlanGuncelle(UpdatePropertyModel model)
		{   
            Property property = databaseContext.Properties.FirstOrDefault(p => p.Id == model.Id);
            property.SizeOfGross=model.SizeOfGross;
            property.SizeOfNet=model.SizeOfNet;
            property.Price=model.Price;
            property.Address=model.Address;
            property.Header=model.Header;
            property.DateOfRenewal=DateTime.Now;
            property.Rooms=model.Rooms;
            property.TypeOfProperty=model.TypeOfProperty;
            property.TypeOfUsage=model.TypeOfUsage;
			//aynı isimde resim eklenememeli
			List<String> currentImageNames = (List<String>)databaseContext.PropertyImages.Where(i => i.PropertyId == model.Id).Select(i => i.FileName).ToList();
			foreach (IFormFile file in model.Files)
			{
                if (currentImageNames.Contains(file.Name))
                {
					//hata ver geri dön
					return BadRequest("Bu dosya ismi ile zaten bir resim ekli: "+file.Name);
				}
			}
			foreach (IFormFile file in model.Files)
			{
				var stream = new MemoryStream();
				await file.CopyToAsync(stream);
				var propertyImage = new PropertyImage(property.Id, stream.ToArray(), file.FileName);
                databaseContext.PropertyImages.Add(propertyImage);
				stream.Dispose();
			}
            databaseContext.Properties.Update(property);
			await databaseContext.SaveChangesAsync();
            return RedirectToAction(nameof(IlanlariYonet));
		}




		//ilan silsin ilanlarıyönet e dönsün
		public IActionResult IlanSil(Guid id)
        {
            Property property=databaseContext.Properties.FirstOrDefault(x => x.Id==id);
            databaseContext.Properties.Remove(property);
            List<PropertyImage> propertyImages = (List<PropertyImage>)databaseContext.PropertyImages.Where(x => x.PropertyId == id).ToList();
            foreach(PropertyImage propertyImage in propertyImages)
            {
                databaseContext.PropertyImages.Remove(propertyImage);
            }
            databaseContext.SaveChanges();
            return RedirectToAction(nameof(IlanlariYonet));
        }



        //burası kopya

  //      [HttpPost]
  //      public async Task<IActionResult> Create(PropertyModel model)
  //      {
  //          foreach (IFormFile file in model.Files)
  //          {
  //              if (file == null || file.Length == 0)
  //              {
  //                  return BadRequest("Please select an image file");
  //              }

  //              if (!file.ContentType.StartsWith("image/"))
  //              {
  //                  return BadRequest("Only image files are allowed");
  //              }
  //          }
  //          foreach (IFormFile file in model.Files)
  //          {
  //              var stream = new MemoryStream();
  //              await file.CopyToAsync(stream);
  //              var propertyImage = new PropertyImage
  //              {
  //                  FileName = file.FileName,
  //                  ImageData = stream.ToArray()
  //              };
  //              propertyImage.PropertyId = model.Id;
  //              databaseContext.PropertyImages.Add(propertyImage);
  //              await databaseContext.SaveChangesAsync();
  //              stream.Dispose();
  //          }
  //          return RedirectToAction(nameof(Index));            
		//}

        //public async Task<IActionResult> Create(List<IFormFile> files)
        //{
        //    // Dosyaları kaydedin ve PropertyImage nesneleri oluşturun  

        //    // PropertyImage nesnelerini veritabanına kaydedin

        //    var propertyImages = await databaseContext.PropertyImages.ToListAsync();
        //    var propertyImageModels = propertyImages.Select(x => x.ToModel());
        //    return View("Create", propertyImageModels);
        //}

    }
}
