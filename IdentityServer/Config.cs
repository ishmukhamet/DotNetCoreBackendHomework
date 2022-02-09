﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("todo-api", "ToDoAPI")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                    new Client
                    {
                        ClientId = "client",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "todo-api" },
                        RedirectUris = new List<string>{ "http://localhost:5000/swagger/oauth2-redirect.html" },
                        AllowedCorsOrigins = new List<string> { "http://localhost:5000" }
                    }
};
    }
}