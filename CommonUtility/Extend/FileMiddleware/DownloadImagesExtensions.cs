using Microsoft.AspNetCore.Builder;

namespace CommonUtility.Extend.FileMiddleware
{

    /// <summary>
    /// 读取文件的中间件扩展
    /// </summary>
    public static class DownloadImagesExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDownloadImages(this IApplicationBuilder app, string directoryPath)
        {
            return app.UseMiddleware<DownloadImagesMiddleware>(directoryPath);
        }
    }
}
