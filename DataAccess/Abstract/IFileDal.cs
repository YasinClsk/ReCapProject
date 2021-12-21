using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IFileDal : IEntityRepository<File>
    {
        List<FileDetailDto> GetFileDetails(Expression<Func<FileDetailDto, bool>> filter = null);
        FileDetailDto GetFileDetailsById(Expression<Func<FileDetailDto, bool>> filter);
    }
}
