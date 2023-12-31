﻿using DianaWebApp.Models.Entity;

namespace DianaWebApp.Models
{
    public class Size : BaseAuditableEntity
    {
        public string Name { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
    }
}
