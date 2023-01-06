using Microsoft.AspNetCore.Mvc;

namespace AngularStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public string GetProducts()
        {
            return "It will be list of products";
        }

        [HttpGet("{id}")]
        public string GetProduct(int id)
        {
            return $"It will be one product № {id}";
        }
    }
}
