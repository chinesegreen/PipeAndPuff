using Ardalis.Specification;
using Core.Entities.OrderAggregate;

namespace Core.Specifications;

public class OrderWithItemsByIdSpec : Specification<Order>, ISingleResultSpecification
{
    public OrderWithItemsByIdSpec(int orderId)
    {
        Query
            .Where(order => order.Id == orderId)
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.ItemOrdered);
    }
}
