using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Helper;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IRepositoryManager _repository;
        protected readonly IConfiguration _configuration;
        protected readonly ILogger _logger;
        private string accessToken;
        public string AccessToken
        {
            get { 
                if(string.IsNullOrEmpty(accessToken))
                    accessToken = User.FindFirst(Data.ClaimTypes.AccessToken).Value;
                return accessToken;
            }
        }
        public BaseController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        protected BaseController(IRepositoryManager repository, IConfiguration configuration) : this(repository)
        {
            _configuration = configuration;
        }
        protected BaseController(IRepositoryManager repository, IConfiguration configuration, ILogger logger) : this(repository,configuration)
        {
            _logger = logger;
        }
        protected void PushNotification(NotificationOption options)
        {            
            TempData["notification"] = JsonConvert.SerializeObject(options);
        }
        private void PushError(Dictionary<string, string[]> errors)
        {
            foreach (var errorMessage in errors)
            {
                foreach (var error in errorMessage.Value)
                {
                    ModelState.AddModelError(errorMessage.Key, error);
                }
            }
        }
        protected IActionResult HandleErrors(ResponseModel response)
        {
            if (response is ErrorMessageResponseModel)
            {
                PushNotification(new NotificationOption
                {
                    Type = "error",
                    Message = (string)response.Data
                });
                return View();
            }
            else
            {
                PushError((Dictionary<string, string[]>)response.Data);
                return View();
            }
        }
        protected async Task<List<Category>> CreateSelectListCategory()
        {
            List<Category> sourceCategories = await _repository.Category.GetCategories();
            sourceCategories = CategoryHelper.CreateTreeLevelCategory(sourceCategories);
            List<Category> selectListCategory = new List<Category>();
            CategoryHelper.CreateSelectListCategory(sourceCategories, selectListCategory, 0);  
            return selectListCategory;
        }
    }
}
