using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/images1")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            //  var result = await _context.Files.Where(x => x.Hash == hash).FirstOrDefaultAsync();
            //  if(result != null)  var fileName = Path.GetFileName(image.FileName);
   
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "images", $"{id}.jpg");
            var a = path.Replace("//", "\\");
            var imageFileStream = System.IO.File.OpenRead(a);
            return "a";
        }
    }
}