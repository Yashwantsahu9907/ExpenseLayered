using ExpenseLayeredApi.Data;
using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.Entities;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseLayeredApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseResult<LoginResponseDto>> LoginUser(LoginDto dto)
    {
        try
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return new ResponseResult<LoginResponseDto>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Email and Password is required",
                    Data = null
                };
            }
            // Check user by email 
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (existingUser == null)
            {
                return new ResponseResult<LoginResponseDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "User Not Found",
                    Data = null
                };
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, existingUser.Password);
            if(!isPasswordValid)
            {
                return new ResponseResult<LoginResponseDto>
                {
                    StatusCode = 401,
                    IsSuccess = false,
                    Message = "Invalid Password",
                    Data = null
                };
            }
            // Generate Jwt Token
            var token = GetJwtToken(existingUser);
            return new ResponseResult<LoginResponseDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Login SuccessFully",
                Data = new LoginResponseDto
                {
                    Token = token,
                    Message = "User Exist"
                }
            };
        }
        catch (Exception )
        {
            return new ResponseResult<LoginResponseDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Something went Wrong",
                Data = null
            };
        }
    }



    // JWT Authentication
    private string GetJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("XFkfUQSsuasKXf9du1j6ulBeRELYTz2AmE5HVgnbXohflPZPIQGtzQAnmoaKMsIV"));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "ExpenseApi",
            audience: "ExpenseUser",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
