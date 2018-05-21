﻿// Copyright (c) Kris Penner. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MultiTenancyServer.Http.Parsers
{
    /// <summary>
    /// Tenant canonical name can be set via a user security claim
    /// within the authenticated user of a request.
    /// </summary>
    public class UserClaimParser : RequestParser
    {
        /// <summary>
        /// The issuer of the tenancy claim.
        /// Eg: "https://authority.multitenancyserver.com".
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The full claim type of the claim to look for in the current user.
        /// Eg: "http://schemas.microsoft.com/identity/claims/tenantid".
        /// </summary>
        public string ClaimType { get; set; } = "http://schemas.microsoft.com/identity/claims/tenantid";

        /// <summary>
        /// An alternative or shorthand claim type of the claim to look for in the current user.
        /// Eg: "tid"
        /// </summary>
        public string ShortClaimType { get; set; } = "tid";

        /// <summary>
        /// Retrieves the value of the <see cref="ClaimType"/> claim from the current user.
        /// </summary>
        /// <param name="httpContext">The request to retrieve the user claim from.</param>
        /// <returns>The value of the matched claim from the current user of the request.</returns>
        public override string ParseRequest(HttpContext httpContext)
        {
            return httpContext?.User?.Claims?.OfType<Claim>()
                .Where(c => (Issuer == null || string.Equals(c.Issuer, Issuer, StringComparison.OrdinalIgnoreCase)) &&
                    ((ClaimType != null && string.Equals(c.Type, ClaimType, StringComparison.OrdinalIgnoreCase)) ||
                    (ShortClaimType != null && string.Equals(c.Type, ShortClaimType, StringComparison.OrdinalIgnoreCase))))
                .FirstOrDefault()?.Value;
        }
    }
}
