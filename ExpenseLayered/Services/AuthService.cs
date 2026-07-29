using ExpenseLayeredApi.Data;
using ExpenseLayeredApi.IServices;

namespace ExpenseLayeredApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    
}
