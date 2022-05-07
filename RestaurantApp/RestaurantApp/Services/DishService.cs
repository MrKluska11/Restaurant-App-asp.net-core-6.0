using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Entities;
using RestaurantApp.Exceptions;
using RestaurantApp.Models;

namespace RestaurantApp.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext restaurantDbContext, IMapper mapper)
        {
            _dbContext = restaurantDbContext;
            _mapper = mapper;
        }

        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);

            if(restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantID = restaurantId;
            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == restaurantId);

            if(restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == dishId);

            if(dish is null || dish.RestaurantID != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            var DishDto = _mapper.Map<DishDto>(dish);
            return DishDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if(restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var dishesDto = _mapper.Map<List<DishDto>>(restaurant.Dishes);
            return dishesDto;
        }
    }
}
