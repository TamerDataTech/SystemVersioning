using DataTech.System.Versioning.Models.Common;
using System;

namespace DataTech.System.Versioning.Extensions
{
    public static class ResultExtensions
    {
        public static void PrepareMissingParameterResult(this OperationResult result, string missingParameter)
        {
            result.ErrorMessage = missingParameter;
            result.ErrorCategory = "Validation";
            result.ErrorCode = "System.General.MissingParameter";
            result.Result = false;
        }

        public static void PrepareExceptionResult<T>(this OperationResult<T> result, Exception ex, string errorMessage = "")
        {
            result.Result = false;
            result.SysErrorMessage = ex.Message;
            result.ErrorCategory = "Exception";
            result.ErrorCode = "System.General.Exception";
            result.ErrorMessage = (errorMessage.IsEmpty() ? result.ErrorMessage : errorMessage);
        }

        public static void PrepareNotFoundResult<T>(this OperationResult<T> result, string errorMessage = "")
        {
            result.ErrorMessage = (errorMessage.IsEmpty() ? result.ErrorMessage : errorMessage);
            result.ErrorCategory = "Validation";
            result.ErrorCode = "System.General.NotFound";
            result.Result = false;
        }

        public static void PrepareExistedPropertyResult<T>(this OperationResult<T> result, string errorMessage = "")
        {
            result.ErrorMessage = (errorMessage.IsEmpty() ? result.ErrorMessage : errorMessage);
            result.ErrorCategory = "Validation";
            result.ErrorCode = "System.Validation.ExistsBefore";
            result.Result = false;
        }
    }
}
