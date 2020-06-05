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
    public class CarModelService : ICarModelService
    {
        private readonly DBContext _context;

        public CarModelService(DBContext context)
        {

            _context = context;
        }
        public async Task<object> Save(CarModelViewModel model)
        {
            try
            {

            var producer = await _context.CarProducents
                    .Where(x => x.IdString == model.IdProducer.ToLower())
                    .Include(x => x.CarModels)
                    .FirstOrDefaultAsync();

                string idString = new string(model.ModelName.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
              
                if (producer.CarModels.Where(x => x.IdString == idString).Where(x => x.Deleted == false).FirstOrDefault() != null)
                    return new
                    {
                        Succeeded = false,
                        Errors = new[]
                        {
                            new
                            {
                                Code = "BusyName"
                            }
                        }
                    };
                CarModel carModel = new CarModel()
            {
                Id = Guid.NewGuid(),
                ModelName = model.ModelName,
                IdString = idString,
                CarProducent = producer
            };
            await _context.CarModels.AddAsync(carModel);
            await _context.SaveChangesAsync();
                return new
                {
                    Succeeded = true
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Succeeded = false

                };
            }


        }

        public async Task<object> Get()
        {
            try
            {
                var result = await _context.CarModels
                    .Where(x => x.Deleted == false)
                    .Select(x => new CarModelViewModel()
                    {
                        IdString = x.IdString,
                        ModelName = x.ModelName
                    }).ToListAsync();
                return new
                {
                    Succeeded = true,
                    CarModelList = result
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    Succeeded = false
                };
            }
        }

        public async Task<CarModelViewModel> Get(string idString)
        {
            var model = await _context.CarModels.Where(x => x.IdString == idString.ToLower()).FirstOrDefaultAsync();
            if (model == null)
                return null;
            return new CarModelViewModel()
            {
                IdString = model.IdString,
                ModelName = model.ModelName
            };
        }

        public async Task<object> Update(CarModelViewModel model)
        {
            var models = await _context.CarModels.Include(x => x.CarProducent).ToListAsync();
            var item = models.Where(x=> x.CarProducent.IdString == model.IdProducer.ToLower())
                .Where(x => x.IdString == model.IdString.ToLower())
                .Where(x => x.Deleted == false)
                
                .FirstOrDefault();
            if (item == null)
                return new
                {
                    Succeeded = false,
                    Code = "NotFound"
                };
            if (model.ModelName.Length <= 0)
                return new
                {
                    Succeeded = false,
                    Code = "EmptyName"

                };

            var newIdString = new string(model.ModelName.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
            var isBusy = models.Where(x => x.IdString == newIdString).Where(x => x.Deleted == false).FirstOrDefault();
            if (isBusy != null && item.IdString != newIdString)
                return new
                {
                    Succeeded = false,
                    Code = "BusyName"
                };
            try
            {
                item.IdString = newIdString;
                item.ModelName = model.ModelName;
                var result = _context.CarModels.Update(item);
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

        public async Task<object> Remove(string idString, string idProducer)
        {
            try
            {
                var model = await _context.CarModels
                    .Where(x => x.CarProducent.IdString == idProducer.ToLower())
                    .Where(x => x.IdString == idString.ToLower())
                    .Include(x => x.CarProducent)
                    .FirstOrDefaultAsync();
                if (model == null)
                    return new
                    {
                        Succeeded = false,
                        Code = "NotFound"
                    };
                model.Deleted = true;
                _context.CarModels.Update(model);
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
