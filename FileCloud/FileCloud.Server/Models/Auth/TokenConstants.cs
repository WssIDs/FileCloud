﻿namespace FileCloud.Server.Models.Auth
{
    public static class TokenConstants
    {
        public const string Issuer = "localhost:5000";
        public const string Audience = "localhost:5000";
        public const string SecretKey = "this_is_secret_key";
        public const int TokenLifeTime = 900;
    }
}