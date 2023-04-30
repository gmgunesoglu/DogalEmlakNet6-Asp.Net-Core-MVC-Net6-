using DogalEmlak.Web.Entities;
using DogalEmlak.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt.Extensions;
using System.Data;

namespace DogalEmlak.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
		private readonly DatabaseContext databaseContext;
		private readonly IConfiguration configuration;

		public AdminController(DatabaseContext databaseContext, IConfiguration configuration)
		{
			this.databaseContext = databaseContext;
			this.configuration = configuration;
		}


		//(S) Admin için menüler olsun ((S)=Sayfası var)
		public IActionResult Index()
        {
            return View();
        }


		//(S) Sadece Admin kullanıcısı user ekleyebilir.
		//Bu proje için personel ile user aynı şey, url de personel, içeride user
		//(M) -> AddUserModel
		public IActionResult PersonelEkle()
		{
			return View();
		}
			
		[HttpPost]
		public IActionResult PersonelEkle(CreateUserModel model)
		{
			//kullanıcı adı kullanılıyorsa...
			if (databaseContext.Users.Any(x => x.UserName == model.UserName))
			{
				ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı kullanılıyor!");
				return View(model);
			}
			if (ModelState.IsValid)
			{
				//ekle
				User user = new User(model.FirstName, model.LastName, model.UserName, model.Password.MD5());
				databaseContext.Users.Add(user);
				Role role = new Role(user.Id, "Staff");
				databaseContext.Roles.Add(role);
				if (databaseContext.SaveChanges() == 0)
				{
					ModelState.AddModelError("", "Kullanıcı Eklenemedi!");
				}
				else
				{
					//Normalde kişi kendisi sisteme üye olsa buradan login e
					//yönlendirmek mantıklı olur ama bu projede userları 
					//sadece admin ekleyebiliyor 
					return Redirect(nameof(Index));
				}
			}
			return View();
		}


		//(S) Tüm personelleri isteleyecek
        public IActionResult PersonelleriListele()
		{
			List<UserModel> userModels = new List<UserModel>();
			foreach(User user in databaseContext.Users)
            {
                List<Role> roles = databaseContext.Roles.Where(x => x.UserId == user.Id).ToList();
                userModels.Add(new UserModel(user));
			}
			return View(userModels);
		}

		//(S) Personeler silme-güncelleme-yetkilendirme-yetkialma sayfası
		//personelleri yönetme sayfasında da tüm personeller gözükecek ama
		//bu sayfada daha detaylı olsun
		public IActionResult PersonelleriYonet()
		{
			List<UserModel> userModels = new List<UserModel>();
			foreach (User user in databaseContext.Users)
			{
				List<Role> roles = databaseContext.Roles.Where(x => x.UserId == user.Id).ToList();
				userModels.Add(new UserModel(user));
			}
			return View(userModels);
		}

		//(S) bir user id gerekir buna dikkat et
        public IActionResult PersonelBilgileriniGuncelle(Guid Id)
        {
			User user= databaseContext.Users.FirstOrDefault(x => x.Id == Id);
            UserModel model = new UserModel(user);
            return View(model);
        }

		[HttpPost]
		public IActionResult PersonelBilgileriniGuncelle(UserModel model)
		{
            //kullanıcı adı kullanılıyorsa... Ama kendi kullanıcı adı olmamalı
            if (databaseContext.Users.Any(x => x.UserName == model.UserName && x.Id != model.Id))
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
			return RedirectToAction(nameof(PersonelleriYonet));
		}

		//yetki ekler
		[HttpPost]
        public IActionResult EmpowerUser(Guid Id)
        {
			List<Role> roles = databaseContext.Roles.Where(x => x.UserId == Id).ToList();
			if(roles.Count == 1) 
			{
				Role role = new Role(Id, "Manager");
				databaseContext.Roles.Add(role);
				databaseContext.SaveChanges();
			}
			else if(roles.Count == 2)
			{
				Role role = new Role(Id, "Admin");
				databaseContext.Roles.Add(role);
				databaseContext.SaveChanges();
			}
			return RedirectToAction(nameof(PersonelleriYonet));
        }

		//yetki çıkartır
        [HttpPost]
        public IActionResult DepowerUser(Guid Id)
        {
			List<Role> roles = databaseContext.Roles.Where(x => x.UserId == Id).ToList();
			if (roles.Count == 2)
			{
				Role role = databaseContext.Roles.FirstOrDefault(x => x.UserId == Id && x.Authority == "Manager");
				databaseContext.Roles.Remove(role);
				databaseContext.SaveChanges();
			}
			else if (roles.Count == 3)
			{
				Role role = databaseContext.Roles.FirstOrDefault(x => x.UserId == Id && x.Authority == "Admin");
				databaseContext.Roles.Remove(role);
				databaseContext.SaveChanges();
			}
			return RedirectToAction(nameof(PersonelleriYonet));
        }

		//kullanıcıyı siler yine user id lazım tabiki
        [HttpPost]
        public IActionResult DeleteUser(Guid Id)
        {
            User user = databaseContext.Users.SingleOrDefault(x => x.Id == Id);
            databaseContext.Users.Remove(user);
            if (databaseContext.SaveChanges() == 0)
            {
                ModelState.AddModelError("", "Personel Silinemedi!");
            }
            return RedirectToAction(nameof(PersonelleriYonet));
        }
    }
}
