using Microsoft.Extensions.Caching.Distributed;
using NationalCookies.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NationalCookies.Data.Services
{
    public class CookieService : ICookieService
    {
        private CookieContext _context;
        private readonly IDistributedCache _cache;

        public CookieService(CookieContext context, IDistributedCache cache)
        {
            _context = context;
            _cache  = cache;
        }

        public List<Cookie> GetAllCookies()
        {
            List<Cookie> cookies; 

            var cachedCookies = _cache.GetString("cookies");
            if (!String.IsNullOrEmpty(cachedCookies))
            {
                cookies = JsonConvert.DeserializeObject<List<Cookie>>(cachedCookies);
            }
            else
            {
                //get the cookies from the database
                cookies = _context.Cookies.ToList();
                var options = new DistributedCacheEntryOptions();


                _cache.SetString("cookies", JsonConvert.SerializeObject(cookies));
            }

            return cookies;
        }

        public void ClearCache()
        {
            _cache.Remove("cookies");
        }
    }
}
