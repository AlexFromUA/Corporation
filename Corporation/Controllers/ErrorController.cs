using Microsoft.AspNetCore.Mvc;

namespace Corporation.Controllers;

public class ErrorController : Controller
{
    public IActionResult ErrorOccured()
    {
        return View("Error");
    }
}