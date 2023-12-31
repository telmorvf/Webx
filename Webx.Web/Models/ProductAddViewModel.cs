﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Syncfusion.EJ2.Spreadsheet;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Webx.Web.Data.Entities;

namespace Webx.Web.Models
{
    public class ProductAddViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The field {0} can contain {1} characteres lenght")]  
        public string Name { get; set; }

        public string Description { get; set; }

        // não usa este formato em modo de edição
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, 99999.99, ErrorMessage = "Only greater than Zero")]
        public decimal Price { get; set; }

        //[MinLength(0, ErrorMessage = "Only greater than Zero")]

        [Display(Name = "Is Service?")]
        public bool IsService { get; set; }

        [Display(Name = "Promotion")]
        public bool IsPromotion { get; set; }

        [Required]
        [Display(Name = "Brand")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a brand.")]
        public string BrandId { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }


        [Required]
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a category.")]
        public string CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }


        [Display(Name = "Minimum Quantity Alert?")]
        
        [Range(1, int.MaxValue, ErrorMessage = "Only greater than Zero")]
        public int MinimumQuantity { get; set; }

        [Display(Name = "Quantity Received?")]
        [Range(0, int.MaxValue, ErrorMessage = "Only Zero or greater than Zero")]
        public int ReceivedQuantity { get; set; }



        [Display(Name = "Picture File")]
        public FormFile PictureFile { get; set; }


        [Display(Name = "Upload Files")]
        public IList<IFormFile> UploadFiles { get; set; }


        public IEnumerable<ProductImages> Images { get; set; }
        public Images ImagesId { get; set; }

        public string ImageFirst { get; set; }

        [Required]
        [Display(Name = "Discount(%)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal Discount { get; set; }

        [Display(Name = "Price with Discount")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal PriceWithDiscount => Price * (1 - (Discount / 100));

    }
}
