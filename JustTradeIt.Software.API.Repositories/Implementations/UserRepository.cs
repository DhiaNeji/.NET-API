using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using AutoMapper;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Repositories.Interfaces;

namespace JustTradeIt.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        JustTradeItContext context;
        private readonly IMapper _mapper;

        public UserRepository(JustTradeItContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }
        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            throw new NotImplementedException();
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            if(!this.findByEmail(inputModel.Email))
            {
                string hashedPassword = this.HashUsingPbkdf2(inputModel.Password);
                //Add the Auth new Token and save it to database
                User newUser = new User(null, inputModel.FullName, inputModel.Email, null, hashedPassword);
                this.context.User.Add(newUser);
                this.context.SaveChanges();
                return this._mapper.Map<UserDto>(newUser); ;
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Email existing", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.BadRequest
                };
                throw new HttpResponseException(response);
            }
        }

        public UserDto GetProfileInformation()
        {
            User user = context.User.Where(u => u.Id == 1).First(); // To be implemented with JWT
            System.Diagnostics.Debug.WriteLine(this.context.User.Count());
            UserDto userDto = this._mapper.Map<UserDto>(user);
            return userDto;
        }

        public UserDto GetUserInformation(string userIdentifier)
        {
             User user= this.context.User.FirstOrDefault(x => x.Id == int.Parse(userIdentifier));
            return this._mapper.Map<UserDto>(user);
        }

        public UserDto UpdateProfile(ProfileInputModel profile)
        {
            User user = context.User.Where(u => u.Id == 1).First();
            // To be implemented with JWT
            user.FullName = profile.FullName;
            // user.ProfileImageUrl = profile.ProfileImage; //To be implemented with Image implementation
            this.context.SaveChanges();
            return this._mapper.Map<UserDto>(user);
        }
        

        public bool findByEmail(string email)
        {
            return this.context.User.Any(u => u.Email == email);
        }

        public User getUserByEmail(string email)
        {
            return this.context.User.Where(u => u.Email.Equals(email)).First();
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