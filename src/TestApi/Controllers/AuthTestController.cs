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
            return Ok("Get method successfully executed");
        }

        [HttpGet]

        [Route("{id:int}")]

        public IActionResult Get(int id)
        {
            return Ok("Get by id method successfully executed");
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, USER1, USER2")]
        public IActionResult Post()
        {
            return Ok("Post method successfully executed");
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Put()
        {
            return Ok("Put method successfully executed");
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete()
        {
            return Ok("Delete method successfully executed");
        }
    }
}
