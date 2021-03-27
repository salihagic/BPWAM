using Microsoft.AspNetCore.Identity;
using System;

namespace BPWA.Common.Services
{
    public interface IPasswordGeneratorService
    {
        String Generate(PasswordOptions options = null);
    }
}
