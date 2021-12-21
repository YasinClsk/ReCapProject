using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string file = @"D:\Codes\Visual Studio 2022\ReCapProject\WebAPI\wwwroot\Uploads\Images\Tez2.pdf";
            using(PdfReader reader = new PdfReader(file))
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
            ogrenciBul(stringBuilder.ToString().ToLower(), ref ogrenci, ref ogrenci2);
            ogrenciNumaraBul(stringBuilder.ToString().ToLower());
            dersAdi(stringBuilder.ToString().ToLower());
            ozetBul(stringBuilder.ToString());
            anahtarKelimeler(stringBuilder.ToString().ToLower());
            teslimDonemi(stringBuilder.ToString().ToLower());
            baslik(stringBuilder.ToString().ToLower(), ogrenci);
            danismanBul(stringBuilder.ToString().ToLower(),ogrenci2);
            juriBul(stringBuilder.ToString().ToLower());
        }

        static void juriBul(string text)
        {
            string[] split = text.Split("jüri üyesi");
            int index = split[0].LastIndexOf("...");
            string danisman1 = split[0].Substring(index + 3).Trim();
            int index2 = split[1].LastIndexOf("...");
            string danisman2 = split[1].Substring(index2 + 3).Trim();
            Console.WriteLine(danisman1);
            Console.WriteLine(danisman2);
        }
        static void danismanBul(string text, string ogrenci)
        {
            string[] split = text.Split("danışman");
            int index = split[0].LastIndexOf(ogrenci);
            string danisman = split[0].Substring(index+ogrenci.Length).Trim();
            Console.WriteLine(danisman);
        }

        static void baslik(string text,string ogrenci)
        {
            if(text.Contains("lisans tezi"))
            {
                int index = text.IndexOf("lisans tezi");
                int index2 = text.IndexOf(ogrenci);
                int length = index2 - (index+11);
                string baslik = text.Substring(index+11, length);
                Console.WriteLine(baslik.Trim());
            }
        }

        static void teslimDonemi(string text)
        {
            int index = text.IndexOf("tezin savunulduğu tarih:");
            string tarih = text.Substring(index + 25, 10);
            DateTime dateTime = DateTime.Parse(tarih);
            Console.WriteLine(dateTime);

            if (dateTime.Month > 6)
            {
                Console.WriteLine(dateTime.Year + "-" + (dateTime.Year + 1) + " Güz");
            }
            else
            {
                Console.WriteLine((dateTime.Year-1) + "-" + (dateTime.Year) + " Bahar");
            }
        }

        static void ozetBul(string text)
        {
            string[] _split = text.Split("Anahtar kelimeler:");
            int index = _split[0].LastIndexOf("ÖZET");

            string ozet = _split[0].Substring(index+4);
            Console.WriteLine(ozet);
            Console.WriteLine("-----------------------------------------");
        }

        static void ogrenciBul(string text,ref string ogrenci, ref string ogrenci2)
        {
            ArrayList ogrenciler = new ArrayList();
            string[] _split = text.Split("adı soyadı:");
            for (int i = 1; i < _split.Count(); i++)
            {
                int index = _split[i].IndexOf("imza");
                ogrenciler.Add(_split[i].Substring(0, index).Trim());
            }
            foreach (string s in ogrenciler)
            {
                Console.WriteLine(s);
            }

            ogrenci = ogrenciler[0].ToString();
            ogrenci2 = ogrenciler[ogrenciler.Count-1].ToString();
        } 

        static void ogrenciNumaraBul(string text)
        {
            ArrayList ogrenciler = new ArrayList();
            string[] _split = text.Split("öğrenci no:");
            for (int i = 1; i < _split.Count(); i++)
            {
                int index = _split[i].IndexOf("adı soyadı:");
                ogrenciler.Add(_split[i].Substring(0, index).Trim());
            }
            foreach (string s in ogrenciler)
            {
                Console.WriteLine(s);
            }
        }

        static void dersAdi(string text)
        {
            if(text.Contains("araştırma problemleri"))
            {
                Console.WriteLine("araştırma problemleri");
            }
            else if (text.Contains("bitirme projesi"))
            {
                Console.WriteLine("bitirme projesi");
            }
        }

        static void anahtarKelimeler(string text)
        {
            string[] _split = text.Split("anahtar kelimeler:");
            string temp = _split[1].Substring(0, _split[1].IndexOf('.')).Trim().ToLower();
            string[] keys = temp.Split(',');
            foreach (var key in keys)
            {
                Console.WriteLine(key.Trim());
            }
        }
    }
}
