namespace Domain.Common
{
    public abstract class BaseResponse<T>(T? result) where T : class
    {
        public List<string> Errors { get; private set; } = [];
        public bool HasErrors => Errors.Any();
        public T? Result { get; init; } = result;
        public void AddError(string error) => Errors.Add(error);
    }
}
