﻿using System.Net.Http.Headers;

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
            if(token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await client.GetAsync(url);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<T>();
            }
            return default(T);
        }

        protected async Task<int> Post<T>(string url, T obj, string token = null)
        {
            if(token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);            
            HttpResponseMessage message = await client.PostAsJsonAsync<T>(url, obj);
            if (message.IsSuccessStatusCode)
                return await message.Content.ReadAsAsync<int>();
            return -1;
        }
        protected async Task<int> Put<T>(string url,T obj, string token = null)
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
    }
}