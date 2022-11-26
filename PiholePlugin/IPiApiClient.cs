namespace Loupedeck.PiholePlugin
{
    using System;
    using System.Threading.Tasks;
    using Loupedeck.PiholePlugin.Models;

    public interface IPiHoleApiClient
    {
        Task<Summary> GetSummaryRawAsync();

        Task<PiStatus> Enable();

        Task<PiStatus> Disable(Int32 seconds = 0);
    }
}
