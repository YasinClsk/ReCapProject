using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class FileDetailDto : IDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UploaderName { get; set; }
        public string FilePath { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string UserNo { get; set; }
        public string UserPeriod { get; set; }
        public string CourseName { get; set; }
        public string Summary { get; set; }
        public string DeliveryTime { get; set; }
        public string ProjectTitle { get; set; }
        public string Keywords { get; set; }
        public string Advisor { get; set; }
        public string Jury { get; set; }
    }
}
