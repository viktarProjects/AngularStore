using AngularStore.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Specifications
{
    public class BasketWithItemsSpecefications : BaseSpecification<Basket>
    {
        public BasketWithItemsSpecefications(string buyerId) : base(b => b.BuyerId == buyerId)
        {
            AddThenInclude
                (q => q.Include(i => i.Items)
                          .ThenInclude(p => p.Product)
                          .ThenInclude(b => b.ProductBrand)
                        .Include(i => i.Items)
                           .ThenInclude(p => p.Product)
                           .ThenInclude(t => t.ProductType)
                );

        }
    }
}
