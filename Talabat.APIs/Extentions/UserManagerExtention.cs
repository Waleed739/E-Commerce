﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extentions
{
    public static class UserManagerExtention
    {
        public static async Task<AppUser> FindUserAddressByEmail(this UserManager<AppUser> userManager ,ClaimsPrincipal currentUser )
        {
            var email = currentUser.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Email==email);
            return user;
        }
    }
}
