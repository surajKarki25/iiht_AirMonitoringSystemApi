namespace iiht_AirMonitoringSystemApi.Models
{
    public class Sensor
    {
        public long Id { get; set; }
        public DateTime date { get; set; }
        public int Mapped_Floor { get; set; } 
        public double Dust_Parcticle { get; set; }
        public double Co2 { get; set; }
        public double Co { get; set; }
        public double No2 { get; set; }
        public double So2 { get; set; }
    }
}
