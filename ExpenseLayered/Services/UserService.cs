using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.Entities.Identity;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseLayeredApi.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    // Get All Users
    public async Task<ResponseResult<List<UserDto>>> GetAllUsers()
    {
        try
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var userList = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Gender = user.Gender,
                    Role = roles.FirstOrDefault()
                };
                userList.Add(userDto);
            }

            return new ResponseResult<List<UserDto>>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = " User Fetched successfully",
                Data = userList
            };
        }
        catch (Exception)
        {
            return new ResponseResult<List<UserDto>>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Something went wrong",
                Data = null
            };
        }
    }


    // Get User By Id
    public async Task<ResponseResult<UserDto>> GetUserById(int id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseResult<UserDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "User not found",
                    Data = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Role = roles.FirstOrDefault()
            };

            return new ResponseResult<UserDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "User Fetched successfully",
                Data = userDto
            };
        }
        catch (Exception)
        {
            return new ResponseResult<UserDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Something went wrong",
                Data = null
            };
        }
    }


    // Create User
    public async Task<ResponseResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        try
        {
            if (dto == null)
            {
                return new ResponseResult<UserDto>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Please fill all the details",
                    Data = null
                };
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return new ResponseResult<UserDto>
                {
                    StatusCode = 409,
                    IsSuccess = false,
                    Message = "User already exist with this email id",
                    Data = null
                };
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                Gender = dto.Gender,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                return new ResponseResult<UserDto>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = errors,
                    Data = null
                };
            }
            // Assign Role
            var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(",", roleResult.Errors.Select(x => x.Description));
                return new ResponseResult<UserDto>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = errors,
                    Data = null
                };
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Role = dto.Role
            };

            return new ResponseResult<UserDto>
            {
                StatusCode = 201,
                IsSuccess = true,
                Message = "User created succesfully",
                Data = userDto
            };
        }
        catch (Exception)
        {
            return new ResponseResult<UserDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Something went wrong",
                Data = null
            };
        }
    }


    // Update User
    public async Task<ResponseResult<UserDto>> UpdateUser(int id, UpdateUserDto dto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ResponseResult<UserDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "User not found",
                    Data = null
                };
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.Gender = dto.Gender;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(x => x.Description));
                return new ResponseResult<UserDto>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = errors,
                    Data = null
                };
            }

            // Update Role
            var existingRoles = await _userManager.GetRolesAsync(user);
            if (existingRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, existingRoles);
            }

            await _userManager.AddToRoleAsync(user, dto.Role);
            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                Role = dto.Role
            };

            return new ResponseResult<UserDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "User updated successfully",
                Data = userDto
            };
        }
        catch (Exception)
        {
            return new ResponseResult<UserDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Something went wrong",
                Data = null
            };
        }
    }


    // Delete User
    public async Task<ResponseResult<bool>> DeleteUser(int id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ResponseResult<bool>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "User not found",
                    Data = false
                };
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description));

                return new ResponseResult<bool>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = errors,
                    Data = false
                };
            }

            return new ResponseResult<bool>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "User deleted successfully",
                Data = true
            };
        }
        catch (Exception)
        {
            return new ResponseResult<bool>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Something went wrong",
                Data = false
            };
        }
    }
}