using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using User.Manager.API.Models;

namespace User.Manager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WishlistController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] WishlistitemDto item)
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value!;
            var user = await _userManager.FindByNameAsync(username);
            if (item is null)
            {
                return BadRequest("Wishlist item is null");
            }
            var wishlist = _context.WishLists.FirstOrDefault(w => w.UserId == user.Id);
            if (wishlist == null)
            {
                wishlist = new WishList()
                {
                    UserId = user.Id,
                };
                await _context.WishLists.AddAsync(wishlist);
                await _context.SaveChangesAsync();
                var Wishlistitem = new WishListItem()
                {
                    ProductId = item.ProductId,
                    Amount = item.Amount,
                    WishlistId = wishlist.Id
                };
                await _context.WishListItems.AddAsync(Wishlistitem);
                await _context.SaveChangesAsync();
                return Created(username, wishlist);
            }
            else
            {
                WishListItem wishlistitem = await _context.WishListItems.FirstOrDefaultAsync(w => w.WishlistId == wishlist.Id && w.ProductId == item.ProductId);
                if (wishlistitem == null)
                {
                    wishlistitem = new WishListItem()
                    {
                        ProductId = item.ProductId,
                        Amount = item.Amount,
                        WishlistId = wishlist.Id
                    };
                    await _context.WishListItems.AddAsync(wishlistitem);
                    await _context.SaveChangesAsync();
                }
                wishlistitem.Amount += item.Amount;
                _context.WishListItems.Update(wishlistitem);
                await _context.SaveChangesAsync();
                return Created(username, wishlist);
            }

        }
        [HttpGet]
        public async Task<IActionResult> ShowWishlist()
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value!;
            var user = await _userManager.FindByNameAsync(username);

            var wishlist = _context.WishLists.Include(w => w.WishListItems).Where(x => x.UserId == user.Id);
            return Ok(wishlist);

        }
    }
}
