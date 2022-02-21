﻿using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(HttpClient client) : base(client)
        {

        }
        public async Task<List<Category>> GetCategories()
        {
            return await Get<List<Category>>("/api/category");            
        }
        public async Task<Category> GetCategoryById(int id)
        {
            return await Get<Category>($"/api/category/{id}");            
        }
        public async Task<int> Create(Category category, string token)
        {
            return await PostJson<Category, int>("/api/category", category, token);            
        }
        public async Task<int> Edit(Category category, string token)
        {
            return await Put<Category>("/api/category", category, token);            
        }

        public async Task<int> Delete(int id, string token)
        {
            return await Delete($"/api/category/{id}", token);            
        }
    }
}
