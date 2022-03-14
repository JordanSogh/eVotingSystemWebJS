using System;
using System.Collections.Generic;

#nullable disable

namespace eVotingSystemWebJS.Models
{
    public partial class Campaign
    {
        public string CampaignName { get; set; }
        public long? CampaignLength { get; set; }
        public bool IsCurrent { get; set; }
    }
}
