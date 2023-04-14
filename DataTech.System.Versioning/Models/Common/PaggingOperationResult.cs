using System.Collections.Generic;

namespace DataTech.System.Versioning.Models.Common
{
    public class PaggingOperationResult<TResponse> : OperationResult<List<TResponse>>
    {
        public int TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public PaggingOperationResult()
        {
            base.Response = new List<TResponse>();
        }

        public PaggingOperationResult(List<TResponse> responses)
        {
            base.Response = responses;
        }

        public PaggingOperationResult(OperationResult result)
        {
            base.Result = result.Result;
            base.Message = result.Message;
            base.ErrorCode = result.ErrorCode;
            base.ErrorMessage = result.ErrorMessage;
            base.SysErrorMessage = result.SysErrorMessage;
            base.ErrorCategory = result.ErrorCategory;
            base.Response = new List<TResponse>();
        }
    }
}
