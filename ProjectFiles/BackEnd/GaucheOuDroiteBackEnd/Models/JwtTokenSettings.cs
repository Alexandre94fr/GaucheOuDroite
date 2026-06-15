namespace GaucheOuDroiteBackEnd.Models
{
    /// <summary> 
    /// JWT stands for JSON Web Token. </summary>
    public class JwtTokenSettings
    {
        public string Key { get; set; } = "";

        public string Issuer { get; set; } = "";

        public string Audience { get; set; } = "";

        public int ExpiryMinutes { get; set; }
    }
}