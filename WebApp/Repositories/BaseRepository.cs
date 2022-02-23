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
            //if (token != null)
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //HttpResponseMessage message = await client.GetAsync(url);
            //if (message.IsSuccessStatusCode)
            //{
            //    return await message.Content.ReadAsAsync<T>();
            //}
            //return default(T);
            return await SendRequest<T>(url, (clien, url) => clien.GetAsync(url), message => message.Content.ReadAsAsync<T>(), token);
        }

        protected async Task<TOut> PostJson<TIn, TOut>(string url, TIn obj, string token = null)
        {
            //if (token != null)
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //HttpResponseMessage message = await client.PostAsJsonAsync<TIn>(url, obj);
            //if (message.IsSuccessStatusCode)
            //{
            //    return await message.Content.ReadAsAsync<TOut>();
            //}
            //return default(TOut);
            return await SendRequest<TIn, TOut>(url, obj, (client, url, obj) => client.PostAsJsonAsync<TIn>(url, obj), message => message.Content.ReadAsAsync<TOut>(), token);
        }
        protected async Task<TOut> Post<TOut>(string url, HttpContent obj = null, string token = null)
        {
            //if (token != null)
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ////recheck this
            //HttpResponseMessage message = await client.PostAsync(url, obj);
            //if (message.IsSuccessStatusCode)
            //{
            //    return await message.Content.ReadAsAsync<TOut>();
            //}

            //return default(TOut);
            return await SendRequest(url, obj, (client, url, obj) => client.PostAsync(url, obj), message => message.Content.ReadAsAsync<TOut>(), token);

        }


        protected async Task<string> PostImage(string url, HttpContent obj, string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await client.PostAsync(url, obj);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsStringAsync();
            }

            return null;
        }

        protected async Task<int> Put<T>(string url, T obj, string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage message = await client.PutAsJsonAsync<T>(url, obj);
            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsAsync<int>();
            return -1;
        }
        protected async Task<int> Delete(string url, string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await client.DeleteAsync(url);
            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsAsync<int>();
            return -1;
        }

        private async Task<TOUt> SendRequest<TIn, TOUt>(string url, TIn obj, Func<HttpClient, string, TIn, Task<HttpResponseMessage>> sendRequest, Func<HttpResponseMessage, Task<TOUt>> getResponseResult, string token = null)
        {
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await sendRequest.Invoke(client, url, obj);
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
            if (message.IsSuccessStatusCode)
            {
                return await getResponseResult.Invoke(message);
            }            
            return default(TOUt);

        }
    }
}
