using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        IFileService _fileService;
        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost("add")]
        public IActionResult Add([FromForm(Name = "Image")] IFormFile file, [FromForm] File _file)
        {
            var result = _fileService.Add(file, _file);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("delete")]
        public IActionResult Delete(File file)
        {
            var result = _fileService.Delete(file);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("update")]
        public IActionResult Update([FromForm] IFormFile file, [FromForm] File _file)
        {
            var result = _fileService.Update(file, _file);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _fileService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getbyuserid")]
        public IActionResult GetByUserId(int userId)
        {
            var result = _fileService.GetByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getbyfileid")]
        public IActionResult GetByFileId(int fileId)
        {
            var result = _fileService.GetByFileId(fileId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
