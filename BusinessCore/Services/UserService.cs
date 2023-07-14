using BusinessCore.Interfaces;
using CoreObject.DTO;
using CoreObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext _dbContext { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public UserService(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<UserDisplayInfoDTO> Register(UserRegDTO user)
        {
            //throw new NotImplementedException();
            var email = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (email != null)
            {
                throw new Exception("Email already exists");
            }

            var username = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == user.Username);

            if (username != null)
            {
                throw new Exception("Username already exists");
            }

            var newUser = new User
            {
                UserName = user.Username,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = user.Role

            };

            var token = _jwtGenerator.CreateToken(newUser);
            newUser.Token = token;

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                return new UserDisplayInfoDTO
                {
                    DisplayName = newUser.DisplayName,
                    Username = newUser.UserName,
                    Token = token
                };
            }

            else
            {
                throw new Exception("Problem creating user");
            }


        }

        public async Task<UserDisplayInfoDTO> Login(UserLoginDTO newUser)
        {
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user == null)
            {
                throw new ArgumentException("incorrect details");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, newUser.Password, false);

            var token = _jwtGenerator.CreateToken(user);
            if (result.Succeeded)
            {
                user.Token = token;

                await _userManager.UpdateAsync(user);
                await _dbContext.SaveChangesAsync();

                return new UserDisplayInfoDTO
                {
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Token = token
                };
            }
            else
            {
                throw new ArgumentException("incorrect details");
            }
        }

        public async Task<UserDisplayInfoDTO> CurrentUser()
        {
            var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByNameAsync(username);

            return new UserDisplayInfoDTO
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Token = _jwtGenerator.CreateToken(user)
            };
                
        }

        public async Task<bool> CurrentUserCheck()
        {
            var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (username == null) return false;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            if (string.IsNullOrEmpty(user.Token)) return false;

            return true;

        }

        public async Task<BaseResponse<string>> LogOut()
        {
            var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new ArgumentException("incorrect details");
            }
            user.Token = string.Empty;

            await _userManager.UpdateAsync(user);
            await _dbContext.SaveChangesAsync();

            return new BaseResponse<string>
            {
                Status = true,
                Message = "successful"
            };
        }



        






        public async Task<BaseResponse<string>> CreatePhone(AddPhoneDto phone)
        {
            var response = new BaseResponse<string>
            {
                Status = false,
                Message = "Failed",
                Data = string.Empty
            };


            try
            {
                if (phone is null) return response;

                if (await CurrentUserCheck() == false) return response;

                var userCheck = await _dbContext.Phones.SingleOrDefaultAsync(x => x.Model == phone.Model);

                if (userCheck != null)
                {
                    response.Message = "Phone already exist";
                    return response;
                }

                var newPhone = new Phone
                {

                  Model  = phone.Model,
                  Status = true

                };
                    
                 await _dbContext.Phones.AddAsync(newPhone);

                 var saveResponse = await _dbContext.SaveChangesAsync();

                if (saveResponse <= 0)
                {
                    response.Message = "Internal error, try again";
                    return response;
                }
                response.Status = true;
                response.Message = "Successful";
                response.Data = newPhone.Id.ToString();
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Phone> GetPhone(int id)
        {
            try
            {
                if (await CurrentUserCheck() == false) return null;
                var phone = await _dbContext.Phones.SingleOrDefaultAsync(x => x.Id == id && x.Status == true);
                return phone;
            }
            catch (Exception ex)
            {


                LogService.LogError("00", "UserService", "GetPhone", ex);

                return null;
            }
        }

        public async Task<List<Phone>> GetPhones()
        {
            try
            {
                if (await CurrentUserCheck() == false) return null;
                var users = await _dbContext.Phones.Where(x =>  x.Status == true).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {


                LogService.LogError("00", "UserService", "GetPhone", ex);

                return null;
            }
        }

        public async Task<BaseResponse<string>> HradDeletePhone(int id)
        {
            var response = new BaseResponse<string>
            {
                Status = false,
                Message = "Failed",
                Data = string.Empty
            };

            try
            {
                if (await CurrentUserCheck() == false) return response;
                var phone = await _dbContext.Phones.SingleOrDefaultAsync(x => x.Id == id);
                if (phone is null)
                {
                    response.Message = "Phone does not exist";
                    return response;
                }
                _dbContext.Phones.Remove(phone);
                var saveResponse = await _dbContext.SaveChangesAsync();

                if (saveResponse <= 0)
                {
                    response.Message = "Internal error, try again";
                    return response;
                }
                response.Status = true;
                response.Message = "Successful";
                return response;
            }
            catch (Exception ex)
            {


                LogService.LogError("00", "UserService", "GetPhone", ex);

                return null;
            }
        }

        public async Task<BaseResponse<string>> SoftDeletePhone(int id)
        {

            var response = new BaseResponse<string>
            {
                Status = false,
                Message = "Failed",
                Data = string.Empty
            };

            try
            {
                if (await CurrentUserCheck() == false) return response;
                var phone = await _dbContext.Phones.SingleOrDefaultAsync(x => x.Id == id);
                if (phone is null)
                {
                    response.Message = "Phone does not exist";
                    return response;
                }
                phone.Status = false;
                _dbContext.Phones.Update(phone);
                var saveResponse = await _dbContext.SaveChangesAsync();

                if (saveResponse <= 0)
                {
                    response.Message = "Internal error, try again";
                    return response;
                }
                response.Status = true;
                response.Message = "Successful";
                return response;
            }
            catch (Exception ex)
            {


                LogService.LogError("00", "UserService", "GetPhone", ex);

                return null;
            }
        }

        public async Task<BaseResponse<Phone>> UpdatePhone(UpdatePhoneDto updatePhone)
        {

            var response = new BaseResponse<Phone>
            {
                Status = false,
                Message = "Failed"
            };

            try
            {
                if (await CurrentUserCheck() == false) return response;
                var phone = await _dbContext.Phones.SingleOrDefaultAsync(x => x.Id == updatePhone.Id);
                if (phone is null)
                {
                    response.Message = "Phone does not exist";
                    return response;
                }

                phone.Model = updatePhone.Model;

                _dbContext.Phones.Update(phone);
                var saveResponse = await _dbContext.SaveChangesAsync();

                if (saveResponse <= 0)
                {
                    response.Message = "Internal error, try again";
                    return response;
                }
                response.Status = true;
                response.Message = "Successful";
                response.Data = phone;

                return response;
            }
            catch (Exception ex)
            {


                LogService.LogError("00", "UserService", "GetPhone", ex);

                return null;
            }
        }
        }


    }
}
