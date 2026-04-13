using BookStore.Core.Entities;
using BookStore.Core.Repositories.Contract;
using BookStore.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddToCartAsync(Guid userId, Guid bookId, int quantity)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetByIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                 _unitOfWork.Repository<Cart>().AddAsync(cart);
            }

            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItem>();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    BookId = bookId,
                    Quantity = quantity,
                    CartId = cart.Id 
                };

                cart.CartItems.Add(newCartItem);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task ClearCart(Guid userId)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetByIdAsync(userId);
            if (cart != null)
            {
                 _unitOfWork.Repository<Cart>().DeleteAsync(cart);
            }
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetByIdAsync(userId);
            return cart?.CartItems ?? Enumerable.Empty<CartItem>();
        }

        public async Task RemoveFromCart(Guid userId, Guid bookId)
        {
            var item = await _unitOfWork.Repository<CartItem>().GetByIdAsync(bookId);
            var cart = await _unitOfWork.Repository<Cart>().GetByIdAsync(userId);
            if (cart == null)
            {
                throw new Exception("Cart not found.");
            }

            if (item == null)
            {
                throw new Exception("Book item not found.");
            }
            _unitOfWork.Repository<CartItem>().DeleteAsync(item);
             _unitOfWork.SaveAsync();
        }
    }
}
