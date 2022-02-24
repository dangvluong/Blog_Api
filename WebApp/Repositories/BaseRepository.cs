using System.Net.Http.Headers;

namespace WebApp.Repositories
{
    public abstract class BaseRepository
    {
        protected HttpClient client;
        public BaseRepository(HttpClient client)
        {
            this.client = client;
        }
        protected async Task<T> Get<T>(string url, string token = null)
        {           
            return await SendRequest<T>(url, (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<T>(), token);
        }

        protected async Task<TOut> PostJson<TIn, TOut>(string url, TIn obj, string token = null)
        {          
            return await SendRequest<TIn, TOut>(url, obj, (client, url, obj) => client.PostAsJsonAsync<TIn>(url, obj), message => message.Content.ReadAsAsync<TOut>(), token);
        }
        protected async Task<TOut> Post<TOut>(string url, HttpContent obj = null, string token = null)
        {          
            return await SendRequest(url, obj, (client, url, obj) => client.PostAsync(url, obj), message => message.Content.ReadAsAsync<TOut>(), token);
        }

        protected async Task<string> PostImage(string url, HttpContent obj, string token = null)
        {          
            return await SendRequest<HttpContent, string>(url, obj,(client,url,obj) => client.PostAsync(url,obj),message => message.Content.ReadAsStringAsync(), token);
        }

        protected async Task<int> Put<T>(string url, T obj, string token = null)
        {           
            return await SendRequest<T, int>(url, obj, (client, url, obj) => client.PutAsJsonAsync<T>(url, obj), message => message.Content.ReadAsAsync<int>(), token);
        }
        protected async Task<int> Delete(string url, string token = null)
        {           
            return await SendRequest<int>(url, (client,url) => client.DeleteAsync(url),message => message.Content.ReadAsAsync<int>(), token);            
        }

        private async Task<TOUt> SendRequest<TIn, TOUt>(string url, TIn obj, Func<HttpClient, string, TIn, Task<HttpResponseMessage>> sendRequest, Func<HttpResponseMessage, Task<TOUt>> getResponseResult, string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await sendRequest.Invoke(client, url, obj);
            return await GetResponse(getResponseResult, message);
        }

        private async Task<TOUt> GetResponse<TOUt>(Func<HttpResponseMessage, Task<TOUt>> getResponseResult, HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
            {
                return await getResponseResult.Invoke(message);
            }
            return default(TOUt);
        }

        private async Task<TOUt> SendRequest<TOUt>(string url, Func<HttpClient, string,  Task<HttpResponseMessage>> sendRequest, Func<HttpResponseMessage, Task<TOUt>> getResponseResult, string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await sendRequest.Invoke(client, url);
            return await GetResponse(getResponseResult, message);           
        }
    }
}
