using Microsoft.AspNetCore.Mvc;
using PracticalNineteen.Models;
using System.Diagnostics;

namespace PracticalNineteen.Controllers
{
    public class ErrorController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
