using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RentalManager rentalManager = new RentalManager(new EfRentalDal());

            var result = rentalManager.Add(new Rental
            {
                CarId = 1,
                CustomerId = 3,
                RentDate = DateTime.Now,
                ReturnDate = DateTime.Today.AddDays(1)
            });

            Console.WriteLine(result.Message);
        }
    }
}
