namespace ClimbingCommunity.Web.Infrastructure.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;
    using System.Collections.Concurrent;
    using System.Security.Claims;
    using static Common.GeneralApplicationConstants;
    public class OnlineUsersMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string cookieName;
        private readonly int lastActivityMinutes;

        private static readonly ConcurrentDictionary<string, bool> AllKeys =
            new ConcurrentDictionary<string, bool>();

        public OnlineUsersMiddleware(RequestDelegate next,
            string cookieName = OnlineUserCookieName,
            int lastActivityMinutes = LastActivityBeforeGoOflineMinutes)
        {
            this.next = next;
            this.cookieName = cookieName;
            this.lastActivityMinutes = lastActivityMinutes;

        }

        public Task InvokeAsync(HttpContext context, IMemoryCache memoryCache)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                if (!context.Request.Cookies.TryGetValue(this.cookieName, out string userId))
                {
                    userId = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                    context.Response.Cookies.Append(cookieName, userId, new CookieOptions()
                    {
                        HttpOnly = true,
                        MaxAge = TimeSpan.FromDays(30)
                    });

                }
                memoryCache.GetOrCreate(userId, cacheEntry =>
                {
                    if (!AllKeys.TryAdd(userId, true))
                    {
                        // invalid case
                        cacheEntry.AbsoluteExpiration = DateTimeOffset.MinValue;
                    }
                    else
                    {
                        // if user don't click anywhere in the application - don't have activity will go offline after 10 min.
                        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(lastActivityMinutes);
                        cacheEntry.RegisterPostEvictionCallback(this.RemoveKeyWhenExpired);
                    }
                    return string.Empty;
                });
            }
            //if user has just logged out 
            else
            {
                if (context.Request.Cookies.TryGetValue(cookieName, out string userId))
                {
                    // setting user cookie to offline after logout
                    if (AllKeys.TryRemove(userId, out _))
                    {
                        AllKeys.TryUpdate(userId, false, true);
                    }

                    context.Response.Cookies.Delete(cookieName);
                }
            }
            return this.next(context);
        }

        public static bool CheckIfUserIsOnline(string userId)
        {
            bool valueTaken = AllKeys.TryGetValue(userId, out bool success);

            return success && valueTaken;
        }

        private void RemoveKeyWhenExpired(object key, object value, EvictionReason reason, object state)
        {
            string keyStr = (string)key;  //UserId

            if (!AllKeys.TryRemove(keyStr, out _))
            {
                AllKeys.TryUpdate(keyStr, false, true);
            }
        }
    }
}
