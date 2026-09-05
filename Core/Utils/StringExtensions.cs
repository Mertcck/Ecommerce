using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class StringExtensions
    {
        public static string ToUrl(this string text)
        {
            if (!string.IsNullOrWhiteSpace(text.Trim()))
            {
                string url = text.Trim()
                                 .Replace("ı", "i")
                                 .Replace("İ", "i")
                                 .Replace("I", "i")
                                 .Replace("ç", "c")
                                 .Replace("Ç", "c")
                                 .Replace("ü", "u")
                                 .Replace("Ü", "u")
                                 .Replace("ğ", "g")
                                 .Replace("Ğ", "g")
                                 .Replace("ö", "o")
                                 .Replace("Ö", "o")
                                 .Replace("ş", "s")
                                 .Replace("Ş", "s");

                url = url.ToLowerInvariant();
                url = url.Replace(" ", "-");
                url = Regex.Replace(url, @"[^a-z0-9\-]", "");
                url = Regex.Replace(url, @"\-+", "-");

                return url.Trim('-');
            }
            return string.Empty;
        }
    }
}
