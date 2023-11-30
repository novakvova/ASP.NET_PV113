using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebRozetka.Data;
using WebRozetka.Models.Category;

namespace WebRozetka.Validators
{
    public class ValidatorCategoryCreate : AbstractValidator<CategoryCreateViewModel>
    {
        private readonly AppEFContext _appEFContext;
        public ValidatorCategoryCreate(AppEFContext appEFContext)
        {
            _appEFContext = appEFContext;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Поле назва є обов'язковим!")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Name).Must(BeUniqueName).WithMessage("Така категорія уже є!");
                });
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Поле опис є обов'язковим!");
            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("Фото є обов'язковим!");
        }
        private bool BeUniqueName(string name)
        {
            if(String.IsNullOrEmpty(name))
                return false;
            return !_appEFContext.Categories.Any(x => x.Name.ToLower().Equals(name.ToLower()));
        }
    }
}
