namespace IMDB.UserModel
{
    public class AddUserPreference
    {
        public string Actor { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
    }

    public class RemoveUserPreference
    {
        public string Actor { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
    }
}
