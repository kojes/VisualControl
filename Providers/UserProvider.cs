using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VueExample.Contexts;
using VueExample.Helpers;
using VueExample.Models;

namespace VueExample.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly AppSettings _appSettings;
        private List<User> _usersList;

        public UserProvider(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _usersList = GetAll();
        }
        
        public User Authenticate(string userUsername, string userPassword)
        {
            throw new NotImplementedException();
        }

        private List<User> GetAll()
        {
            using (var visualControlContext = new VisualControlContext())
            {
                return visualControlContext.Users.ToList();
            }
        }
    }
}
