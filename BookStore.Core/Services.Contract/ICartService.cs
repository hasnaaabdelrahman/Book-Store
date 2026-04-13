using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Services.Contract
{
    public interface ICartService
    {
        public Task AddToCartAsync(Guid userId, Guid bookId, int quantity);
            public Task RemoveFromCart(Guid userId, Guid bookId);
            public Task ClearCart(Guid userId);
            public Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId);   
    }
}
