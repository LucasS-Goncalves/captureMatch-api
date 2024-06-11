namespace CaptureMatchApi.DTOs
{
    public class MemberDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Created {  get; set; }

        public DateTime LastActive { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public DateOnly DateOfBirth { get; set; }


        public string PhotographerAboutMe { get; set; }
        public string PhotographerSpecialties { get; set; }

    }
}
