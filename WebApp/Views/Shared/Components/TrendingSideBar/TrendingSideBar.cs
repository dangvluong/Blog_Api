using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;

namespace WebApp.Views.Shared.Components.TrendingSideBar
{
    [ViewComponent]
    public class TrendingSideBar : ViewComponent
    {
        private readonly IRepositoryManager _repository;
        public TrendingSideBar(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _repository.Post.GetTrendingPost());
        }
    }
}
