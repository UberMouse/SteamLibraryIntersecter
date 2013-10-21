using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SteamLibraryIntersecter
{
    public static class Extensions
    {
        public static bool IsNumerical(this string str)
        {
            return Regex.IsMatch(str, @"^\d+$");
        }
    }
}