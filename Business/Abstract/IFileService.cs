using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFileService
    {
        IResult Add(IFormFile file, File _file);
        IResult Delete(File _file);
        IResult Update(IFormFile file, File _file);
        IDataResult<List<FileDetailDto>> GetAll();
        IDataResult<FileDetailDto> GetByFileId(int fileId);
        IDataResult<List<FileDetailDto>> GetByUserId(int userId);
    }
}
