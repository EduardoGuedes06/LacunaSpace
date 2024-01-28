using AutoMapper.Execution;

namespace LacunaSpace.API.Models.Request
{
    public class JobRequestDone
    {
        public string probeNow { get; set; }
        public long roundTrip { get; set; }

    }
}
