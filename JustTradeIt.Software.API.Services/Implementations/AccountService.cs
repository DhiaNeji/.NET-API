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

        private ITokenRepository tokenRepository;
        private ITokenService tokenService;
        private readonly IMapper _mapper;

        public AccountService(IUserRepository userRepository,TokenService tokenService,IMapper mapper,ITokenRepository tokenRepository)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this._mapper = mapper;
            this.tokenRepository = tokenRepository;
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
            return this._mapper.Map<UserDto>(this.userRepository.getUserByEmail(loginInputModel.Email));
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            UserDto userDto= this.userRepository.CreateUser(inputModel);
            this.tokenService.GenerateJwtToken(userDto);
            return userDto;
        }


        public UserDto GetProfileInformation(string email)
        {
            return this.userRepository.GetProfileInformation(email);
        }

        public void Logout(int tokenId)
        {
            this.tokenRepository.VoidToken(tokenId);
        }

        public UserDto UpdateProfile(string FullName, string email, string imgUrl)
        {
            return this.userRepository.UpdateProfile(FullName,email,imgUrl);
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