using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Helpers.FileHelper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class FileManager : IFileService
    {
        IFileDal _fileDal;
        IFileHelper _fileHelper;
        public FileManager(IFileDal carImageDal, IFileHelper fileHelper)
        {
            _fileDal = carImageDal;
            _fileHelper = fileHelper;
        }
        public IResult Add(IFormFile file, File _file)
        {
            /*IResult result = BusinessRules.Run(CheckIfCarImageLimit(carImage.UserId));
            if (result != null)
            {
                return result;
            }*/
            _file.FilePath = _fileHelper.Upload(file, PathConstants.ImagesPath);
            _file.Date = DateTime.Now.ToString("dd/MM/yyyy");

            StringBuilder stringBuilder = new StringBuilder();
            string filePath = @"D:\Codes\Visual Studio 2022\ReCapProject\WebAPI\wwwroot\Uploads\Images\" + _file.FilePath;
            
            using (PdfReader reader = new PdfReader(filePath))
            {
                for (int pageNo = 1; pageNo < reader.NumberOfPages; pageNo++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string text = PdfTextExtractor.GetTextFromPage(reader, pageNo, strategy);
                    text = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
                    stringBuilder.Append(text);
                }
            }

            string ogrenci = "";
            string ogrenci2 = "";
            //Console.WriteLine(stringBuilder.ToString());
            _file.UserName = ogrenciBul(stringBuilder.ToString().ToLower(), ref ogrenci, ref ogrenci2);
            _file.UserNo = ogrenciNumaraBul(stringBuilder.ToString().ToLower());
            _file.CourseName = dersAdi(stringBuilder.ToString().ToLower());
            _file.Summary = ozetBul(stringBuilder.ToString());
            _file.Keywords = anahtarKelimeler(stringBuilder.ToString().ToLower());
            _file.DeliveryTime = teslimDonemi(stringBuilder.ToString().ToLower());
            _file.ProjectTitle = baslik(stringBuilder.ToString().ToLower(), ogrenci);
            _file.Advisor = danismanBul(stringBuilder.ToString().ToLower(), ogrenci2);
            _file.Jury = juriBul(stringBuilder.ToString().ToLower());
            _file.UserPeriod = ogrenciDonemBul(stringBuilder.ToString().ToLower());

            _fileDal.Add(_file);
            return new SuccessResult("Dosya başarıyla yüklendi");
        }

        public IResult Delete(File carImage)
        {
            _fileHelper.Delete(PathConstants.ImagesPath + carImage.FilePath);
            _fileDal.Delete(carImage);
            return new SuccessResult();
        }
        public IResult Update(IFormFile file, File carImage)
        {
            carImage.FilePath = _fileHelper.Update(file, PathConstants.ImagesPath + carImage.FilePath, PathConstants.ImagesPath);
            _fileDal.Update(carImage);
            return new SuccessResult();
        }

        public IDataResult<List<FileDetailDto>> GetByUserId(int userId)
        {
            var result = BusinessRules.Run(CheckCarImage(userId));
            /*
            if (result != null)
            {
                return new SuccessDataResult<List<CarImage>>(GetDefaultImage(userId).Data);
            }*/
            return new SuccessDataResult<List<FileDetailDto>>(_fileDal.GetFileDetails(c => c.UserId == userId));
        }

        public IDataResult<FileDetailDto> GetByFileId(int imageId)
        {
            return new SuccessDataResult<FileDetailDto>(_fileDal.GetFileDetailsById(c => c.Id == imageId));
        }

        public IDataResult<List<FileDetailDto>> GetAll()
        {
            return new SuccessDataResult<List<FileDetailDto>>(_fileDal.GetFileDetails());
        }
        private IResult CheckIfCarImageLimit(int userId)
        {
            var result = _fileDal.GetAll(c => c.UserId == userId).Count;
            if (result >= 5)
            {
                return new ErrorResult();
            }
            return new SuccessResult();
        }
        //private IDataResult<List<File>> GetDefaultImage(int userId)
        //{
        //    //List<File> carImage = new List<File>();
        //    //carImage.Add(new File { UserId = userId, Date = DateTime.Now, FilePath = "DefaultImage.jpg" });
        //    //return new SuccessDataResult<List<File>>(carImage);
        //}
        private IResult CheckCarImage(int userId)
        {
            var result = _fileDal.GetAll(c => c.UserId == userId).Count;
            if (result > 0)
            {
                return new SuccessResult();
            }
            return new ErrorResult();
        }

        private string juriBul(string text)
        {
            string[] split = text.Split("jüri üyesi");
            int index = split[0].LastIndexOf("...");
            string juri1 = split[0].Substring(index + 3).Trim();
            int index2 = split[1].LastIndexOf("...");
            string juri2 = split[1].Substring(index2 + 3).Trim();
            return juri1 + "," + juri2;
        }
        private string danismanBul(string text, string ogrenci)
        {
            string[] split = text.Split("danışman");
            int index = split[0].LastIndexOf(ogrenci);
            string danisman = split[0].Substring(index + ogrenci.Length).Trim();
            return danisman;
        }

        private string baslik(string text, string ogrenci)
        {
            int index = text.IndexOf("lisans tezi");
            int index2 = text.IndexOf(ogrenci);
            int length = index2 - (index + 11);
            string baslik = text.Substring(index + 11, length);
            Console.WriteLine(baslik.Trim());

            return baslik;
        }

        private string teslimDonemi(string text)
        {
            int index = text.IndexOf("tezin savunulduğu tarih:");
            string tarih = text.Substring(index + 25, 10);
            DateTime dateTime = DateTime.Parse(tarih);
            Console.WriteLine(dateTime);

            if (dateTime.Month > 6)
            {
                return ((dateTime.Year) + "-" + (dateTime.Year + 1) + " Güz");
            }
            else if(dateTime.Month < 2)
            {
                return ((dateTime.Year - 1) + "-" + (dateTime.Year) + " Güz");
            }
            else
            {
                return ((dateTime.Year - 1) + "-" + (dateTime.Year) + " Bahar");
            }
        }

        private string ozetBul(string text)
        {
            string[] _split = text.Split("Anahtar kelimeler:");
            int index = _split[0].LastIndexOf("ÖZET");

            string ozet = _split[0].Substring(index + 4);
            return ozet;
        }

        private string ogrenciBul(string text, ref string ogrenci, ref string ogrenci2)
        {
            ArrayList ogrenciler = new ArrayList();
            string[] _split = text.Split("adı soyadı:");
            for (int i = 1; i < _split.Count(); i++)
            {
                int index = _split[i].IndexOf("imza");
                ogrenciler.Add(_split[i].Substring(0, index).Trim());
            }

            string tumOgrenciler = "";
            foreach (string s in ogrenciler)
            {
                tumOgrenciler += s + ",";
            }

            tumOgrenciler = tumOgrenciler.Substring(0, tumOgrenciler.Length - 1);

            ogrenci = ogrenciler[0].ToString();
            ogrenci2 = ogrenciler[ogrenciler.Count - 1].ToString();

            return tumOgrenciler;
        }

        private string ogrenciNumaraBul(string text)
        {
            ArrayList ogrenciler = new ArrayList();
            string[] _split = text.Split("öğrenci no:");
            for (int i = 1; i < _split.Count(); i++)
            {
                int index = _split[i].IndexOf("adı soyadı:");
                ogrenciler.Add(_split[i].Substring(0, index).Trim());
            }

            string tumNumaralar = "";
            foreach (string s in ogrenciler)
            {
                tumNumaralar += s + ",";
            }

            tumNumaralar = tumNumaralar.Substring(0, tumNumaralar.Length - 1);

            return tumNumaralar;
        }

        private string ogrenciDonemBul(string text)
        {
            ArrayList ogrenciler = new ArrayList();
            string[] _split = text.Split("öğrenci no:");
            for (int i = 1; i < _split.Count(); i++)
            {
                int index = _split[i].IndexOf("adı soyadı:");
                ogrenciler.Add(_split[i].Substring(0, index).Trim());
            }

            string tumDonemler = "";
            foreach (string s in ogrenciler)
            {
                if(s[5] == '1')
                {
                    tumDonemler += "1.Öğretim,";
                }
                else
                {
                    tumDonemler += "2.Öğretim,";
                }
                
            }
            tumDonemler = tumDonemler.Substring(0, tumDonemler.Length - 1);
            return tumDonemler;
        }

        private string dersAdi(string text)
        {
            if (text.Contains("araştırma problemleri"))
            {
                return ("araştırma problemleri");
            }
            else
            {
                return("bitirme projesi");
            }
        }

        private string anahtarKelimeler(string text)
        {
            string[] _split = text.Split("anahtar kelimeler:");
            string temp = _split[1].Substring(0, _split[1].IndexOf('.')).Trim().ToLower();
            string[] keys = temp.Split(',');

            string anahtarKelimeler = "";
            foreach (var key in keys)
            {
                anahtarKelimeler += key + ",";
            }

            anahtarKelimeler = anahtarKelimeler.Substring(0, anahtarKelimeler.Length - 1);
            return anahtarKelimeler;
        }
    }
}
