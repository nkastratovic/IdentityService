using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Get method");
        }

        [HttpGet]

        [Route("{id:int}")]

        public IActionResult Get(int id)
        {
            return Ok("Get by id method");
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, CUSTOMER")]
        public IActionResult Post()
        {
            return Ok("Post method");
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Put()
        {
            return Ok("Put method");
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete()
        {
            return Ok("Delete method");
        }
    }
}
