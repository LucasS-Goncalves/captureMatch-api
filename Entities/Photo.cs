using System.ComponentModel.DataAnnotations.Schema;

namespace CaptureMatchApi.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool IsProfilePicture { get; set; }

        public string PublicId { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
