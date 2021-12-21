using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfFileDal : EfEntityRepositoryBase<File, ApplicationContext>, IFileDal
    {
        public List<FileDetailDto> GetFileDetails(Expression<Func<FileDetailDto, bool>> filter = null)
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                var result = from file in context.Files
                             join user in context.Users on file.UserId equals user.Id
                             select new FileDetailDto
                             {
                                 Id = file.Id,
                                 UserId = user.Id,
                                 UploaderName = user.FirstName + " " + user.LastName,
                                 UserName = file.UserName,
                                 Advisor = file.Advisor,
                                 CourseName = file.CourseName,
                                 Date = file.Date,
                                 DeliveryTime = file.DeliveryTime,
                                 FilePath = file.FilePath,
                                 Jury = file.Jury,
                                 Keywords = file.Keywords,
                                 ProjectTitle = file.ProjectTitle,
                                 Summary = file.Summary,
                                 UserNo = file.UserNo,
                                 UserPeriod = file.UserPeriod,
                             };
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }

        public FileDetailDto GetFileDetailsById(Expression<Func<FileDetailDto, bool>> filter)
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                var result = from file in context.Files
                             join user in context.Users on file.UserId equals user.Id
                             select new FileDetailDto
                             {
                                 Id = file.Id,
                                 UserId = user.Id,
                                 UploaderName = user.FirstName + " " + user.LastName,
                                 UserName = file.UserName,
                                 Advisor = file.Advisor,
                                 CourseName = file.CourseName,
                                 Date = file.Date,
                                 DeliveryTime = file.DeliveryTime,
                                 FilePath = file.FilePath,
                                 Jury = file.Jury,
                                 Keywords = file.Keywords,
                                 ProjectTitle = file.ProjectTitle,
                                 Summary = file.Summary,
                                 UserNo = file.UserNo,
                                 UserPeriod = file.UserPeriod,
                             };
                return result.Where(filter).FirstOrDefault();
            }
        }
    }
}
