﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Webx.Web.Data.Entities;
using Webx.Web.Models;
using Webx.Web.Models.AdminPanel;

namespace Webx.Web.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task AddCategoryAsync(CategoryViewModel model);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetAllCategoriesByIdAsync(int id);
        Task<Category> GetAllCategoryByNameAsync(string name);
        Task<List<ChartCategoriesViewModel>> GetMostSoldCategoriesData();
    }
}
