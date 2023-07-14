using CoreObject.DTO;
using CoreObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Interfaces
{
    public interface IUserService
    {

        public Task<UserDisplayInfoDTO> Register(UserRegDTO user);  
        public Task<UserDisplayInfoDTO> Login(UserLoginDTO newUser);
        public Task<UserDisplayInfoDTO> CurrentUser();
        public Task<BaseResponse<string>> LogOut();

        public Task<bool> CurrentUserCheck();




        public Task<BaseResponse<string>> CreatePhone(AddPhoneDto phone);
       public Task<Phone> GetPhone(int id);
       public Task<List<Phone>> GetPhones();
       public Task<BaseResponse<Phone>> UpdatePhone(UpdatePhoneDto updatePhone);
       public Task<BaseResponse<string>> SoftDeletePhone(int id);  
       public Task<BaseResponse<string>> HradDeletePhone(int id);
    }
}
