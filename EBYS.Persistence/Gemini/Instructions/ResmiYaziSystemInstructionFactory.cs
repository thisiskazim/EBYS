using EBYS.Domain.Enum;
using EBYS.Domain.Exceptions;

namespace EBYS.Persistence.Gemini.Instructions
{
    public class ResmiYaziSystemInstructionFactory
    {
        private readonly IReadOnlyDictionary<Enums.DocumentType, IResmiYaziSystemInstructionStrategy> _strategies;

        public ResmiYaziSystemInstructionFactory(IEnumerable<IResmiYaziSystemInstructionStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(s => s.DocumentType);
        }

        public string GetSystemInstruction(Enums.DocumentType documentType, Enums.YaziUzunlugu yaziUzunlugu)
        {
            var baseInstruction = documentType switch
            {
                Enums.DocumentType.Dilekce => GetStrategy(Enums.DocumentType.Dilekce),
                Enums.DocumentType.UstYazi => GetStrategy(Enums.DocumentType.UstYazi),
                Enums.DocumentType.IcYazisma => GetStrategy(Enums.DocumentType.IcYazisma),
                _ => throw new DesteklenmeyenYaziTuru()
            };

            return baseInstruction + "\n\n" + YaziUzunlukInstructionHelper.GetLengthRules(yaziUzunlugu);
        }

        private string GetStrategy(Enums.DocumentType documentType)
        {
            if (!_strategies.TryGetValue(documentType, out var strategy))
                throw new DesteklenmeyenYaziTuru();

            return strategy.GetSystemInstruction();
        }
    }
}
