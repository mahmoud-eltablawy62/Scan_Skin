using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Core.Entites.Identity_User
{
    public enum BloodType
    {
        [EnumMember(Value = "B-")]
        Bnegative,
        [EnumMember(Value = "B+")]
        BPositive,     
        [EnumMember(Value = "A+")]
        APositive,
        [EnumMember(Value = "A-")]
        ANegative,
        [EnumMember(Value = "AB+")]
        ABPositive,
        [EnumMember(Value = "AB-")]
        ABNegative,
        [EnumMember(Value = "O+")]
        OPositive,
        [EnumMember(Value = "O+")]
        ONegative,
    }
}
  