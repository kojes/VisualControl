using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using VueExample.Services;

namespace VueExample.ServiceModels
{
    public class Password
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
        
        public Password(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }
        
    }

    
}
