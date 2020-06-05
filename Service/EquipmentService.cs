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
    public class EquipmentService : IEquipmentService
    {
        private readonly DBContext _context;

        public EquipmentService(DBContext context)
        {
            _context = context;
        }
        public async Task<object> Save(EquipmentViewModel model)
        {
            try
            {
                string idString = new string(model.Name.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
                if ( await _context.Equipments.Where(x => x.IdString == idString).Where(x => x.Deleted == false).FirstOrDefaultAsync() != null)
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
                Equipment equipment = new Equipment()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    IdString = idString,
                    Deleted = false
                };
                await _context.Equipments.AddAsync(equipment);
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
                var result = await _context.Equipments
                    .Where(x => x.Deleted == false)
                    .Select(x => new EquipmentViewModel()
                    {
                        IdString = x.IdString,
                        Name = x.Name
                    }).ToListAsync();
                return new
                {
                    Succeeded = true,
                    EquipmentList = result
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

        public async Task<EquipmentViewModel> Get(string idString)
        {
            var model = await _context.Equipments.Where(x => x.IdString == idString.ToLower()).FirstOrDefaultAsync();

            if (model == null)
                return null;
            return new EquipmentViewModel()
            {
                IdString = model.IdString,
                Name = model.Name
            };
        }

        public async Task<object> Update(EquipmentViewModel model)
        {
            var models = await _context.Equipments.ToListAsync();
            var item = models.Where(x => x.IdString == model.IdString.ToLower()).Where(x => x.Deleted == false).FirstOrDefault();
            if (item == null)
                return new
                {
                    Succeeded = false,
                    Code = "NotFound"
                };
            if (model.Name.Length <= 0)
                return new
                {
                    Succeeded = false,
                    Code = "EmptyName"

                };

            var newIdString = new string(model.Name.Select(x => char.IsLetterOrDigit(x) ? x : '-').ToArray()).ToLower();
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
                item.Name = model.Name;
                var result = _context.Equipments.Update(item);
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
                var model = await _context.Equipments
                    .Where(x => x.IdString == idString.ToLower())
                    .FirstOrDefaultAsync();
                if (model == null)
                    return new
                    {
                        Succeeded = false,
                        Code = "NotFound"
                    };
                model.Deleted = true;
                _context.Equipments.Update(model);
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
