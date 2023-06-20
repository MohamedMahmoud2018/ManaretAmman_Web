namespace ManaretAmman.Models
{
    public interface IApiResponse
    {
        string[] Errors { get; }
        bool IsSuccess { get; }
        string Message { get; }
    }
    public interface IApiResponse<T> : IApiResponse
    {
        public T Data { get; }
    }
}