using DataTech.System.Versioning.Models.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System;

namespace DataTech.System.Versioning.Extensions
{
    public class SqlExecptionHandler
    {
        private readonly ILogger<SqlExecptionHandler> _logger;

        private const int SqlServerViolationOfUniqueIndex = 2601;

        private const int SqlServerViolationOfUniqueConstraint = 2627;

        private readonly Regex UniqueConstraintRegex = new Regex("'UniqueError_([a-zA-Z0-9]*)_([a-zA-Z0-9]*)'", RegexOptions.Compiled);

        public SqlExecptionHandler(ILogger<SqlExecptionHandler> logger)
        {
            _logger = logger;
        }

        public void HandelException(Exception e, OperationResult result, object entity = null)
        {
            result.Result = false;
            result.ErrorCategory = "Exception";
            result.ErrorCode = "System.General.Exception";
            result.ErrorMessage = "System.General.Exception";
            result.SysErrorMessage = e.Message;
            _logger.LogError(e, "SqlExecption {@entity}", entity);
            _logger.LogTrace(e, "LogTrace SqlExecption {@entity}", entity);
            if (e.InnerException != null && e.InnerException is SqlException)
            {
                _logger.LogError(e.InnerException, "SqlExecption -InnerException {@entity}", entity);
                SqlException ex = e.InnerException as SqlException;
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    UniqueErrorFormatter(ex, result);
                    return;
                }

                result.ErrorCode = "System.General.Exception";
                result.ErrorCategory = "Exception";
                result.ErrorMessage = e.Message;
                result.SysErrorMessage = e.Message + "  - " + e.InnerException?.Message;
            }
            else
            {
                _logger.LogError(e.InnerException, "SqlExecption -InnerException {@entity}", entity);
                result.ErrorCode = "System.General.Exception";
                result.ErrorCategory = "Exception";
                result.ErrorMessage = e.Message;
                result.SysErrorMessage = e.Message + "  - " + e.InnerException?.Message;
            }
        }

        private void UniqueErrorFormatter(SqlException ex, OperationResult result)
        {
            string message = ex.Errors[0].Message;
            MatchCollection matchCollection = UniqueConstraintRegex.Matches(message);
            if (matchCollection.Count != 0)
            {
                string value = matchCollection[0].Groups[1].Value;
                _logger.LogError("Cannot have a duplicate {value} in  {entity}", matchCollection[0].Groups[2].Value, value);
                int num = message.IndexOf("(");
                if (num > 0)
                {
                    string text = message.Substring(num + 1, message.Length - num - 3);
                    _logger.LogError("Duplicate value was  {value}", text);
                }

                result.ErrorCode = "System.General.UsedBefore";
                result.ErrorCategory = "Exception";
                result.ErrorMessage = matchCollection[0].Groups[2].Value;
            }
        }
    }
}
