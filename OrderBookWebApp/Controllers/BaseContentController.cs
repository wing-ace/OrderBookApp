using Microsoft.AspNetCore.Mvc;
using OrderBook.BLL.Common.ResultModels;

namespace OrderBookWebApp.Controllers
{
    /// <summary>
    /// The base controller for return data in default format
    /// </summary>
    public class BaseContentController : Controller
    {
        /// <summary>
        /// Transefer <see cref="ManagerResult{TResult}"/> to default controller result
        /// </summary>
        /// <typeparam name="TResult">The type return value</typeparam>
        /// <param name="managerResult">The result from manager</param>
        /// <returns>The default controller result</returns>
        protected static IActionResult TransferToControllerResult<TResult>(ManagerResult<TResult> managerResult)
            => new JsonResult(new
            {
                IsSuccess = !managerResult.IsInErrorState,
                Data = managerResult.Result,
                managerResult.ErrorModel
            });
    }
}
