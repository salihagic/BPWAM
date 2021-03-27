using BPWA.Common.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BPWA.Common.Services
{
    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        const string NonAlphanumericSet = @"!#$%&*@\";
        const string LowercaseSet = "abcdefghijklmnopqrstuvwxyz";
        const string UppercaseSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string DigitsSet = "0123456789";

        private PasswordOptions _options;

        public PasswordGeneratorService(PasswordOptions options)
        {
            _options = options;
        }

        public string Generate(PasswordOptions options = null)
        {
            if(options != null)
                _options = options;

            var characterSets = new List<string>();

            if (_options.RequireNonAlphanumeric)
                characterSets.Add(NonAlphanumericSet);
            if (_options.RequireLowercase)
                characterSets.Add(LowercaseSet);
            if (_options.RequireUppercase)
                characterSets.Add(UppercaseSet);
            if (_options.RequireDigit)
                characterSets.Add(DigitsSet);

            if(characterSets.IsEmpty())
                characterSets.Add(LowercaseSet);

            var password = "";
            Random random = new Random();

            for (int i = 0, j = 0; i < _options.RequiredLength; i++, j++)
            {
                if (j >= characterSets.Count)
                    j = 0;

                password += characterSets[j][random.Next(characterSets[j].Length - 1)];
            }

            return password;
        }
    }
}
