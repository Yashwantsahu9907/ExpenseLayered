using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.IServices;
using ExpenseLayeredApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseLayeredApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        // Constructor Injection
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [Authorize]
        [HttpPost("CreateExpense")]
        public async Task<IActionResult> CreateExpense([FromBody]ExpenseDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);  // generate token match and store
            if (userIdClaim == null)
            {
                return Unauthorized("UserId claim not found.");
            }
            int userId = int.Parse(userIdClaim.Value);
            var result = await _expenseService.CreateExpense(dto, userId);
            return Ok(result);
        }
    }
}
