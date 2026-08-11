using FluentValidation;

namespace EBYS.Application.DTOs.ResmiYaziDTO.Validator
{
    public class ResmiYaziGenerateRequestValidator : AbstractValidator<ResmiYaziGenerateRequest>
    {
        public ResmiYaziGenerateRequestValidator()
        {
            RuleFor(x => x.YaziTuru)
                .IsInEnum().WithMessage("Geçerli bir yazı türü seçilmelidir.");

            RuleFor(x => x.YaziUzunlugu)
                .IsInEnum().WithMessage("Geçerli bir yazı uzunluğu seçilmelidir.");

            RuleFor(x => x.TaslakMetin)
                .NotEmpty().WithMessage("Taslak metin boş olamaz.")
                .MinimumLength(10).WithMessage("Taslak metin en az 10 karakter olmalıdır.");
        }
    }
}
