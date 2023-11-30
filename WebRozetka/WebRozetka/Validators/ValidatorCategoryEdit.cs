using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebRozetka.Data;
using WebRozetka.Models.Category;

namespace WebRozetka.Validators
{
    public class ValidatorCategoryEdit : AbstractValidator<CategoryEditViewModel>
    {
        private readonly AppEFContext _appEFContext;
        public ValidatorCategoryEdit(AppEFContext appEFContext)
        {
            _appEFContext = appEFContext;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Поле назва є обов'язковим!")
                .DependentRules(() =>
                {
                    RuleFor(x => x).Must(BeUniqueName).WithMessage("Така категорія уже є!");
                });
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Поле опис є обов'язковим!");
        }
        private bool BeUniqueName(CategoryEditViewModel model)
        {
            if(String.IsNullOrEmpty(model.Name))
                return false;
            return !_appEFContext.Categories
                .Where(x=>x.Id!=model.Id) //щоб при едіти назва залишалася
                .Any(x => x.Name.ToLower().Equals(model.Name.ToLower()));
        }
    }
}
