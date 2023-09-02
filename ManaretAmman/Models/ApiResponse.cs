namespace ManaretAmman.Models
{
    public class ApiResponse<T> : ApiResponse, IApiResponse<T>
    {
        protected ApiResponse(bool isSuccess, string message, string[] errors) 
            : base(isSuccess, message, errors)
        {
        }
        protected ApiResponse(bool isSuccess, string message, string[] errors, T data)
            : base(isSuccess, message, errors)
        {
            Data = data;
        }

        public static IApiResponse<T> Success(string message, T data) => new ApiResponse<T>(true, message, null, data);
        public static IApiResponse<T> Success(T data) => new ApiResponse<T>(true, "Completed Successfully", null, data); 
        public static IApiResponse Failure(T data,string[] errors) => new ApiResponse<T>(false,"An Error Occured!", errors,data);


        public T Data { get; set; }
    }
    public class ApiResponse : IApiResponse
    {
        public string Message { get; }
        public bool IsSuccess { get; }
        public string[] Errors { get; }

        protected ApiResponse(bool isSuccess, string message, string[] errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors;
        }

        public static IApiResponse Success(string message) => new ApiResponse(true, message, null);
        public static IApiResponse Success() => Success("Complated Successfully");
        public static IApiResponse Failure(string message, string[] errors) => new ApiResponse(false, message, errors);
        public static IApiResponse Failure(string message) => Failure(message, null);
        public static IApiResponse Failure(string[] errors) => Failure("An Error Occured!", errors);
    }
}
