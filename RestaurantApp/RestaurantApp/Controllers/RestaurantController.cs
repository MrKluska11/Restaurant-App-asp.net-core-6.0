using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Entities;
using RestaurantApp.Models;
using Microsoft.AspNetCore.Http;
using RestaurantApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantApp.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private RestaurantSeeder _seeder;
        private readonly RestaurantDbContext _dbContext;


        public RestaurantController(IRestaurantService restaurantService, RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
            _restaurantService = restaurantService;
            _seeder = new RestaurantSeeder(_dbContext);
            _seeder.Seed();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CreateRestaurant([FromBody]CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var id = _restaurantService.Create(dto, userId);

            return Created($"api/restaurant/{ id }", null);
        }

        [HttpGet]
        //[Authorize(Policy = "AtLeast 20")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery]RestaurantQuery query)
        {
                var restaurantsDto = _restaurantService.GetAll(query);

                return Ok(restaurantsDto);

        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute]int id)
        {
            var restaurantDto = _restaurantService.GetById(id);

            return Ok(restaurantDto);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id, User);


            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Modify([FromRoute]int id, [FromBody]UpdateRestaurantDto dto)
        {
            _restaurantService.Modify(id, dto, User);

            return Ok();
        }
    }
}
