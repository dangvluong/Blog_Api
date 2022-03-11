using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin,Moderator")]
    public class HomeController : BaseController
    {
        public HomeController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index()
        {
            Statistic statistic = await _repository.Statistic.GetStatistic(AccessToken);
            return View(statistic);
        }
    }
}
