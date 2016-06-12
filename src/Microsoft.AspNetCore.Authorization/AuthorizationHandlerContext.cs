// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.AspNetCore.Authorization
{
    /// <summary>
    /// Contains authorization information used by <see cref="IAuthorizationHandler"/>.
    /// </summary>
    public class AuthorizationHandlerContext
    {
        /// <summary>
        /// Creates a new instance of <see cref="AuthorizationHandlerContext"/>.
        /// </summary>
        /// <param name="requirements">A collection of all the <see cref="IAuthorizationRequirement"/> for the current authorization action.</param>
        /// <param name="user">A <see cref="ClaimsPrincipal"/> representing the current user.</param>
        /// <param name="resource">An optional resource to evaluate the <paramref name="requirements"/> against.</param>
        public AuthorizationHandlerContext(
            IEnumerable<IAuthorizationRequirement> requirements,
            ClaimsPrincipal user,
            object resource)
        {
            if (requirements == null)
            {
                throw new ArgumentNullException(nameof(requirements));
            }

            Requirements = requirements;
            User = user;
            Resource = resource;
            FailedRequirements = new ConcurrentQueue<IAuthorizationRequirement>();
            SucceededRequirements = new ConcurrentQueue<IAuthorizationRequirement>();
        }

        /// <summary>
        /// The collection of all the <see cref="IAuthorizationRequirement"/> for the current authorization action.
        /// </summary>
        public IEnumerable<IAuthorizationRequirement> Requirements { get; }

        /// <summary>
        /// The <see cref="ClaimsPrincipal"/> representing the current user.
        /// </summary>
        public ClaimsPrincipal User { get; }

        /// <summary>
        /// The optional resource to evaluate the <see cref="AuthorizationHandlerContext.Requirements"/> against.
        /// </summary>
        public object Resource { get; }

        public ConcurrentQueue<IAuthorizationRequirement> FailedRequirements { get; }

        public ConcurrentQueue<IAuthorizationRequirement> SucceededRequirements { get; }

        public void Fail(IAuthorizationRequirement requirement = null)
        {
            FailedRequirements.Enqueue(requirement);
        }

        public void Succeed(IAuthorizationRequirement requirement)
        {
            if (requirement == null)
            {
                throw new ArgumentNullException(nameof(requirement));
            }

            SucceededRequirements.Enqueue(requirement);
        }
    }
}