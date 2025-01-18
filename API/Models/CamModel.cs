namespace CamWebRtc.API.Models
{
    public class CamModel
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public bool IsActive { get; set; }
        public string ?ConnectionID { get; set; }
    }
}
