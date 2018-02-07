﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductAttributeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "AttributeId")]
        public int AttributeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AttributeName")]
        public string AttributeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ProductAttributeStatus IsActive { get; set; }
    }

    public class ProductAttributeEntry : DtoBase
    {
        public ProductAttributeEntry()
        {
            IsActive = ProductAttributeStatus.Active;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "AttributeName")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string AttributeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        //[Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ProductAttributeStatus IsActive { get; set; }

        public List<ProductAttributeOptionEntry> Options { get; set; }
    }
    public class ProductAttributeEditEntry : ProductAttributeEntry
    {
        public int? Index { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AttributeId")]
        public int? AttributeId { get; set; }

        public List<ProductAttributeOptionEditEntry> ExistedOptions { get; set; }

    }
}
