namespace DataTech.System.Versioning.Models.Common
{
    public class OperationResult<TResponse> : OperationResult
    {
        public TResponse Response { get; set; }

        public OperationResult()
        {
        }

        public OperationResult(TResponse response)
        {
            Response = response;
        }

        public OperationResult(OperationResult result)
        {
            base.Result = result.Result;
            base.Message = result.Message;
            base.ErrorCode = result.ErrorCode;
            base.ErrorMessage = result.ErrorMessage;
            base.SysErrorMessage = result.SysErrorMessage;
            base.ErrorCategory = result.ErrorCategory;
        }
    }

    public class OperationResult
    {
        public bool Result { get; set; }

        public string Message { get; set; }

        public string ErrorCategory { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string SysErrorMessage { get; set; }
    }
}
