namespace AutenticationDemo.DTOs
{
    public class UserModel
    {
        public string Username { get; private set; }

        public UserModel(string username)
        {
            Username = username;
        }
    }
}