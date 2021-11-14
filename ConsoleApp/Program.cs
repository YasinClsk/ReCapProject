using Business.Concrete;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CarManager carManager = new CarManager(new InMemoryCarDal());
            carManager.Add(new Car { Id = 5, BrandId = 2, ColorId = 1, ModelYear = 2021, DailyPrice = 4000, Description = "BMW" });
            foreach (var item in carManager.GetList())
            {
                Console.WriteLine(item.Description);
            }
        }
    }
}
