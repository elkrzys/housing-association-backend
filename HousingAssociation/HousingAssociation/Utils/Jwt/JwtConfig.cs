﻿public class JwtConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public int RefreshTokenTTL { get; set; }
}