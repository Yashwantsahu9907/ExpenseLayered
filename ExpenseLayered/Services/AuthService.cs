using ExpenseLayeredApi.Constant;
using ExpenseLayeredApi.Data;
using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.Entities.Identity;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseLayeredApi.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    public AuthService(UserManager<User> userManager, IConfiguration configuration  )
    {
        _userManager = userManager;
        _configuration = configuration;
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
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
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
            // check password using identity
            var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, dto.Password);
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
            var token = await GetJwtToken(existingUser);
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
    private async Task<string> GetJwtToken(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new[]  // Array(implicity type)
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "")
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // REGISTER 
    public async Task<ResponseResult<User>> RegisterUser(RegisterDto dto)
    {
        try
        {
            if(dto == null)
            {
                return new ResponseResult<User>
                {
                    StatusCode = 400,
                    IsSuccess=false,
                    Message = "Fill All the details",
                    Data= null
                };

            }
            var existing = await _userManager.Users.AnyAsync(x => x.Email == dto.Email);
            if(existing)
            {
                return new ResponseResult<User>
                {
                    StatusCode = 401,
                    IsSuccess = false,
                    Message = "User is already exist with this email",
                    Data = null
                };
            }

            // create user Object
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Gender = dto.Gender,
                UserName = dto.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            
            // Identity yaha User Create kar raha hai aur password hash kar raha hai 
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                return new ResponseResult<User>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = errors,
                    Data = null
                };
            }
            //assign default user role
            var roleResult = await _userManager.AddToRoleAsync(user, RoleConstant.User);
            if(!roleResult.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return new ResponseResult<User>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = errors,
                    Data = null
                };
            }
            return new ResponseResult<User>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Register Successfully",
                Data = null
            };
            
        }
        catch (Exception)
        {
            return new ResponseResult<User>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Something went wrong"
            };
        }
    }
}
