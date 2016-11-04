namespace AutenticationDemo.DTOs.Negocio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using Jose;

    public class UserDatabase
    {
        static readonly List<Tuple<string, string>> ActiveApiKeys = new List<Tuple<string, string>>();
        private static readonly List<Tuple<string, string>> Users = new List<Tuple<string, string>>();

        static UserDatabase()
        {
            Users.Add(new Tuple<string, string>("usuario1", "123"));
            Users.Add(new Tuple<string, string>("usuario2", "456"));
        }

        public static ClaimsPrincipal GetUserFromApiKey(string apiKey)
        {
            var activeKey = ActiveApiKeys.FirstOrDefault(x => x.Item2 == apiKey);

            if (activeKey == null)
            {
                return null;
            }

            var userRecord = Users.First(u => u.Item1 == activeKey.Item1);
            return new ClaimsPrincipal(new GenericIdentity(userRecord.Item1, "stateless"));
        }

        public static string ValidateUser(string username, string password)
        {
            //try to get a user from the "database" that matches the given username and password
            var userRecord = Users.FirstOrDefault(u => u.Item1 == username && u.Item2 == password);

            if (userRecord == null)
            {
                return null;
            }

            //now that the user is validated, create an api key that can be used for subsequent requests
            var apiKey = Guid.NewGuid().ToString();
            var payload = new Dictionary<string, object>()
            {
                {"username",username},
                {"role","Admin"}

            };
            var secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };
            string token = Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);



            ActiveApiKeys.Add(new Tuple<string, string>(username, token));
            return token;
        }

        public static void RemoveApiKey(string apiKey)
        {
            var apiKeyToRemove = ActiveApiKeys.First(x => x.Item2 == apiKey);
            ActiveApiKeys.Remove(apiKeyToRemove);
        }

        public static Tuple<string, string> CreateUser(string username, string password)
        {
            var user = new Tuple<string, string>(username, password);
            Users.Add(user);
            return user;
        }
    }
}