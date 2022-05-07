using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantApp.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirement> _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirement> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

            var userEmail = context.User.FindFirst(u => u.Type == ClaimTypes.Name).Value;

            _logger.LogInformation($"User: {userEmail} with date of birth {dateOfBirth}");

            if(dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
            {
                _logger.LogInformation("Authorization succed");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization Failed");
            }

            return Task.CompletedTask;
        }
    }
}
