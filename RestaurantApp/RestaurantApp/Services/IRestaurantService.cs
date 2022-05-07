using RestaurantApp.Models;
using System.Security.Claims;

namespace RestaurantApp.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto, int userId);
        PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
        RestaurantDto GetById(int id);
        void Delete(int id, ClaimsPrincipal user);
        void Modify(int id, UpdateRestaurantDto dto, ClaimsPrincipal user);
    }
}