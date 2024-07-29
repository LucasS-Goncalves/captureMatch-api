namespace CaptureMatchApi.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUserName { get; set; }

        public string PhotographersSearchKey { get; set; } = string.Empty;

        public string State { get; set; }

        public string City { get; set; }
    }
}
