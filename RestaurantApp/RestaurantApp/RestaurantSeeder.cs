using Microsoft.EntityFrameworkCore;
using RestaurantApp.Entities;

namespace RestaurantApp
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }

                if(!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "KFC - Kentacky Fried Chicken is american fast food restaurant...",
                    ContactEmail = "contact@kfc.com",
                    ContactNumber = "666777888",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Nashville hot chicken",
                            Description = "This is Nashville hot chicken",
                            Price = 10.30m,
                        },

                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Description = "This is Chicken Nuggets",
                            Price = 10.30m,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-001"
                    }

                },
                new Restaurant()
                {
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description = "McDonald is american fast food restaurant...",
                    ContactEmail = "contact@mcdonald.com",
                    ContactNumber = "666777888",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Hamburger",
                            Description = "This is hamburger",
                            Price = 6.00m,
                        },
                        new Dish()
                        {
                            Name = "Cheaps",
                            Description = "This is Cheaps",
                            Price = 5.50m,
                        }
                    },
                    Address = new Address()
                    {
                        City = "Wrocław",
                        Street = "Hallera 100",
                        PostalCode = "53-230"
                    }

                }
            };

            return restaurants;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }
    }
}
