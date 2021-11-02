using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Interfaces;

namespace JustTradeIt.Software.API.Services.Implementations
{
    public class AccountService : IAccountRepository
    {
        private IUserRepository userRepository;

        private TokenService tokenService;
        private readonly IMapper _mapper;

        public AccountService(IUserRepository userRepository,TokenService tokenService,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this._mapper = mapper;
        }
        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            User user = this.userRepository.getUserByEmail(loginInputModel.Email);
            if (user == null)
                return null;
            var HashedPassword = this.HashUsingPbkdf2(loginInputModel.Password);
            if (user.hashedPassword != HashedPassword)
                return null;
            string token = this.tokenService.GenerateJwtToken(this._mapper.Map<UserDto>(user));
            System.Diagnostics.Debug.WriteLine(token);
            return this._mapper.Map<UserDto>(user);
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            System.Diagnostics.Debug.WriteLine("line 1");
            UserDto userDto= this.userRepository.CreateUser(inputModel);
            System.Diagnostics.Debug.WriteLine("line 2");
            this.tokenService.GenerateJwtToken(userDto);
            System.Diagnostics.Debug.WriteLine("line 3");
            return userDto;
        }


        public UserDto GetProfileInformation()
        {
            return this.userRepository.GetProfileInformation();
        }

        public void Logout(int tokenId)
        {
            throw new System.NotImplementedException();
        }

        public UserDto UpdateProfile(ProfileInputModel profile)
        {
            System.Diagnostics.Debug.WriteLine("eee");
            return this.userRepository.UpdateProfile(profile);
        }
        public string HashUsingPbkdf2(string password)
        {
            using var bytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String("CGYzqeN4plZekNC88Umm1Q=="), 10000, HashAlgorithmName.SHA256);
            var derivedRandomKey = bytes.GetBytes(32);
            var hash = Convert.ToBase64String(derivedRandomKey);
            return hash;
        }
    }
}