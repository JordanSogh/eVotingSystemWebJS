using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable

namespace eVotingSystemWebJS.Models
{
    public partial class VotingDBContext : DbContext
    {
        public VotingDBContext()
        {
        }

        public VotingDBContext(DbContextOptions<VotingDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<CampaignVote> CampaignVotes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }

        public User ValidateLogin(User user)
        {
            User userRetrieved = null;

            using (VotingDBContext votingDBContext = new VotingDBContext())
            {
                userRetrieved = votingDBContext.Users
                                       .Where(m => m.UserName == user.UserName && m.Password == user.Password)
                                       .FirstOrDefault();
            }

            return userRetrieved;
        }
        public Campaign GetCampaign()
        {
            Campaign campaignRetrieved = null;

            using (VotingDBContext votingDBContext = new VotingDBContext())
            {
                campaignRetrieved = votingDBContext.Campaigns
                                    .Where(c => c.IsCurrent == true)
                                    .FirstOrDefault();
            }

            return campaignRetrieved;
        }
        public List<CampaignVote> GetCampaignVotes(Campaign campaign)
        {
            List<CampaignVote> voteOptionslist = null;

            using (VotingDBContext votingDBContext = new VotingDBContext())
            {
                var voteOptionslistretrieved = votingDBContext.CampaignVotes
                                    .Where(v => v.Campaign == campaign.CampaignName);
                voteOptionslist = voteOptionslistretrieved.ToList();
            }
            return voteOptionslist;
        }
        public bool CastVote(Campaign campaign, string ballotDescription, int ballotNumber)
        {
            using (VotingDBContext votingDBContext = new VotingDBContext())
            {
                var Vote = new Vote()
                {
                    Ballot = ballotNumber,
                    BallotDescription = ballotDescription,
                    Campaign = campaign.CampaignName,
                };
                votingDBContext.Votes.Add(Vote);
                votingDBContext.SaveChanges();

                var user = votingDBContext.Users.Where(u => u.UserName == "userVoter").First();
                user.Voted = true;
                votingDBContext.SaveChanges();
            }
            return true;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data source = " + Directory.GetCurrentDirectory().ToString() + "\\VotingDB.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasKey(e => e.CampaignName);

                entity.ToTable("Campaign");

                entity.Property(e => e.CampaignName).HasColumnType("CHAR (30)");

                entity.Property(e => e.IsCurrent).HasColumnType("BOOLEAN");
            });

            modelBuilder.Entity<CampaignVote>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Campaign).HasColumnType("CHAR");

                entity.Property(e => e.VoteDescription).HasColumnType("CHAR");

                entity.HasOne(d => d.CampaignNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Campaign);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserName);

                entity.ToTable("User");

                entity.Property(e => e.UserName)
                    .HasColumnType("CHAR")
                    .HasColumnName("userName");

                entity.Property(e => e.Password)
                    .HasColumnType("CHAR")
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasColumnType("CHAR")
                    .HasColumnName("role");

                entity.Property(e => e.Voted)
                    .HasColumnType("BOOLEAN")
                    .HasColumnName("voted");
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.HasKey(e => e.Ballot);

                entity.ToTable("Vote");

                entity.Property(e => e.BallotDescription).HasColumnType("CHAR");

                entity.Property(e => e.Campaign).HasColumnType("CHAR");

                entity.HasOne(d => d.CampaignNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Campaign);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}