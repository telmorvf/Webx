﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using PayPalCheckoutSdk.Orders;
using Syncfusion.EJ2.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Webx.Web.Data.Entities;
using Webx.Web.Helpers;
using Webx.Web.Models;

namespace Webx.Web.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ProductRepository(DataContext context, IHttpContextAccessor httpContext) : base(context)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public IEnumerable<SelectListItem> GetBrandsCombo()
        {
            var list = _context.Brands.Select(b => new SelectListItem
            {
                Text = b.Name,
                Value = b.Id.ToString()

            }).OrderBy(b => b.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a brand...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetBrandsCombo(int brandId)
        {
            var brand = _context.Brands.Find(brandId);
            var list = new List<SelectListItem>();
            if (brand != null)
            {
                list = _context.Brands.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()

                }).OrderBy(l => l.Text).ToList();


                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a brand...)",
                    Value = "0"
                });

            }

            return list;
        }


        public IEnumerable<SelectListItem> GetCategoriesCombo()
        {
            var list = _context.Categories.Select(b => new SelectListItem
            {
                Text = b.Name,
                Value = b.Id.ToString()

            }).OrderBy(b => b.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a category...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetCategoriesCombo(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            var list = new List<SelectListItem>();
            if (category != null)
            {
                list = _context.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()

                }).OrderBy(l => l.Text).ToList();


                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a category...)",
                    Value = "0"
                });

            }

            return list;
        }


        public async Task<Product> GetFullProduct(int id)
        {
            var product =
                await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Where(p => p.Id == id)
                .OrderBy(p => p.Id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
            }
            return product;
        }

