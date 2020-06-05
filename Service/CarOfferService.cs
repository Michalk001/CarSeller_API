using API.Models;
using API.ServiceInterfaces;
using API.ViewModels;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace API.Service
{
    public class CarOfferService : ICarOfferService
    {

        private readonly DBContext _context;
        private IHostingEnvironment _env;

        public CarOfferService(IHostingEnvironment env,  DBContext context)
        {

            _env = env;
            _context = context;
        }
        [Authorize]
        public async Task<object> Save(CarOfferViewModel model, string token)
        {
            var offerID = Guid.NewGuid();
            var allTitles = await _context.CarOffers.Select(x => x.Title).ToListAsync();
            string title;
            string titleTmp = new string(model.ShortDescription.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
            string check;
            do
            {   title = titleTmp + ObjectId.GenerateNewId().ToString().Substring(0, 8);
                check = allTitles.FirstOrDefault(x => x.Contains(title));
            } while (check != null);

            List<File> files = new List<File>();
            foreach (var item in model.FileViewModels)
            {
                if (item.Type.Contains("image") && item.Base64.Length > 65)
                {
                    string hash = (item.Base64.Replace("data:" + item.Type + ";base64,", "").Substring(0, 60) + ObjectId.GenerateNewId().ToString().Substring(0, 8)).Replace("/","").Replace("-","") + "."+item.Type.Replace("image/", "");
                    var byteBuffer = Convert.FromBase64String(item.Base64.Replace("data:"+item.Type+";base64,",""));
                    var webRoot = _env.WebRootPath;
                    var filePath = System.IO.Path.Combine(webRoot, "images" );
                     filePath = System.IO.Path.Combine(filePath, hash);
                    System.IO.File.WriteAllBytes(filePath , byteBuffer);
                    files.Add(new File()
                    {
                        Id = Guid.NewGuid(),
                        Name = item.Name,
                        Hash = hash,
                        Path = "images\\",
                        Type = item.Type.Split('/').FirstOrDefault()
                    });
                }
            }
            List<CarOfferEquipment> equipments = new List<CarOfferEquipment>();
            var equipmentDB = await _context.Equipments.ToListAsync();
            foreach (var item in model.Equipment)
            {
                var eqId = equipmentDB.Where(x => x.IdString == item).Select(x => x.Id).FirstOrDefault();
                if (eqId != null)
                    equipments.Add(new CarOfferEquipment()
                    {
                        CarOfferId = offerID,
                        EquipmentId = eqId
                    });
                
            }


            var userNameToken = JwtDecode.User(token);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == userNameToken.ToUpper());

            CarOffer carOffer = new CarOffer() {
                Id = Guid.NewGuid(),
                CarModel = await _context.CarModels.Where(x => x.ModelName.ToUpper() == model.CarModel.ToUpper()).FirstOrDefaultAsync(),
                CarProducent = await _context.CarProducents.Where(x => x.ProducentName.ToUpper() == model.CarProducent.ToUpper()).FirstOrDefaultAsync(),
                Fuel = await _context.FuelTypes.Where(x => x.FuelName.ToUpper() == model.Fuel.ToUpper()).FirstOrDefaultAsync(),
                Description = model.Description,
                PhoneNumber = model.PhoneNumber,
                ShortDescription = model.ShortDescription,
                Equipments = equipments,
                Title = title,
                Year = model.Year,
                Price = model.Price,
                VinNumber = model.VinNumber,
                Files = files,
                Mileage = model.Mileage,
                AddDate = DateTime.Now,
                Condition = model.Condition,
                Country = model.Country,
                Door = model.Door,
                Seat = model.Seat,
                Color = model.Color,
                HoursePower = model.HoursePower,
                User = user
            };
            await _context.CarOffers.AddAsync(carOffer);
            await _context.SaveChangesAsync();
            return new
            {
                Succeeded = true

            };
        }

        public async Task <object> Get(string title)
        {
            try
            {

                var model = await _context.CarOffers.Where(x => x.Title.ToLower() == title.ToLower())
                .Where(x => x.Deleted == false)
                .Include(x => x.Equipments)
                .ThenInclude(x => x.Equipment)
                .Include(x => x.CarModel)
                .Include(x => x.CarProducent)
                .Include(x => x.Fuel)
                .Include(x => x.Files)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
                if (model == null)
                    return null;
                List<string> equipment = new List<string>();
                foreach (var item in model.Equipments)
                {

                    string tmp = item.Equipment.Name;
                    equipment.Add(tmp);
                }

                List<FileViewModel> fileViewModels = new List<FileViewModel>();
                foreach (var item in model.Files)
                {
                    fileViewModels.Add(new FileViewModel()
                    {
                        Name = item.Name,
                        Path = item.Path + item.Hash,
                        Type = item.Type
                    });
                }

                CarOfferViewModel carOffer = new CarOfferViewModel
                {
                    CarModel = model.CarModel.ModelName,
                    CarProducent = model.CarProducent.ProducentName,
                    Fuel = model.Fuel.FuelName,
                    Description = model.Description,
                    ShortDescription = model.ShortDescription,
                    Title = model.Title,
                    PhoneNumber = model.PhoneNumber,
                    Equipment = equipment,
                    Year = model.Year,
                    Price = model.Price,
                    VinNumber = model.VinNumber,
                    Mileage = model.Mileage,
                    Condition = model.Condition,
                    Country = model.Country,
                    Seat = model.Seat,
                    Door = model.Door,
                    Color = model.Color,
                    HoursePower = model.HoursePower,
                    FileViewModels = fileViewModels,
                    BusinessProfile = model.User.BusinessProfile.ToString(),
                    UserName = model.User.FirstName

                };
                return new
                {
                    Succeeded = true,
                    carOffer
                };
            }
            catch
            {
                return new
                {
                    Succeeded = false

                };
            }
        }


        public async Task<object> GetRaw(string title)
        {
            try
            {

                var model = await _context.CarOffers.Where(x => x.Title.ToLower() == title.ToLower())
                .Where(x => x.Deleted == false)
                .Include(x => x.Equipments)
                .ThenInclude(x => x.Equipment)
                .Include(x => x.CarModel)
                .Include(x => x.CarProducent)
                .Include(x => x.Fuel)
                .Include(x => x.Files)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
                if (model == null)
                    return null;
                List<string> equipment = new List<string>();
                foreach (var item in model.Equipments)
                {

                    string tmp = item.Equipment.IdString;
                    equipment.Add(tmp);
                }

                List<FileViewModel> fileViewModels = new List<FileViewModel>();
                foreach (var item in model.Files.Where(x => x.Deleted == false))
                {
                    fileViewModels.Add(new FileViewModel()
                    {
                        Name = item.Name,
                        Path = item.Path + item.Hash,
                        Type = item.Type
                    });
                }

                CarOfferViewModel carOffer = new CarOfferViewModel
                {
                    CarModel = model.CarModel.IdString,
                    CarProducent = model.CarProducent.IdString,
                    Fuel = model.Fuel.IdString,
                    Description = model.Description,
                    ShortDescription = model.ShortDescription,
                    Title = model.Title,
                    PhoneNumber = model.PhoneNumber,
                    Equipment = equipment,
                    Year = model.Year,
                    Price = model.Price,
                    VinNumber = model.VinNumber,
                    Mileage = model.Mileage,
                    Condition = model.Condition,
                    Country = model.Country,
                    Seat = model.Seat,
                    Door = model.Door,
                    Color = model.Color,
                    HoursePower = model.HoursePower,
                    FileViewModels = fileViewModels,

                };
                return new
                {
                    Succeeded = true,
                    carOffer
                };
            }
            catch
            {
                return new
                {
                    Succeeded = false

                };
            }
        }

        public async Task<List<CarOfferViewModel>> Get()
        {
            var models = await _context.CarOffers
                .Where(x => x.Deleted == false)
                .Include(x => x.Equipments)
                .ThenInclude(x => x.Equipment)
                .Include(x => x.CarModel)
                .Include(x => x.CarProducent)
                .Include(x => x.Fuel)
                .Include(x => x.Files)
                .Include(x => x.User)
                .ToListAsync();

            if (models == null)
                return null;
            
            List<CarOfferViewModel> carOfferList = new List<CarOfferViewModel>();
            foreach (var model in models)
            {

                List<string> equipment = null;
                equipment = model.Equipments.Select(x => x.Equipment.Name).ToList();
                CarOfferViewModel carOffer = new CarOfferViewModel
                {
                    CarModel = model.CarModel.ModelName,
                    CarProducent = model.CarProducent.ProducentName,
                    Fuel = model.Fuel.FuelName,
                    Description = model.Description,
                    ShortDescription = model.ShortDescription,
                    Title = model.Title,
                    PhoneNumber = model.PhoneNumber,
                    Equipment = equipment,
                    Year = model.Year,
                    Price = model.Price,
                    VinNumber = model.VinNumber,
                    Mileage = model.Mileage,
                    Condition = model.Condition,
                    Country = model.Country,
                    Seat = model.Seat,
                    Door = model.Door,
                    Color = model.Color,
                    HoursePower = model.HoursePower,
                    BusinessProfile = model.User.BusinessProfile.ToString()
                };
                carOfferList.Add(carOffer);
            }
            return carOfferList;
        }
        public async Task<IEnumerable<CarOfferAdverViewModel>> GetAdvertList()
        {
            List<CarOfferAdverViewModel> carOfferAdverVieModel = new List<CarOfferAdverViewModel>();
            var carOffers = await _context.CarOffers
                .Where(x => x.Deleted == false)
                .Include(x => x.Fuel)
                .Include(x => x.CarProducent)
                .Include(x => x.CarModel)
                .ToListAsync();


            foreach(var item in carOffers)
            {
                FileViewModel fileViewModel = null;
                var firstImage = item.Files.FirstOrDefault();
                if(firstImage != null)
                {
                    fileViewModel = new FileViewModel()
                    {
                        Name = firstImage.Name,
                        Path = firstImage.Path + firstImage.Hash,
                        Type = firstImage.Type
                    };
                }
                var carAdvert = new CarOfferAdverViewModel()
                {

                    ModelName = item.CarModel.ModelName,
                    ProducentName = item.CarProducent.ProducentName,
                    Fuel = item.Fuel.FuelName,
                    ShortDescription = item.ShortDescription,
                    Title = item.Title,
                    Year = item.Year,
                    Price = item.Price,
                    Mileage = item.Mileage,
                    FileViewModel = fileViewModel,
                    AddDate = item.AddDate
                };
                carOfferAdverVieModel.Add(carAdvert);
            }
            return carOfferAdverVieModel;
        }
        public async Task<List<string>> GetAllOnlyTitle()
        {
            return await _context.CarOffers.Select(x => x.Title).ToListAsync();
        }
        public async Task<object> Remove(string title, string token)
        {
            try
            {
                var user = JwtDecode.User(token);
                var model = await _context.CarOffers
                    .Where(x => x.User.NormalizedUserName == user.ToUpper())
                    .Where(x => x.Title == title.ToLower()).FirstOrDefaultAsync();
                if (model == null)
                    return false;
                model.Deleted = true;
                _context.CarOffers.Update(model);
                await _context.SaveChangesAsync();
                return new
                {
                    Succeeded = true

                };
            }
            catch
            {
                return new
                {
                    Succeeded = false

                };
            }
        }

        public async Task<object> Update(CarOfferViewModel model,string token)
        {

            string user = JwtDecode.User(token);

            var offer = await _context.CarOffers
                .Where(x => x.User.NormalizedUserName == user.ToUpper())
                .Where(x => x.Title == model.Title)
                .Include(x => x.Equipments)
                .ThenInclude(x => x.Equipment)
                .Include(x => x.CarModel)
                .Include(x => x.CarProducent)
                .Include(x => x.Fuel)
                .Include(x => x.Files)
                .FirstOrDefaultAsync();
            if (offer == null)
                return new
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        new {Code = "NotFound" }
                    }
                };

            offer.CarModel = await _context.CarModels.Where(x => x.IdString == model.CarModel).FirstOrDefaultAsync();
            offer.CarProducent = await _context.CarProducents.Where(x => x.IdString == model.CarProducent).FirstOrDefaultAsync();
            offer.Color = model.Color;
            offer.Condition = model.Condition;
            offer.Country = model.Country;
            offer.Description = model.Description;
            offer.Door = model.Door;
            offer.HoursePower = model.HoursePower;
            offer.Mileage = model.Mileage;
            offer.PhoneNumber = model.PhoneNumber;
            offer.Price = model.Price;
            offer.Seat = model.Seat;
            offer.ShortDescription = model.ShortDescription;
            offer.VinNumber = model.VinNumber;
            offer.Year = model.Year;

            List<CarOfferEquipment> equipments = new List<CarOfferEquipment>();
            var equipmentDB = await _context.Equipments.ToListAsync();
            foreach (var item in model.Equipment)
            {
                var eqId = equipmentDB.Where(x => x.IdString == item).Select(x => x.Id).FirstOrDefault();
                if (eqId != null)
                    equipments.Add(new CarOfferEquipment()
                    {
                        CarOfferId = offer.Id,
                        EquipmentId = eqId
                    });

            }
            List<File> files = new List<File>();
            foreach (var item in model.FileViewModels)
            {
                if (item.Base64 != null)
                {
                    if (item.Type.Contains("image") && item.Base64.Length > 65)
                    {
                        string hash = (item.Base64.Replace("data:" + item.Type + ";base64,", "").Substring(0, 60) + ObjectId.GenerateNewId().ToString().Substring(0, 8)).Replace("/", "").Replace("-", "").Replace("+", "") + "." + item.Type.Replace("image/", "");
                        var byteBuffer = Convert.FromBase64String(item.Base64.Replace("data:" + item.Type + ";base64,", ""));
                        var webRoot = _env.WebRootPath;
                        var filePath = System.IO.Path.Combine(webRoot, "images");
                        filePath = System.IO.Path.Combine(filePath, hash);
                        System.IO.File.WriteAllBytes(filePath, byteBuffer);
                        files.Add(new File()
                        {
                            Id = Guid.NewGuid(),
                            Name = item.Name,
                            Hash = hash,
                            Path = "images\\",
                            Type = item.Type.Split('/').FirstOrDefault()
                        });
                    }
                }
                else
                if (item.Type == "image" && (item.Base64 == "" || item.Base64 == null))
                {
                    var img = await _context.Files.Where(x => (x.Path+x.Hash) == item.Path).FirstOrDefaultAsync();
                    if (img != null)
                        files.Add(img);
                }
            }

            offer.Files = files;
            offer.Equipments = equipments;
            _context.Update(offer);
            await _context.SaveChangesAsync();

            return new
            {
                Succeeded = true
            };
        }
    }
}
