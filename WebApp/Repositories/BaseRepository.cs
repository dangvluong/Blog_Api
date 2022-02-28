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
        //protected async Task<T> Get<T>(string url, string token = null)
        //{           
        //    return await SendRequest<T>(url, (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<T>(), token);
        //}

        //protected async Task<TOut> PostJson<TIn, TOut>(string url, TIn obj, string token = null)
        //{          
        //    return await SendRequest<TIn, TOut>(url, obj, (client, url, obj) => client.PostAsJsonAsync<TIn>(url, obj), message => message.Content.ReadAsAsync<TOut>(), token);
        //}
        //protected async Task<TOut> Post<TOut>(string url, HttpContent obj = null, string token = null)
        //{          
        //    return await SendRequest(url, obj, (client, url, obj) => client.PostAsync(url, obj), message => message.Content.ReadAsAsync<TOut>(), token);
        //}

        //protected async Task<string> PostImage(string url, HttpContent obj, string token = null)
        //{          
        //    return await SendRequest<HttpContent, string>(url, obj,(client,url,obj) => client.PostAsync(url,obj),message => message.Content.ReadAsStringAsync(), token);
        //}

        //protected async Task<int> Put<T>(string url, T obj, string token = null)
        //{           
        //    return await SendRequest<T, int>(url, obj, (client, url, obj) => client.PutAsJsonAsync<T>(url, obj), message => message.Content.ReadAsAsync<int>(), token);
        //}
        //protected async Task<int> Delete(string url, string token = null)
        //{           
        //    return await SendRequest<int>(url, (client,url) => client.DeleteAsync(url),message => message.Content.ReadAsAsync<int>(), token);            
        //}

        //private async Task<TOUt> SendRequest<TIn, TOUt>(string url, TIn obj, Func<HttpClient, string, TIn, Task<HttpResponseMessage>> sendRequest, Func<HttpResponseMessage, Task<TOUt>> getResponseResult, string token = null)
        //{
        //    if (token != null)
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    HttpResponseMessage message = await sendRequest.Invoke(client, url, obj);
        //    return await GetResponse(getResponseResult, message);
        //}

        //private async Task<TOUt> GetResponse<TOUt>(Func<HttpResponseMessage, Task<TOUt>> getResponseResult, HttpResponseMessage message)
        //{
        //    if (message.IsSuccessStatusCode)
        //    {
        //        return await getResponseResult.Invoke(message);
        //    }
        //    return default(TOUt);
        //}





        //private async Task<TOUt> SendRequest<TOUt>(string url, Func<HttpClient, string,  Task<HttpResponseMessage>> sendRequest, Func<HttpResponseMessage, Task<TOUt>> getResponseResult, string token = null)
        //{
        //    if (token != null)
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    HttpResponseMessage message = await sendRequest.Invoke(client, url);
        //    return await GetResponse(getResponseResult, message);           
        //}



        //Nothing,data out (get request)
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
        ////Nothing in, data out
        //protected async Task<ResponseModel> Send<TOut>(string url, Func<HttpClient, string, Task<HttpResponseMessage>> sendRequest, Func<HttpResponseMessage, TOut> getResponse, string token = null)
        //{
        //    if (token != null)
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    HttpResponseMessage message = await sendRequest.Invoke(client, url);
        //    if (message.IsSuccessStatusCode)
        //    {
        //        return new SuccessResponseModel
        //        {
        //            Status = (int)message.StatusCode
        //        };
        //    }
        //    return await HandleError(message);
        //}

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
