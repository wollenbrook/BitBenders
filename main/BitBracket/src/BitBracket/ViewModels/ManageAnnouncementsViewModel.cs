using System.Collections.Generic;
using BitBracket.Models;  // Ensure you have the correct using statement to access UserAnnouncement

namespace BitBracket.ViewModels
{
    public class ManageAnnouncementsViewModel
    {
        public IEnumerable<UserAnnouncement> DraftAnnouncements { get; set; }
        public IEnumerable<UserAnnouncement> PublishedAnnouncements { get; set; }
    }
}
