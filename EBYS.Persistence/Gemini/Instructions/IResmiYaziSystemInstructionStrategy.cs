using EBYS.Domain.Enum;

namespace EBYS.Persistence.Gemini.Instructions
{
    public interface IResmiYaziSystemInstructionStrategy
    {
        Enums.DocumentType DocumentType { get; }
        string GetSystemInstruction();
    }
}
