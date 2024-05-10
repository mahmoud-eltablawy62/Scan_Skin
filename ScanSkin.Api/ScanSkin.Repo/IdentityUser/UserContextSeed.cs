using Microsoft.AspNetCore.Identity;
using ScanSkin.Core.Entites.Identity_User;
using System.Globalization;
using System.Text;
namespace ScanSkin.Repo.IdentityUser
{
    public static class UserContextSeed
    {
        public static async Task UserSeedAsync(UserManager<Users> _user)
        {

            DateTime currentDate = DateTime.Now.Date;
            DayOfWeek currentDayOfWeek = currentDate.DayOfWeek;
            int daysToAdd = ((int)DayOfWeek.Sunday - (int)currentDayOfWeek + 7) % 7;
            DateTime startOfWeek = currentDate.AddDays(daysToAdd);
            DateTime endOfWeek = startOfWeek.AddDays(4);
            if (_user.Users.Count() == 0)
            {
                var user = new Users()
                {
                    Email = "Dr.QamriyaAl-Banna702@gmail.com",
                    UserName = "Dr.QamriyaAl-Banna",
                    PhoneNumber = "01554847412",
                    Experience = 12,
                    Price = 250,
                    AddressLocation = "Tanta/ElheloStreet",
                    AddressDescription = "Tanta/elheloStreet/84",
                    Speciality = "specialistindermatology,cosmeticsandlaser",
                    StartWork = new TimeSpan(9, 0, 0),
                    EndWork = new TimeSpan(17, 0,0),
                    DurationTime = new TimeSpan(8, 0,0),
                    StartDate = startOfWeek,
                    EndDate = endOfWeek,
                    Profile_Picture = Encoding.UTF8.GetBytes("Doctors/download.jpg"),
                    EmailConfirmed = true
                };
                await _user.CreateAsync(user, "mahM2#");
                await _user.AddToRoleAsync(user,"Doctor");

                var user1 = new Users()
                {
                    Email = "AhmedFekry702@gmail.com",
                    UserName = "Dr.AhmedFekry",
                    PhoneNumber = "01554847412",
                    Experience = 12,
                    Price = 300,
                    AddressLocation = "Tanta/ElheloStreet",
                    AddressDescription = "Tanta/elheloStreet/84",
                    Speciality = "specialistindermatology,cosmeticsandlaser",
                    StartWork = new TimeSpan(9, 0, 0),
                    EndWork = new TimeSpan(17, 0, 0),
                    DurationTime = new TimeSpan(8, 0, 0),
                    StartDate = startOfWeek,
                    EndDate = endOfWeek,
                    Profile_Picture = Encoding.UTF8.GetBytes("Doctors/downloa.jpg"),
                    EmailConfirmed = true
                };
                await _user.CreateAsync(user1, "mahM2#");
                await _user.AddToRoleAsync(user1, "Doctor");

                var user2 = new Users()
                {
                    Email = "TariqAbdelHamidAl-Najjar702@gmail.com",
                    UserName = "Dr.TariqAbdelHamidAl-Najjar",
                    PhoneNumber = "01554847412",
                    Experience = 12,
                    Price = 250,
                    AddressLocation = "Tanta/ElheloStreet",
                    AddressDescription = "Tanta/elheloStreet/84",
                    Speciality = "specialistindermatology,cosmeticsandlaser",
                    Profile_Picture = Encoding.UTF8.GetBytes("Doctors/down.jpg"),
                    StartWork = new TimeSpan(9, 0, 0),
                    EndWork = new TimeSpan(17, 0, 0),
                    DurationTime = new TimeSpan(8, 0, 0),
                    StartDate = startOfWeek,
                    EndDate = endOfWeek,
                    EmailConfirmed = true
                };
                await _user.CreateAsync(user2, "mahM2#");
                await _user.AddToRoleAsync(user2, "Doctor");

                var user3 = new Users()
                {
                    Email = "Dr.NerminTariqKhorshid702@gmail.com",
                    UserName = "Dr.NerminTariqKhorshid",
                    PhoneNumber = "01554847412",
                    Experience = 12,
                    Price = 350,
                    AddressLocation = "Tanta/ElheloStreet",
                    AddressDescription = "Tanta/elheloStreet/84",
                    Speciality = "specialistindermatology,cosmeticsandlaser",
                    StartWork = new TimeSpan(9, 0, 0),
                    EndWork = new TimeSpan(17, 0, 0),
                    DurationTime = new TimeSpan(8, 0, 0),
                    StartDate = startOfWeek,
                    EndDate = endOfWeek,
                    Profile_Picture = Encoding.UTF8.GetBytes("Doctors/downl.jpg"),
                    EmailConfirmed = true
                };
                await _user.CreateAsync(user3, "mahM2#");
                await _user.AddToRoleAsync(user3, "Doctor");

                var user4 = new Users()
                {
                    Email = "Dr.HaniAl-Zammi702@gmail.com",
                    UserName = "Dr.HaniAl-Zammi",
                    PhoneNumber = "01554847412",
                    Experience = 12,
                    Price = 200,
                    AddressLocation = "Tanta/ElheloStreet",
                    AddressDescription = "Tanta/elheloStreet/84",
                    Speciality = "specialistindermatology,cosmeticsandlaser",
                    StartWork = new TimeSpan(9, 0, 0),
                    EndWork = new TimeSpan(17, 0, 0),
                    DurationTime = new TimeSpan(8, 0, 0),
                    StartDate = startOfWeek,
                    EndDate = endOfWeek,
                    Profile_Picture = Encoding.UTF8.GetBytes("Doctors/downlo.jpg"),
                    EmailConfirmed = true
                };
                await _user.CreateAsync(user4, "mahM2#");
                await _user.AddToRoleAsync(user4, "Doctor");

                var user5 = new Users()
                {
                    Email = "Dr.SalemSalahSalem702@gmail.com",
                    UserName = "Dr.SalemSalahSalem",
                    PhoneNumber = "01554847412",
                    Experience = 12,
                    Price = 250,
                    AddressLocation = "Tanta/ElheloStreet",
                    AddressDescription = "Tanta/elheloStreet/84",
                    Speciality = "specialistindermatology,cosmeticsandlaser",
                    StartWork = new TimeSpan(9, 0, 0),
                    EndWork = new TimeSpan(17, 0, 0),
                    DurationTime = new TimeSpan(8, 0, 0),
                    StartDate = startOfWeek,
                    EndDate = endOfWeek,
                    Profile_Picture = Encoding.UTF8.GetBytes("Doctors/dow.jpg"),
                    EmailConfirmed = true
                };
                await _user.CreateAsync(user5, "mahM2#");
                await _user.AddToRoleAsync(user5, "Doctor");

            }
        }
    }
}
