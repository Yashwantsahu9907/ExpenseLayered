//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace ExpenseLayeredApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RoleController : ControllerBase
//    {
//        [HttpGet("user")]
//        [Authorize(Roles = "User")]
//        public IActionResult UserOnly()
//        {
//            return Ok("You are user");
//        }

//        [HttpGet("admin")]
//        [Authorize(Roles ="Admin,superAdmin")]
//        public IActionResult AdminOnly()
//        {
//            return Ok("Your are admin");
//        }

//        [HttpGet("superAdmin")]
//        [Authorize(Roles ="SuperAdmin")]
//        public IActionResult SuperAdminOnly()
//        {
//            return Ok("You are Superadmin");
//        }

//    }
//}
