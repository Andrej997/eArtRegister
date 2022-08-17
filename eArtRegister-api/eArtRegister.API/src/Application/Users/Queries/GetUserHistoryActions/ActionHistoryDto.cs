using eArtRegister.API.Application.Common.Mappings;
using eArtRegister.API.Domain.Entities;
using System;

namespace eArtRegister.API.Application.Users.Queries.GetUserHistoryActions
{
    public class ActionHistoryDto : IMapFrom<NFTActionHistory>
    {
        public string Wallet { get; set; }
        public Guid? NFTId { get; set; }
        public string EventAction { get; set; }
        public long EventTimestamp { get; set; }
        public string TransactionHash { get; set; }
        public bool IsCompleted { get; set; }
        public string DateOfEvent { get => new DateTime(EventTimestamp).ToString("MMMM dd, yyyy. hh mm ss"); }
    }
}
