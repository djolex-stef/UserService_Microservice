using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public static class IdentityDataInitializer
    {
        public static void SeedData (UserManager<AccountInfo> userManager, RoleManager<AccountRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        public static void SeedUsers(UserManager<AccountInfo> userManager)
        {

            Guid firstPersonalUserWithAdminId = Guid.Parse("CE593D02-C615-4AF6-A794-C450B79E9B4D");

            Guid secondPersonalUserWithRegularUserId = Guid.Parse("728569AA-7A1F-45C9-B9D4-94BCC176BD0C");
            Guid thirdPersonalUserWithRegularUserId = Guid.Parse("194DF880-D4CE-4997-96C9-878102EB6E0E");

            Guid firstCorporationUserWithRegularUserId = Guid.Parse("9171F23E-ADF2-4698-B73F-05C6FD7AD1BE");
            Guid secondCorporationUserWithRegularUserId = Guid.Parse("9346B8C4-1B3B-435F-9C35-35DE3A76FCF9");

            if (userManager.FindByNameAsync("djolex").Result == null)
            {
                AccountInfo acc = new AccountInfo("djolex", "djolex3211@gmail.com", firstPersonalUserWithAdminId);
                IdentityResult result = userManager.CreateAsync(acc, "mojasifra").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc,"Admin").Wait();
                }
                 
            }
            if (userManager.FindByNameAsync("markoMarkovic").Result == null)
            {
                AccountInfo acc = new AccountInfo("markoMarkovic", "marko@gmail.com", secondPersonalUserWithRegularUserId);
                IdentityResult result = userManager.CreateAsync(acc, "123asd").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc, "Regular user").Wait();
                }
            }
            if (userManager.FindByNameAsync("NevenaNN").Result == null)
            {
                AccountInfo acc = new AccountInfo("NevenaNN", "nevena@gmail.com", thirdPersonalUserWithRegularUserId);
                IdentityResult result = userManager.CreateAsync(acc, "nevena123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc, "Regular user").Wait();
                }
            }
            if (userManager.FindByNameAsync("Stark").Result == null)
            {
                AccountInfo acc = new AccountInfo("Stark", "stark@gmail.com", firstCorporationUserWithRegularUserId);
                IdentityResult result = userManager.CreateAsync(acc, "stark123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc, "Regular user").Wait();
                }
            }
            if (userManager.FindByNameAsync("NationalBank").Result == null)
            {
                AccountInfo acc = new AccountInfo("NationalBank", "nat_bank@gmail.com", secondCorporationUserWithRegularUserId);
                IdentityResult result = userManager.CreateAsync(acc, "bank123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(acc, "Regular user").Wait();
                }
            }

        }

        public static void SeedRoles(RoleManager<AccountRole> roleManager)
        {
            Guid adminRoleId = Guid.Parse("987268E5-F880-4F81-B1BF-5B9704604E26");
            Guid regularUserRoleId = Guid.Parse("33253633-10E4-45C8-9B8E-84020A5C8C58");



            if (!roleManager.RoleExistsAsync("Regular user").Result)
            {
                AccountRole role = new AccountRole(regularUserRoleId, "Role that has only few privileges", "Regular user");
                roleManager.CreateAsync(role).Wait();
            }
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                AccountRole role = new AccountRole(adminRoleId, "Role that has every privilege", "Admin");
                roleManager.CreateAsync(role).Wait();
            }
        }
    }
}
