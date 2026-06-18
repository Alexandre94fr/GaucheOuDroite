namespace Shared.DTOs 
{
    public class ApiResponseDTO
    {
        public bool HasSucceeded { get; set; } = false;
    
        public string Message { get; set; } = "";
    }
}