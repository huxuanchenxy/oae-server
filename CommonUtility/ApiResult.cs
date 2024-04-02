using BusinessEntity.Other;
namespace CommonUtility
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<ApiResultDto> Success(object? data)
        {
            var res = Task.Run(() =>
            {
                return new ApiResultDto() { IsSuccess = true, Data = data };
            });
            return await res;
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<ApiResultDto> Error(string message)
        {
            var res = Task.Run(() =>
            {
                return new ApiResultDto() { IsSuccess = false, Message = message };
            });
            return await res;
        }
    }
}
