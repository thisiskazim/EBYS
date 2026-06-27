using EBYS.Application.DTOs.EvrakDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.GidenEvrakDTO.Validator
{
    public class GidenEvrakCreateValidator: AbstractValidator<GidenEvrakCreateDTO>
    {
        public GidenEvrakCreateValidator()
        {
            RuleFor(x => x.Konu)
                .NotEmpty().WithMessage("Konu alanı boş olamaz.");
            RuleFor(x => x.Muhataplar)
                .NotEmpty().WithMessage("Alıcılar boş olamaz");
            RuleFor(x => x.KonuKoduId)
               .NotEmpty().WithMessage("Konu Kodu boş olamaz");
            RuleFor(x => x.Icerik)
             .NotEmpty().WithMessage("İçerik boş olamaz");
        }
    }
}
