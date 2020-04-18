namespace APBD3.Responses
{
    public class BadRequestDto
    {
        public string Message { get; set; }

        public BadRequestDto(string message)
        {
            Message = message;
        }
    }
}