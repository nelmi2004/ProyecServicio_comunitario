namespace ProyecServicio_comunitario.Models.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? DetailedMessage { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse() { }

        // Helper para respuestas exitosas
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Success = true,
                Message = message,
                Data = data
            };
        }

        // Helper para respuestas de error
        public static ApiResponse<T> ErrorResponse(string message, string? detailed = null, int statusCode = 400, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Success = false,
                Message = message,
                DetailedMessage = detailed,
                Errors = errors
            };
        }
    }
}