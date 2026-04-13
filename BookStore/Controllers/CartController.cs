using BookStore.Core.Entities;
using BookStore.Core.Services.Contract;
using BookStore.Dtos.outgoingDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("{userId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems(Guid userId)
        {
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            return Ok(cartItems);
        }
        [HttpPost("add")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            await _cartService.AddToCartAsync(addToCartDto.userId, addToCartDto.bookId, addToCartDto.quantity);
            return Ok();
        }
        [HttpDelete("remove")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RemoveFromCart([FromBody] RemoveFromCartDto removeFromCartDto)
        {
            await _cartService.RemoveFromCart(removeFromCartDto.userId, removeFromCartDto.bookId);
            return Ok();
        }
        [HttpDelete("clear")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> ClearCart(Guid userId)
        {
            await _cartService.ClearCart(userId);
            return Ok();
        }
    }
}
