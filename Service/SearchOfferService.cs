using API.Models;
using API.ServiceInterfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Service
{
    public class SearchOfferService : ISearchOfferService
    {

        private readonly DBContext _context;

        public SearchOfferService(DBContext context)
        {
            _context = context;
        }



        public async Task<object> GetByUser(string page, string token)
        {
            try
            {
                IQueryable<CarOffer> query = _context.CarOffers;
                int itemsOnPage = 2;
                if (!int.TryParse(page, out int pageNumber))
                    return null;
                var user = JwtDecode.User(token);
                if (string.IsNullOrEmpty(user))
                    return null;
                query = query.Where(x => x.User.NormalizedUserName == user.ToUpper()).Include(x => x.CarProducent)
                    .Where(x => x.Deleted == false)
                    .Include(x => x.CarModel)
                    .Include(x => x.User)
                    .Include(x => x.Fuel)
                    .Include(x => x.Files);

                var countOffers = await query.CountAsync();

                var offers = await query.OrderByDescending(x => x.AddDate)
                                         .Skip(itemsOnPage * (pageNumber - 1))
                                         .Take(itemsOnPage)
                                         .ToListAsync();


                var offerList = new List<CarOfferAdverViewModel>();

                foreach (var item in offers)
                {
                    var image = item.Files.FirstOrDefault();
                    var fileViewModel = new FileViewModel();
                    if (image != null)
                        fileViewModel = new FileViewModel()
                        {
                            Path = image.Path + image.Hash,
                            Name = image.Name
                        };
                    offerList.Add(new CarOfferAdverViewModel
                    {
                        ProducentName = item.CarProducent.ProducentName,
                        Fuel = item.Fuel.FuelName,
                        AddDate = item.AddDate,
                        Mileage = item.Mileage,
                        ModelName = item.CarModel.ModelName,
                        Price = item.Price,
                        Title = item.Title,
                        ShortDescription = item.ShortDescription,
                        Year = item.Year,
                        FileViewModel = fileViewModel
                    });
                }

                var pagination = new Pagination
                {
                    TotalItems = countOffers,
                    ItemsOnPage = itemsOnPage,
                    CurrentyPage = pageNumber
                };



                return new { offerList, pagination, Succeeded = true };
            }
            catch (Exception ex)
            {
                return new
                {
                    Succeeded = false
                };
                throw ex;
                
            }
            return null;
        }

        public async Task<object> Get(string page, string producer, string model, string fuel/*, string yearMin, string yearMax*/)
        {
            try
            {
                IQueryable<CarOffer> query = _context.CarOffers.Where(x => x.Deleted == false);
                int itemsOnPage = 2;
                if (!int.TryParse(page, out int pageNumber))
                    pageNumber = 1;
                //   var offerList = (await carOfferService.GetAdvertList());
                if (producer != null)
                    query = query.Where(x => x.CarProducent.IdString == producer);
                if (model != null)
                    query = query.Where(x => x.CarModel.IdString == model);
                if (fuel != null)
                    query = query.Where(x => x.Fuel.IdString == fuel);

               
                query = query.Include(x => x.CarProducent)
                                        .Include(x => x.CarModel)
                                        .Include(x => x.Fuel)
                                        .Include(x => x.Equipments)
                                        .Include(x => x.Files);
               var countOffers = await query.CountAsync();
               var offers = await       query.OrderByDescending(x => x.AddDate)
                                        .Skip(itemsOnPage * (pageNumber - 1))
                                        .Take(itemsOnPage)
                                        .ToListAsync();


                var offerList = new List<CarOfferAdverViewModel>();



            foreach (var item in offers)
            {
                var image = item.Files.FirstOrDefault();
                var fileViewModel = new FileViewModel();
                if (image != null)
                    fileViewModel = new FileViewModel()
                    {
                        Path = image.Path + image.Hash,
                        Name = image.Name
                    };
                offerList.Add(new CarOfferAdverViewModel
                {
                    ProducentName = item.CarProducent.ProducentName,
                    Fuel = item.Fuel.FuelName,
                    AddDate = item.AddDate,
                    Mileage = item.Mileage,
                    ModelName = item.CarModel.ModelName,
                    Price = item.Price,
                    Title = item.Title,
                    ShortDescription = item.ShortDescription,
                    Year = item.Year,
                    FileViewModel = fileViewModel
                });
            }

            var pagination = new Pagination
            {
                TotalItems = countOffers,
                ItemsOnPage = itemsOnPage,
                CurrentyPage = pageNumber
            };

            return new { Succeeded = true, offerList, pagination };
        }
               catch (Exception ex)
            {
                return new { Succeeded = false};
            }
        }
    }
}
