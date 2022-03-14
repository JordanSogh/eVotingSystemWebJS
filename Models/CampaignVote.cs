#nullable disable

namespace eVotingSystemWebJS.Models
{
    public partial class CampaignVote
    {
        public string Campaign { get; set; }
        public string VoteDescription { get; set; }
        public long? VoteNumber { get; set; }

        public virtual Campaign CampaignNavigation { get; set; }
    }
}