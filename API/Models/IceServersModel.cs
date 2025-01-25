namespace CamWebRtc.API.Models
{
    public class IceServersModel
    {
        public int Id { get; set; }
        public List<StunServersUrls> ? UrlsStun { get; set; }
        public string ?username { get; set; }
        public string ?credential { get; set; }
        public List<TurnServersUrls> ?urlsTurn { get; set; }
    }

    public class StunServersUrls
    {
        public int id {  get; set; }
        public string ?Urls { get; set; }
    }
    public class TurnServersUrls
    {
        public int id { get; set; }
        public string ?Urls { get; set; }
    }

}
