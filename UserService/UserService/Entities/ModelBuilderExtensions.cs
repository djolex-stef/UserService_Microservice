using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public static class ModelBuilderExtensions
    {
       
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Guid adminRoleId = Guid.Parse("987268E5-F880-4F81-B1BF-5B9704604E26");
            Guid regularUserRoleId = Guid.Parse("33253633-10E4-45C8-9B8E-84020A5C8C58");
            
            Guid firstCountryId = Guid.Parse("8C349E7B-1C97-486D-AA2E-E58205D11577");
            Guid secondCountryId = Guid.Parse("FF0C9396-7C4C-4BF5-A12E-6AA79C272413");
            
            Guid firstPersonalUserWithAdminId = Guid.Parse("CE593D02-C615-4AF6-A794-C450B79E9B4D");
            
            Guid secondPersonalUserWithRegularUserId = Guid.Parse("728569AA-7A1F-45C9-B9D4-94BCC176BD0C");
            Guid thirdPersonalUserWithRegularUserId = Guid.Parse("194DF880-D4CE-4997-96C9-878102EB6E0E");

            Guid firstCorporationUserWithRegularUserId = Guid.Parse("9171F23E-ADF2-4698-B73F-05C6FD7AD1BE");
            Guid secondCorporationUserWithRegularUserId = Guid.Parse("9346B8C4-1B3B-435F-9C35-35DE3A76FCF9");

            modelBuilder.Entity<Role>().HasData(
            new
            {
                RoleId = adminRoleId,
                RoleName = "Admin"
            },
            new
            {
                RoleId = regularUserRoleId,
                RoleName = "Regular user"
            });


            modelBuilder.Entity<Country>().HasData(
            new
            {
                CountryId = firstCountryId,
                CountryName = "Serbia"
            },
            new
            {
                CountryId = secondCountryId,
                CountryName = "US"
            });

            modelBuilder.Entity<PersonalUser>().HasData(
            new
            {
                UserId = firstPersonalUserWithAdminId,
                Email = "djolex3211@gmail.com",
                Password = "mojasifra",
                IsActive = true,
                Telephone = "+381628192354",
                Username = "djolex",
                FirstName = "Djordje",
                LastName = "Stefanovic",
                CountryId = firstCountryId,
                RoleId = adminRoleId
            },
            new
            {
                UserId = secondPersonalUserWithRegularUserId,
                Email = "marko@gmail.com",
                Password = "123asd",
                IsActive = true,
                Telephone = "+3816965555555",
                Username = "markoMarkovic",
                FirstName = "Marko",
                LastName = "Markovic",
                CountryId = firstCountryId,
                RoleId = regularUserRoleId
            },
            new
            {
                UserId = thirdPersonalUserWithRegularUserId,
                Email = "nevena@gmail.com",
                Password = "nevena123",
                IsActive = true,
                Telephone = "+381691234567",
                Username = "nikolicNN",
                FirstName = "Nevena",
                LastName = "Nikolic",
                CountryId = secondCountryId,
                RoleId = regularUserRoleId
            });

            modelBuilder.Entity<CorporationUser>().HasData(
            new
            {
                UserId = firstCorporationUserWithRegularUserId,
                Email = "stark@gmail.com",
                Password = "stark123",
                IsActive = true,
                Telephone = "+38160111222",
                Username = "Stark",
                CorporationName = "Stark",
                Pib = "515731",
                CorporationCity = "Novi Sad",
                CorporationAddress = "Kisacka 25",
                CountryId = firstCountryId,
                RoleId = regularUserRoleId
            },
            new
            {
                UserId = secondCorporationUserWithRegularUserId,
                Email = "nat_bank@gmail.com",
                Password = "bank123",
                IsActive = true,
                Telephone = "+38165555113",
                Username = "NationalBank",
                CorporationName = "National Bank",
                Pib = "51516",
                CorporationCity = "Washington DC",
                CorporationAddress = "unknown",
                CountryId = secondCountryId,
                RoleId = regularUserRoleId
            });
        }

        public static void SeedIdentity(this ModelBuilder modelBuilder)
        {
            Guid adminRoleId = Guid.Parse("987268E5-F880-4F81-B1BF-5B9704604E26");
            Guid regularUserRoleId = Guid.Parse("33253633-10E4-45C8-9B8E-84020A5C8C58");

            Guid firstPersonalUserWithAdminId = Guid.Parse("CE593D02-C615-4AF6-A794-C450B79E9B4D");

            Guid secondPersonalUserWithRegularUserId = Guid.Parse("728569AA-7A1F-45C9-B9D4-94BCC176BD0C");
            Guid thirdPersonalUserWithRegularUserId = Guid.Parse("194DF880-D4CE-4997-96C9-878102EB6E0E");

            Guid firstCorporationUserWithRegularUserId = Guid.Parse("9171F23E-ADF2-4698-B73F-05C6FD7AD1BE");
            Guid secondCorporationUserWithRegularUserId = Guid.Parse("9346B8C4-1B3B-435F-9C35-35DE3A76FCF9");

            modelBuilder.Entity<AccountInfo>().HasData(
                new AccountInfo("djolex", "djolex3211@gmail.com", firstPersonalUserWithAdminId),
                new AccountInfo("markoMarkovic", "marko@gmail.com", secondPersonalUserWithRegularUserId),
                new AccountInfo("nikolicNN", "nevena@gmail.com", thirdPersonalUserWithRegularUserId),
                new AccountInfo("Stark", "stark@gmail.com", firstCorporationUserWithRegularUserId),
                new AccountInfo("NationalBank", "nat_bank@gmail.com", secondCorporationUserWithRegularUserId)
         );

            modelBuilder.Entity<AccountRole>().HasData(
                new AccountRole(adminRoleId, "Admin", "Role that has every privilege"),
                new AccountRole(regularUserRoleId, "Regular user", "Role that has few privileges")
         );
        }
    }
}