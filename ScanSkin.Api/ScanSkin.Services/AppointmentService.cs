using Microsoft.EntityFrameworkCore;
using ScanSkin.Core;
using ScanSkin.Core.Entites;
using ScanSkin.Core.Service.Contract;
using ScanSkin.Repo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScanSkin.Services
{
    public class AppointmentService : IAppointementService
    {
        private readonly IUnitOfWork _Unit;
        private readonly ScanSkinContext scanSkinContext ; 
        public AppointmentService(IUnitOfWork Unit , ScanSkinContext _scanSkinContext)
        {
            _Unit = Unit;
            scanSkinContext = _scanSkinContext;
        }
        public async Task<Appointment> Appointment(string DoctorEmail, string patient_name, string Doctor_Phone, DateTime Date, TimeSpan StartTime, TimeSpan EndTime, string Doctor_Id)
        {
            var Appoint = new Appointment(DoctorEmail, patient_name , Doctor_Phone , Date , StartTime , EndTime , Doctor_Id);
            await _Unit.Repo<Appointment>().Add(Appoint);
            await _Unit.CompleteAsync();
            return Appoint;
        }

        public async Task<List<Appointment>> GetAppointmentsByData(DateTime date) =>
            await scanSkinContext.Appointments.Where(A=>A.Date == date).ToListAsync();
         

        public async Task<List<Appointment>> GetAppoiontments(string Doctor_Id) =>
          await scanSkinContext.Appointments.Where(a => a.Doctor_Id == Doctor_Id).ToListAsync();

       

    }
}
