namespace LacunaSpace.API.Models.Response
{
    public class ProbeListResponseModel
    {
        public List<ProbeModel> probes { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }
}
