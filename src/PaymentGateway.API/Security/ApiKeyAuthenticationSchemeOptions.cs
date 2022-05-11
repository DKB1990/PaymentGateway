﻿using Microsoft.AspNetCore.Authentication;

namespace PaymentGateway.API.Security
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string Scheme = "ApiKeyScheme";
    }
}
