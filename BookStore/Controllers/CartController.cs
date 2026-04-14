using BookStore.Core.Entities;
using BookStore.Core.Services.Contract;
using BookStore.Dtos.Incoming;
using BookStore.Dtos.outgoingDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        private Guid GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                        ?? User.FindFirst("sub");

            if (claim == null || !Guid.TryParse(claim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid token.");

            return userId;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems()
        {
            var userId = GetUserId();
            var cartItems = await _cartService.GetCartItemsAsync(userId);
            return Ok(cartItems);
        }
        [HttpPost("add")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            var userId = GetUserId();
            await _cartService.AddToCartAsync(userId, addToCartDto.bookId, addToCartDto.quantity);
            return Ok();
        }
        [Authorize(Roles = "User")]
        [HttpDelete("remove/{bookId}")]
        public async Task<ActionResult> RemoveFromCart(Guid bookId)
        {
            var userId = GetUserId();
            await _cartService.RemoveFromCart(userId, bookId);
            return Ok();
        }
        [HttpDelete("clear")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearCart(userId);
            return Ok();
        }
    }
}
