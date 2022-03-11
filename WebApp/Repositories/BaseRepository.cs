using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public abstract class BaseRepository
    {
        protected HttpClient client;
        public BaseRepository(HttpClient client)
        {
            this.client = client;
        }

        //Nothing in,data out (get request)
        protected async Task<ResponseModel> Send<TDataOut>(string url, Func<HttpClient, string, Task<HttpResponseMessage>> sendRequest, Func<HttpResponseMessage,Task<TDataOut>> fetchData,string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await sendRequest.Invoke(client, url);
            if (message.IsSuccessStatusCode)
            {
                return new SuccessResponseModel
                {
                    Status = (int)message.StatusCode,
                    Data = await fetchData.Invoke(message)
                };
            }
            return await HandleError(message);
        }
        //Nothing in, nothing out, only via url
        protected async Task<ResponseModel> Send(string url, Func<HttpClient, string, Task<HttpResponseMessage>> sendRequest, string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await sendRequest.Invoke(client, url);
            if (message.IsSuccessStatusCode)
            {
                return new SuccessResponseModel
                {
                    Status = (int)message.StatusCode                    
                };
            }
            return await HandleError(message);
        }


        //Data in, nothing out
        protected async Task<ResponseModel> Send<TIn>(string url, TIn obj, Func<HttpClient, string,TIn, Task<HttpResponseMessage>> sendRequest,string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await sendRequest.Invoke(client, url,obj);
            if (message.IsSuccessStatusCode)
            {
                return new SuccessResponseModel
                {
                    Status = (int)message.StatusCode                    
                };
            }
            return await HandleError(message);
        }
        //Data in, data out
        protected async Task<ResponseModel> Send<TIn,TDataOut>(string url, TIn obj, Func<HttpClient, string, TIn, Task<HttpResponseMessage>> sendRequest,Func<HttpResponseMessage,Task<TDataOut>> fetchData, string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await sendRequest.Invoke(client, url, obj);
            if (message.IsSuccessStatusCode)
            {
                return new SuccessResponseModel
                {
                    Status = (int)message.StatusCode,
                    Data = await fetchData.Invoke(message)
                };
            }
            return await HandleError(message);
        }       

        private async Task<ResponseModel> HandleError(HttpResponseMessage message)
        {
            try
            {
                var errors = JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
                return errors;
            }
            catch (Exception)
            {
                return new ErrorMessageResponseModel
                {
                    Status = (int)message.StatusCode,
                    Errors = await message.Content.ReadAsStringAsync()
                };
            }
        }
    }
}
