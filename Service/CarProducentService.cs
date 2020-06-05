using API.Models;
using API.ServiceInterfaces;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Service
{
    public class CarProducentService : ICarProducentService
    {

        DBContext _context;
        public CarProducentService(DBContext context)
        {

            _context = context;
        }
        public async Task<object> Save(CarProducentViewModel model)
        {
            try
            {
                string idString = new string(model.ProducentName.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
                var check = await _context.CarProducents
                    .Where(x => x.Deleted == false)
                    .Where(x => x.IdString == idString)
                    .Include(x => x.CarModels)
                    .FirstOrDefaultAsync();
                if (check != null)
                    return new
                    {
                        Succeeded = false,
                        Errors = new[] { new { Code = "BusyName" } }
                    };

                CarProducent carProducent = new CarProducent()
                {
                    Id = Guid.NewGuid(),
                    ProducentName = model.ProducentName,
                    IdString = idString,
                    CarModels = new List<CarModel>()

                };
                await _context.CarProducents.AddAsync(carProducent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new
                {
                    Succeeded = false
                };
            }
            return new
            {
                Succeeded = true
            };
        }

        public async Task<object> Get()
        {

            try
            {
                var result = await _context.CarProducents.Where(x => x.Deleted == false).Select(x => new CarProducentViewModel()
                {
                    IdString = x.IdString,
                    ProducentName = x.ProducentName
                }).ToListAsync();
                return new
                {
                    Succeeded = true,
                    CarProducerList = result
                };
            }
            catch(Exception ex)
            {
                return new
                {
                    Succeeded = false,
                    Error = ex
                };
            }
           
        }

        public async Task<object> Get(string idString)
        {
            try
            {
                var model = await _context.CarProducents
                    .Where(x => x.IdString == idString.ToLower())
                    .Where(x => x.Deleted == false)
                    .Include(x => x.CarModels)
                    .FirstOrDefaultAsync();

                if (model == null)
                    return new
                    {
                        Succeeded = false
                    };

                var carModelVMs = new List<CarModelViewModel>();
                if (model.CarModels != null)
                {
                    foreach (var item in model.CarModels)
                    {
                        if (item.Deleted == true)
                            continue;
                        var carVM = new CarModelViewModel()
                        {
                            IdString = item.IdString,
                            ModelName = item.ModelName
                        };
                        carModelVMs.Add(carVM);
                    }
                }
                return new
                {
                    Succeeded = true,
                    model.IdString,
                    model.ProducentName,
                    ModelList = carModelVMs
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

        public async Task<object> Update(CarProducentViewModel model)
        {
            var producers = await _context.CarProducents.ToListAsync();
            var item = producers.Where(x => x.IdString == model.IdString.ToLower()).Where(x => x.Deleted == false).FirstOrDefault();
            if (item == null)
                return new
                {
                    Succeeded = false,
                    Code = "NotFound"
                };
            if (model.ProducentName.Length <= 0)
                return new
                {
                    Succeeded = false,
                    Code = "EmptyName"

                };

            var newIdString = new string(model.ProducentName.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
            var isBusy = producers.Where(x => x.IdString == newIdString).Where(x => x.Deleted == false).FirstOrDefault();
            if (isBusy != null && item.IdString != newIdString)
                return new
                {
                    Succeeded = false,
                    Code = "BusyName"
                };
            try
            {
                item.IdString = newIdString;
                item.ProducentName = model.ProducentName;
                var result = _context.CarProducents.Update(item);
                await _context.SaveChangesAsync();
                return new
                {
                    Succeeded = true,

                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Succeeded = false,

                };
            }
            
        }

        public async Task<object> Remove(string idString)
        {
            try
            {
                var model = await _context.CarProducents
                    .Where(x => x.IdString == idString.ToLower())
                    .Include(x => x.CarModels)
                    .FirstOrDefaultAsync();
                if (model == null)
                    return new
                    {
                        Succeeded = false,
                        Code = "NotFound"
                    };
                model.Deleted = true;
                model.CarModels.ToList().ForEach(x => x.Deleted = true);
                _context.CarProducents.Update(model);
                await _context.SaveChangesAsync();
                return new
                {
                    Succeeded = true,

                };
            }
            catch
            {
                return new
                {
                    Succeeded = false,

                };
            }

        }
    }
}
