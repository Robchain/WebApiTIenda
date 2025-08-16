namespace WebApiTIenda.Application.DTOs.Admin
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
