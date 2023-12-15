﻿namespace IT_Arg_API.Models.Helpers
{
    public class RefreshToken
    {
       
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.Now >= Expires;

        
    }
}
