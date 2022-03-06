using System;
using System.Collections.Generic;

#nullable disable

namespace eVotingSystemWebJS.Models
{
    public partial class Vote
    {
        public string Campaign { get; set; }
        public long? Ballot { get; set; }
        public string BallotDescription { get; set; }

        public virtual Campaign CampaignNavigation { get; set; }
    }
}
