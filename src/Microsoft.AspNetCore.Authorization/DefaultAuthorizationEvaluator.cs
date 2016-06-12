// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;

namespace Microsoft.AspNetCore.Authorization
{
    public class DefaultAuthorizationEvaluator : IAuthorizationEvaluator
    {
        public bool HasFailed(AuthorizationHandlerContext context)
        {
            return context.FailedRequirements.Any();
        }

        public bool HasSucceeded(AuthorizationHandlerContext context)
        {
            return !context.FailedRequirements.Any() && context.SucceededRequirements.Any() && !context.Requirements.Except(context.FailedRequirements).Except(context.SucceededRequirements).Any();
        }
    }
}