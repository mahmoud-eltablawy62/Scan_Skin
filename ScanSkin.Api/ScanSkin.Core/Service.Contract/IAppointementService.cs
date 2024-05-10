using ScanSkin.Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Core.Service.Contract
{
    public interface IAppointementService
    {
        Task<Appointment> Appointment(string DoctorEmail, string patient_name, string Doctor_Phone, DateTime Date, TimeSpan StartTime, TimeSpan EndTime, string Doctor_Id);
       Task<List<Appointment>> GetAppoiontments(string Doctor_Id);

        Task<List<Appointment>> GetAppointmentsByData(DateTime date);

       
    }
}
