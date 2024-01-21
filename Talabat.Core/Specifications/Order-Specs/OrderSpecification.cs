using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggreation;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrderSpecification: BaseSpecification<Order>
    {
        public OrderSpecification(string email) 
            : base(o=>o.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDescending(o => o.OrderDate);
        }
        public OrderSpecification(string email,int id) 
            : base(o=>o.BuyerEmail == email && o.Id==id)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

        }
    }
}
