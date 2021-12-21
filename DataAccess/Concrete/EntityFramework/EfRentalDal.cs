using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRentalDal : EfEntityRepositoryBase<Rental, ApplicationContext>, IRentalDal
    {
        public List<RentalDetailDto> GetRentalDetails()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                var result = from rental in context.Rentals
                             join car in context.Cars on rental.CarId equals car.Id
                             join customer in context.Customers on rental.CustomerId equals customer.Id
                             join brand in context.Brands on car.BrandId equals brand.Id
                             join user in context.Users on customer.UserId equals user.Id
                             select new RentalDetailDto
                             {
                                 Id = car.Id,
                                 BrandName = brand.Name,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 RentDate = rental.RentDate,
                                 ReturnDate = rental.ReturnDate,
                             };
                return result.ToList();
            }
        }
    }
}
