namespace UPTrain.Models
{
    public class Badge
    {
        public int BadgeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
    }
}
