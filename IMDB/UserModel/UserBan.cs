namespace IMDB.UserModel
{
    public class UserBan
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime BannedUntil { get; set; }
        public string Reason { get; set; }


        public void SomeMethod()
        {
            var userId = "UserIdToBan";
            var banDuration = DateTime.Now.AddDays(7);
            var reason = "Inappropriate behavior";


        }


    }
}
