using FluentValidation;
using RestaurantApp.Entities;

namespace RestaurantApp.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly RestaurantDbContext _dbContext;

        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(a => a.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(a => a.Password)
                .MinimumLength(6);

            RuleFor(a => a.Password).Equal(b => b.ConfirmPassword);

            RuleFor(a => a.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = _dbContext.Users.Any(a => a.Email == value);

                    if (emailInUse)
                        context.AddFailure("Email", "Podany email istnieje");
                });
        }
    }
}
