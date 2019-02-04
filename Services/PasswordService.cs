using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using VueExample.ServiceModels;

namespace VueExample.Services
{
    public class PasswordService
    {
        public bool IsPasswordValid(string passwordToValidate, string saltString, string correctPasswordHashString)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            byte[] correctPasswordHash = Convert.FromBase64String(correctPasswordHashString);
            byte[] hashedPassword = ComputePasswordHash(passwordToValidate, salt);

            return hashedPassword.SequenceEqual(correctPasswordHash);
        }
        public Password CreatePassword(string rawPassword)
        {
            var salt = GenerateSaltForPassword();
            return new Password(Convert.ToBase64String(ComputePasswordHash(rawPassword, salt)), Convert.ToBase64String(salt));

        }

        private byte[] ComputePasswordHash(string password, byte[] salt) => KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);

        

        private byte[] GenerateSaltForPassword()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);

            }

            return salt;
        }
    }
}
