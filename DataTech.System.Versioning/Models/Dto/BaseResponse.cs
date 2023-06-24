namespace DataTech.System.Versioning.Models.Dto
{
    public class BaseResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }

    }

    public class BaseResponse<T> : BaseResponse
    {
        public T Response { get; set; }
    }
}
