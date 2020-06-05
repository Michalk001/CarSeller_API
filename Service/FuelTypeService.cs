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
    public class FuelTypeService : IFuelTypeService
    {

        private readonly DBContext _context;

        public FuelTypeService(DBContext context)
        {
            _context = context;
        }
        public async Task<object> Save(FuelTypeViewModel model)
        {
            try
            {
                string idString = new string(model.FuelName.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
                if (await _context.FuelTypes.Where(x => x.IdString == idString).Where(x => x.Deleted == false).FirstOrDefaultAsync() != null)
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
                FuelType fuelType = new FuelType()
                {
                    Id = Guid.NewGuid(),
                    FuelName = model.FuelName,
                    IdString = idString,
                    Deleted = false
                };
                await _context.FuelTypes.AddAsync(fuelType);
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
                var result = await _context.FuelTypes
                    .Where(x => x.Deleted == false)
                    .Select(x => new FuelType()
                    {
                        IdString = x.IdString,
                        FuelName = x.FuelName
                    }).ToListAsync();
                return new
                {
                    Succeeded = true,
                    FuelList = result
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

        public async Task<FuelTypeViewModel> Get(string idString)
        {
            var model = await _context.FuelTypes.Where(x => x.IdString == idString.ToLower()).FirstOrDefaultAsync();
            if (model == null)
                return null;
            return new FuelTypeViewModel()
            {
                IdString = model.IdString,
                FuelName = model.FuelName
            };
        }

        public async Task<object> Update(FuelTypeViewModel model)
        {
            var models = await _context.FuelTypes.ToListAsync();
            var item = models.Where(x => x.IdString == model.IdString.ToLower()).Where(x => x.Deleted == false).FirstOrDefault();
            if (item == null)
                return new
                {
                    Succeeded = false,
                    Code = "NotFound"
                };
            if (model.FuelName.Length <= 0)
                return new
                {
                    Succeeded = false,
                    Code = "EmptyName"

                };

            var newIdString = new string(model.FuelName.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
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
                item.FuelName = model.FuelName;
                var result = _context.FuelTypes.Update(item);
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
                var model = await _context.FuelTypes
                    .Where(x => x.IdString == idString.ToLower())
                    .FirstOrDefaultAsync();
                if (model == null)
                    return new
                    {
                        Succeeded = false,
                        Code = "NotFound"
                    };
                model.Deleted = true;
                _context.FuelTypes.Update(model);
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
