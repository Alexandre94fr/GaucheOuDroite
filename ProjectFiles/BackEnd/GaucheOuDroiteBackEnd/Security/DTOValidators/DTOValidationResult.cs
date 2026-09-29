namespace GaucheOuDroiteBackEnd.Security.DTOValidators
{
    public class DTOValidationResult
    {
        public bool IsValid { get; init; }
        public string ErrorMessage { get; init; } = "";


        public static DTOValidationResult Valid()
        {
            return new()
            {
                IsValid = true
            };
        }

        public static DTOValidationResult Invalid(string p_errorMessage)
        {
            return new()
            {
                IsValid = false,
                ErrorMessage = p_errorMessage
            };
        }
    }
}