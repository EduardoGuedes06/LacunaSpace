namespace LacunaSpace.API.Models
{
    public class ProbeSyncInfoModel
    {
        public string ProbeId { get; set; }
        public long TimeOffset { get; set; }
        public long RoundTrip { get; set; }
    }

}
