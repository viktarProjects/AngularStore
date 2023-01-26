using AngularStore.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Specifications
{
    public class OrderByPaymentIntentSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentSpecification(string paymnetIntentId)
            :base(o => o.PaymentIntentId == paymnetIntentId)
        {

        }
    }
}
