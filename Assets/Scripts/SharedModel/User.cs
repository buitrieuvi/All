using System;

public class User
{
    public string Id;
    public string UserName;
    public string PasswordHash;

    public string RefreshToken;
    public DateTime RefreshTokenExpiryTime;

    public User(string id, string userName, string passwordHash)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
    }

    public class Register 
    {

        public string UserName;
        public string Password;
    }

    public class Login
    {
        public string UserName;
        public string Password;
    }

    public class Token 
    {
        public string AccessToken;
        public string RefreshToken;

        public Token(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
