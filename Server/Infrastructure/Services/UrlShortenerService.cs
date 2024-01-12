using Domain.Services;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly String ALPHABET;
        private readonly int BASE;

        public UrlShortenerService()
        {
            ALPHABET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            BASE = ALPHABET.Count();
        }

        public string GenerageShortUrl(string url)
        {
            int num = 0;
            for ( int i = 0; i < url.Count(); i++)
                num = num * BASE + ALPHABET.IndexOf(url[i]);
            return Math.Abs(num).ToString();
        }
    }
}
