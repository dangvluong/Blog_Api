using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
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
        protected string AccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(accessToken))
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
        protected BaseController(IRepositoryManager repository, IConfiguration configuration, ILogger logger) : this(repository, configuration)
        {
            _logger = logger;
        }


        protected void PushNotification(NotificationOptions options)
        {
            TempData["notification"] = JsonConvert.SerializeObject(options);
        }
        private void FetchValidationError(Dictionary<string, string[]> errors)
        {
            foreach (var errorMessage in errors)
            {
                foreach (var error in errorMessage.Value)
                {
                    ModelState.AddModelError(errorMessage.Key, error);
                }
            }
        }
        protected void HandleErrors(ResponseModel response)
        {
            if (response is ErrorMessageResponseModel)
            {
                var error = response as ErrorMessageResponseModel;
                PushNotification(new NotificationOptions
                {
                    Type = "error",
                    Message = (string)error.Errors
                });
            }
            else
            {
                var errorValidationResponse = response as ErrorValidationResponseModel;
                FetchValidationError(errorValidationResponse.Errors);
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

        protected async Task<string> UploadThumbnail(IFormFile thumbnailImage)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StreamContent(thumbnailImage.OpenReadStream()), nameof(thumbnailImage), thumbnailImage.FileName);
            var response = await _repository.FileUpload.UploadThumbnail(content, AccessToken);
            if (response is SuccessResponseModel)
            {
                return (string)response.Data;
            }
            return null;
        }
    }
}
