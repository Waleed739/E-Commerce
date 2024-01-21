using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggreation;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecifications(string intentId):
            base(o=>o.PaymentIntentId== intentId)
        {
                
        }
    }
}
