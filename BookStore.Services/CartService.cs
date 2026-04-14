using BookStore.Core.Entities;
using BookStore.Core.Repositories.Contract;
using BookStore.Core.Services.Contract;
using BookStore.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public CartService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task AddToCartAsync(Guid userId, Guid bookId, int quantity)
        {
          var cart = await _context.carts
                                     .Include(c => c.CartItems)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()  
                };
                 _unitOfWork.Repository<Cart>().AddAsync(cart);
                await _unitOfWork.SaveAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                var book = await _unitOfWork.Repository<Book>().GetByIdAsync(bookId);
                if (book == null)
                    throw new Exception("Book not found.");

                var newCartItem = new CartItem
                {
                    BookId = bookId,
                    Quantity = quantity,
                    CartId = cart.Id
                };
                 _unitOfWork.Repository<CartItem>().AddAsync(newCartItem);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task ClearCart(Guid userId)
        {
            var cart = await _context.carts
                                     .Include(c => c.CartItems)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                throw new Exception("Cart not found.");

            _context.Set<CartItem>().RemoveRange(cart.CartItems);
            _unitOfWork.Repository<Cart>().DeleteAsync(cart);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(Guid userId)
        {
            var cart = await _context.carts
                                     .Include(c => c.CartItems)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);

            return cart?.CartItems ?? Enumerable.Empty<CartItem>();
        }

        public async Task RemoveFromCart(Guid userId, Guid bookId)
        {
            var cart = await _context.carts
                                     .Include(c => c.CartItems)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new Exception("Cart not found.");

            var item = cart.CartItems.FirstOrDefault(ci => ci.BookId == bookId);
            if (item == null)
                throw new Exception("Book item not found.");

            _unitOfWork.Repository<CartItem>().DeleteAsync(item);
            await _unitOfWork.SaveAsync(); 
        }
    }
}