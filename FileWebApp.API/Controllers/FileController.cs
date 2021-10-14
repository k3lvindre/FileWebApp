using FileWebApp.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileWebApp.API.Services;

namespace FileWebApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private IFileService _fileService { get; }

        public FileController(ILogger<FileController> logger,
                                          IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpPost("Post")]
        public async Task<IActionResult> UploadTransaction([FromBody] Transaction item)
        {
            item = await _fileService.UploadFileAsync(item);
            if (item.CustomerId > 0)
            {
                return Accepted();
            }
            else return BadRequest();
        }
        
    }
}