#nullable enable
        public async Task<IEnumerable<Product>> GetFullProducts(string? category)
        {
            //IEnumerable<Product> productAll;
            List<Product> list = new List<Product>();

            if (category == "AllCategories")
            {

                list =
                    await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Include(p => p.Brand)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            else
            {
                list =
                    await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Include(p => p.Brand)
                    .Where(c => c.Category.Name == category)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }

            return list;
        }

        public async Task<List<Product>> GetFilteredProducts(string category, List<string> brandsFilter)
        {
            //IEnumerable<Product> productAll;
            List<Product> list = new List<Product>();

            if (category == "AllCategories")
            {

                list =
                    await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Include(p => p.Brand)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            else
            {
                list =
                    await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Images)
                    .Include(p => p.Brand)
                    .Where(c => c.Category.Name == category)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }

            if (brandsFilter != null && brandsFilter.Count > 0)
            {
                List<Product> filteredlist = new List<Product>();


                foreach (var filter in brandsFilter)
                {
                    filteredlist.AddRange(list.Where(p => p.Brand.HtmlId == filter).ToList());
                }

                return filteredlist;
            }

            return list;
        }


        public async Task<decimal> MostExpensiveProductPriceAsync()
        {
            var Product = await _context.Products.OrderByDescending(p => p.Price).FirstAsync();

            return Product.Price;
        }

        public async Task<IEnumerable<Product>> GetAllProductsControllerAsync()
        {
            IEnumerable<Product> productAll;

            productAll = await _context.Products
                .OrderBy(p => p.Id)
                .ToListAsync();

            return productAll;
        }

        public async Task<IEnumerable<Product>> GetProductAllAsync()
        {
            IEnumerable<Product> productAll;

            productAll = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Where(p => p.IsService == false)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return productAll;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product =
                await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Brand)
                .Where(p => p.Id == id && p.IsService == false)
                .OrderBy(p => p.Name)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
            }

            return product;


        }


        public async Task<Product> GetProSerByIdAsync(int id)
        {
            var product =
                await _context.Products
                .Include(p => p.Category)
                //.Include(p => p.Images)
                .Include(p => p.Brand)
                .Where(p => p.Id == id)
                .OrderBy(p => p.Name)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetServiceAllAsync()
        {
            IEnumerable<Product> productAll;

            productAll = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.IsService == true)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return productAll;
        }



        public async Task<Product> GetServiceByIdAsync(int id)
        {
            var product =
                await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.Id == id && p.IsService == true)
                .OrderBy(p => p.Name)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
            }
            return product;
        }
        public async Task<Product> GetServiceByNameAsync(string name)
        {
            return await _context.Products.SingleOrDefaultAsync(b => b.Name == name);
        }

        public async Task<ShopViewModel> GetInitialShopViewModelAsync()
        {
            var cookie = _httpContext.HttpContext.Request.Cookies["Cart"];
            List<CartViewModel> cart = new List<CartViewModel>();


            if (string.IsNullOrEmpty(cookie))
            {
                List<CookieItemModel> cookieItemList = new List<CookieItemModel>();
                var serializedCart = JsonConvert.SerializeObject(cookieItemList);
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.UtcNow.AddDays(365);
                options.Secure = true;
                _httpContext.HttpContext.Response.Cookies.Append("Cart", serializedCart, options);
            }
            else
            {
                var cookieItemList = JsonConvert.DeserializeObject<List<CookieItemModel>>(cookie);
                foreach (var item in cookieItemList)
                {
                    var color = await GetStockColor(item.ProductId, item.StoreId);
                    var product = await GetFullProduct(item.ProductId);
                    cart.Add(new CartViewModel { Product = product, Quantity = item.Quantity, StoreId = item.StoreId, Color = color });
                }
            }

            return new ShopViewModel
            {
                Cart = cart,
            };
        }

        public async Task<List<CartViewModel>> GetCurrentCartAsync()
        {
            List<CartViewModel> cart = new List<CartViewModel>();

            var cookie = _httpContext.HttpContext.Request.Cookies["Cart"];
            var cookieItemList = JsonConvert.DeserializeObject<List<CookieItemModel>>(cookie);

            if (cookieItemList != null && cookieItemList.Count() > 0)
            {
                foreach (var item in cookieItemList)
                {
                    var color = await GetStockColor(item.ProductId, item.StoreId);
                    var product = await GetFullProduct(item.ProductId);
                    cart.Add(new CartViewModel { Product = product, Quantity = item.Quantity, StoreId = item.StoreId, Color = color });
                }
            }

            return cart;

        }

        private async Task<string> GetStockColor(int productId, int storeId)
        {
            var product = await _context.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
            var color = "";

            if (product.IsService)
            {
                color = "Green";
                return color;
            }
            else
            {
                var stock = await _context.Stocks.Where(s => s.Product.Id == productId && s.Store.Id == storeId).FirstOrDefaultAsync();
                var productTotal = stock.Quantity;

                if (productTotal == 0)
                {
                    color = "#3F3F3F";
                }

                if (productTotal > 0 && productTotal < 10)
                {
                    color = "Red";
                }

                if (productTotal >= 10 && productTotal <= 25)
                {
                    color = "#ffb703";
                }

                if (productTotal > 25)
                {
                    color = "Green";
                }

                return color;
            }
        }


        public bool CheckCookieConsentStatus()
        {
            var cookieConsent = _httpContext.HttpContext.Request.Cookies["Consent"];
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.UtcNow.AddDays(365);
            options.Secure = true;

            if (string.IsNullOrEmpty(cookieConsent))
            {
                _httpContext.HttpContext.Response.Cookies.Append("Consent", "false", options);
                return false;
            }
            else
            {
                _httpContext.HttpContext.Response.Cookies.Append("Consent", "true", options);
                return true;
            }

        }

        public Response UpdateCartCookie(List<CartViewModel> cart)
        {
            try
            {
                var cookie = _httpContext.HttpContext.Request.Cookies["Cart"];

                List<CookieItemModel> cookieItemList = new List<CookieItemModel>();
                foreach (var item in cart)
                {
                    cookieItemList.Add(new CookieItemModel { ProductId = item.Product.Id, Quantity = item.Quantity, StoreId = item.StoreId });
                }

                var serializedCart = JsonConvert.SerializeObject(cookieItemList);
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.UtcNow.AddDays(365);
                options.Secure = true;
                _httpContext.HttpContext.Response.Cookies.Append("Cart", serializedCart, options);

                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {

                return new Response { IsSuccess = false, Message = ex.Message };
            }
        }

        public Response ClearCart()
        {
            _httpContext.HttpContext.Response.Cookies.Delete("Cart");

            try
            {
                List<CookieItemModel> cookieItemList = new List<CookieItemModel>();
                var serializedCart = JsonConvert.SerializeObject(cookieItemList);
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.UtcNow.AddDays(365);
                options.Secure = true;
                _httpContext.HttpContext.Response.Cookies.Append("Cart", serializedCart, options);
                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new Response { IsSuccess = false, Message = ex.Message };
            }

        }

        public async Task<List<APIProductsModel>> GetProductsAsync()
        {

            List<APIProductsModel> list = new List<APIProductsModel>();

            var products = await _context.Products
                 .Include(p => p.Brand)
                 .Include(p => p.Images)
                 .Include(p => p.Category).Where(p => p.IsService == false).ToListAsync();

            foreach (var product in products)
            {
                var stock = await _context.Stocks.Where(s => s.ProductId == product.Id && s.StoreId == 3).FirstOrDefaultAsync();

                list.Add(new APIProductsModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Brand = product.Brand.Name,
                    Category = product.Category.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = stock.Quantity,
                    MinimumQuantity = stock.MinimumQuantity,
                    ImageId = product.Images.ElementAt(0).ImageId,
                });
            }

            return list;
        }


        public async Task<Product> GetProductByNameAsync(string productName)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.Name == productName)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetAllProducts(string category)
        {
            List<Product> list = new List<Product>();

            if (category == "AllCategories")
            {
                list = await _context.Products.Include(p => p.Images).Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            }
            else
            {
                list = await _context.Products.Include(p => p.Images).Include(p => p.Brand).Include(p => p.Category).Where(p => p.Category.Name == category).ToListAsync();
            }

            return list;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Brand).Include(p => p.Category).ToListAsync();
        }

        public async Task<List<Product>> GetHighlightedProductsAsync()
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Brand).Include(p => p.Category).Where(p => p.IsPromotion == true).ToListAsync();
        }

        public async Task<List<Product>> GetOrStartWishListAsync()
        {
            var wishlistCookie = _httpContext.HttpContext.Request.Cookies["Wishlist"];
            List<int> wishlist = new List<int>();
            List<Product> Products = new List<Product>();

            if (string.IsNullOrEmpty(wishlistCookie))
            {
                var serializedList = JsonConvert.SerializeObject(wishlist);
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.UtcNow.AddDays(365);
                options.Secure = true;
                _httpContext.HttpContext.Response.Cookies.Append("Wishlist", serializedList, options);
            }
            else
            {
                wishlist = JsonConvert.DeserializeObject<List<int>>(wishlistCookie);
                foreach (int number in wishlist)
                {
                    Products.Add(await GetFullProduct(number));
                }
            }

            return Products;
        }

        public Response AddProductToWishList(Product product)
        {
            try
            {
                var wishlistCookie = _httpContext.HttpContext.Request.Cookies["Wishlist"];
                //List<int> wishlist = new List<int>();
                var wishlist = JsonConvert.DeserializeObject<List<int>>(wishlistCookie);
                wishlist.Add(product.Id);

                var serializedList = JsonConvert.SerializeObject(wishlist);
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.UtcNow.AddDays(365);
                options.Secure = true;
                _httpContext.HttpContext.Response.Cookies.Append("Wishlist", serializedList, options);

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public Response UpdateWishlistCookie(List<Product> currentWishlist)
        {
            try
            {
                var cookie = _httpContext.HttpContext.Request.Cookies["Wishlist"];
                List<int> productsIds = new List<int>();
                foreach (var item in currentWishlist)
                {
                    productsIds.Add(item.Id);
                }

                var serializedList = JsonConvert.SerializeObject(productsIds);
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.UtcNow.AddDays(365);
                options.Secure = true;
                _httpContext.HttpContext.Response.Cookies.Append("Wishlist", serializedList, options);

                return new Response { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<ProductReview>> GetProductReviewsAsync(int productId)
        {
            return await _context.Reviews.Include(r => r.User).Include(r => r.Product).Where(r => r.Product.Id == productId).ToListAsync();
        }

        public async Task<ProductReview> GetThisCustomerProdReviewAsync(User user, Product product)
        {
            return await _context.Reviews.Include(r => r.User).Where(r => r.User.Id == user.Id && r.Product.Id == product.Id).FirstOrDefaultAsync();
        }

        public async Task CreateReviewAsync(ProductReview review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductReview> GetProductReviewByIdAsync(int value)
        {
            return await _context.Reviews.Include(r => r.User).Include(r => r.Product).ThenInclude(p => p.Images).Where(r => r.Id == value).FirstOrDefaultAsync();
        }

        public async Task UpdateReviewAsync(ProductReview customerReview)
        {
            _context.Reviews.Update(customerReview);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductReview>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product).ThenInclude(p => p.Images)
                .Include(r => r.Product).ThenInclude(p => p.Brand)
                .Include(r => r.Product).ThenInclude(p => p.Category)
                .ToListAsync();
        }

        public Task<ProductReview> GetRecentCreatedReviewAsync(ProductReview review)
        {
            return _context.Reviews.Where(r => r.User == review.User && r.Product == review.Product && r.ReviewDate == review.ReviewDate).FirstOrDefaultAsync();
        }

        public async Task CreateReviewTempAsync(ProductReview review)
        {
            ProductReviewTemps temp = new ProductReviewTemps
            {
                ProductReview = review
            };

            await _context.ReviewsTemps.AddAsync(temp);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveReviewTempIfExistsAsync(ProductReview customerReview)
        {
            var reviewTemp = await _context.ReviewsTemps.Where(rt => rt.ProductReview == customerReview).FirstOrDefaultAsync();

            if (reviewTemp != null)
            {
                _context.ReviewsTemps.Remove(reviewTemp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ProductReviewTemps>> GetReviewsTempsAsync()
        {
            return await _context.ReviewsTemps.Include(rt => rt.ProductReview).ToListAsync();
        }

        public async Task RemoveReviewTempsAsync(List<ProductReviewTemps> temps)
        {
            _context.ReviewsTemps.RemoveRange(temps);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetReviewsTempsCountAsync()
        {
            var temps = await _context.ReviewsTemps.AsNoTracking().ToListAsync();

            if (temps != null && temps.Count > 0)
            {
                return temps.Count;
            }

            return 0;


        }
    }
}
