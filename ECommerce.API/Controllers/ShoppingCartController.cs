using AutoMapper;
using ECommerce.Entities;
using ECommerce.Services.Abstract;
using ECommerce.Services.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService<ShoppingCart> _shoppingCartService;
        private readonly IMapper _mapper; // AutoMapper 

        public ShoppingCartController(IShoppingCartService<ShoppingCart> shoppingCartService, IMapper mapper)
        {
            _shoppingCartService = shoppingCartService;
            _mapper = mapper;
        }

        [HttpGet, Route("[action]/{userId}")]
        public async Task<IActionResult> GetShoppingCartByUserId(string userId)
        {
            var cart = await _shoppingCartService.GetShoppingCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }

            var cartDto = _mapper.Map<ShoppingCartDTO>(cart);
            return Ok(cartDto);
        }

        [HttpGet, Route("[action]/{userId}")]
        public async Task<IActionResult> GetTotalAmount(string userId)
        {
            var cart = await _shoppingCartService.GetShoppingCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }

            var totalAmount = _shoppingCartService.CalculateTotalAmount(cart);
            return Ok(new { TotalAmount = totalAmount });
        }

        [HttpPost, Route("[action]")]
        public IActionResult CreateShoppingCart([FromBody] ShoppingCartDTO shoppingCartDto)
        {
            #region Example Create API Request Body
            //  {
            //    "userId": "1",
            //    "shoppingCartItems": [
            //      {
            //        "productId": 1,
            //        "quantity": 2
            //      }
            //    ]
            //  }
            #endregion

            if (shoppingCartDto == null)
            {
                return BadRequest("Shopping cart data is null.");
            }

            var shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartDto);
            var createdCart = _shoppingCartService.CreateShoppingCart(shoppingCart);

            var createdCartDto = _mapper.Map<ShoppingCartDTO>(createdCart);
            return CreatedAtAction(nameof(GetShoppingCartByUserId), new { userId = createdCartDto.UserId }, createdCartDto);
        }

        [HttpPut, Route("[action]/{shoppingCartId}")]
        public IActionResult UpdateShoppingCart(int shoppingCartId, [FromBody] ShoppingCartDTO shoppingCartDto)
        {
            #region Example Update API Request Body
            //  {
            //     "id": 1,
            //     "userId": "1",
            //     "shoppingCartItems": [
            //       {
            //         "id": 1,
            //         "shoppingCartId": 1,
            //         "productId": 1,
            //         "quantity": 2
            //       },
            //      {
            //       "id": 2,
            //       "shoppingCartId": 1,
            //     "productId": 2,
            //     "quantity": 1
            //      }
            //    ]
            //  } 
            #endregion

            if (shoppingCartDto == null || shoppingCartId != shoppingCartDto.Id)
            {
                return BadRequest();
            }

            var shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartDto);
            _shoppingCartService.UpdateShoppingCart(shoppingCart);
            return NoContent();
        }

        [HttpDelete, Route("[action]/{shoppingCartId}")]
        public IActionResult DeleteShoppingCart(int shoppingCartId)
        {
            _shoppingCartService.DeleteShoppingCart(shoppingCartId);
            return NoContent();
        }
    }
}
