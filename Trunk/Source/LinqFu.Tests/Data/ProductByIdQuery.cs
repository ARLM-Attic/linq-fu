using System;
using System.Linq;

namespace LinqFu.Tests.Data
{
    public class ProductByIdQuery
    {
        private readonly IQueryable<Product> provider;

        public ProductByIdQuery(IQueryable<Product> provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");

            this.provider = provider;
        }

        public IQueryable<Product> CreateQuery(Int32 productId)
        {
            var query = from p in this.provider
                        where p.ProductID == 1
                        select p;

            return query;
        }
    }
}
