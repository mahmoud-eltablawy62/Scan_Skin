using ScanSkin.Api.Dtos;
using AutoMapper;
using ScanSkin.Core.Entites.Identity_User;
using System.Collections;
using System.Text;

namespace ScanSkin.Api.Helpers
{
    public class ItemResolver : IValueResolver<Users, DoctorToReturnDto, string>
    {

        private readonly IConfiguration _Configuration;
        public ItemResolver(IConfiguration configuration)
        {

            _Configuration = configuration;

        }
        public string Resolve(Users source, DoctorToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (source.Profile_Picture != null)
            {
                string result = Encoding.UTF8.GetString(source.Profile_Picture);
                if (!string.IsNullOrEmpty(result))
                {
                    return $"{_Configuration["ApiBaseUrl"]}/{result}";
                }
            }
            return string.Empty;

        }
    }
}
