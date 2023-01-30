using iiht_AirMonitoringSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace iiht_AirMonitoringSystemApi.Data
{
   
        public class SensorDbContext : DbContext
        {
            public SensorDbContext(DbContextOptions<SensorDbContext> options) : base(options)
            {

            }
            public DbSet<Sensor> Sensors { get; set; } //sensors data name will be given to the table in database
            public DbSet<Users> Users { get; set; }
        }
    
}
