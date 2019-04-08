using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageAPI.Models.Database;

namespace AdminPanelTurBuro.Controllers.API
{
    [Route("api/[controller]")]
    public class VideoController : Controller
    {
        private readonly TourAgencyContext _context;

        public VideoController(TourAgencyContext context)
        {
            _context = context;
        }

        public string GetFirstBisy()
        {
            try
            {

                return _context.VideoConnects.FirstOrDefault(f => !f.IsBusy)?.Name;

            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<bool> MakeConnect(string name)
        {
            try
            {
                var admin = _context.VideoConnects.FirstOrDefault(f => f.Name == name);
                if (admin == null) return false;
                admin.IsBusy = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public async Task CallEnd(string name)
        {
            try
            {
                var admin = _context.VideoConnects.FirstOrDefault(f => f.Name == name);
                if (admin == null) return;
                admin.IsBusy = false;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }  
        }

        [HttpGet]
        public async Task<bool> AddNewAdmin(string name)
        {
            try
            {
                VideoConnect admin = new VideoConnect();
                admin.Name = name;
                admin.IsBusy = false;
                _context.VideoConnects.Add(admin);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public async Task AdminLeave(string name)
        {
            try
            {
                var admin = _context.VideoConnects.FirstOrDefault(f => f.Name == name);
                if (admin == null) return;
                _context.VideoConnects.Remove(admin);
                await _context.SaveChangesAsync();
            }
            catch
            {
            }
        }
    }
}
