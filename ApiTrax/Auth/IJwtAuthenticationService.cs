﻿namespace ApiTrax.Auth
{
    public interface IJwtAuthenticationService
    {
        string Authenticate(string username, string password);

    }
}
