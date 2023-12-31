﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Webx.Web.Data.Entities;
using Webx.Web.Models;
using Webx.Web.Models.AdminPanel;

namespace Webx.Web.Data.Repositories
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<Brand> GetBrandByIdAsync(int id);
        Task<Brand> GetBrandByNameAsync(string name);
        Task<List<ChartBrandsViewModel>> GetBrandsChartDataAsync();
        Task<int> GetTotalBrandsSoldAsync();
        //Task<List<Brand>> GetAllBrandsAsync(); estava a retornar lista em vez de IEnumerable
    }
}
