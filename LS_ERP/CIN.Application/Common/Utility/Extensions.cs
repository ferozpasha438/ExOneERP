using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application
{
    public static class Extensions
    {

        public static decimal GetMeters(double lat1, double lon1, double lat2, double lon2)
        {


            //Declare @source geography = geography::Point(17.425650605008894, 78.43642906284583, 4326)
            //Declare @destination geography = geography::Point(17.425647, 78.436594, 4326)
            //Select @source.STDistance(@destination) as Meters
            //Select @source.STDistance(@destination) / 1000 as Kilometers
            //Select @source.STDistance(@destination) / 1609.344 as Miles



            // generally used geo measurement function
            var R = 6378.137; // Radius of earth in KM
            var dLat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;
            var dLon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return (decimal)d * 1000; // meters
        }

        //public static decimal? TwoDecimals(this decimal? value) => Decimal.Round((decimal)value, 2);
        //public static decimal? TwoDecimals(this decimal value) => Decimal.Round((decimal)value, 2);
        public static string ToCommaInvarient(this decimal? decValue) => string.Format(CultureInfo.InvariantCulture, "{0:#,##0.##}", decValue).ToString(CultureInfo.InvariantCulture);
        public static string TwoDecInvarient(this decimal? decValue) => decValue is not null ? decValue.Value.ToString("0.00", CultureInfo.InvariantCulture) : "0.00";
        public static bool DbLike(this DbFunctions dbFunctions, string column, string search) => dbFunctions.Like(column, "%" + search + "%");
        public static string DecToString(this decimal? value) => Convert.ToString(value);
        /// <summary>
        /// Returns true if there is value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(this string value) => !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
        public static DateTime GetDateOnly(this DateTime? dateValue) => EF.Functions.DateFromParts(dateValue.Value.Year, dateValue.Value.Month, dateValue.Value.Day);
        public static bool IsArab(this string culture) => culture.ToLower() == "ar";
        public static decimal DecValue(this decimal? value) => value ?? 0;

        //public static string HashPassword(this string password)
        //{

        //    byte[] salt;
        //    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        //    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
        //    byte[] hash = pbkdf2.GetBytes(20);

        //    byte[] hashBytes = new byte[36];
        //    Array.Copy(salt, 0, hashBytes, 0, 16);
        //    Array.Copy(hash, 0, hashBytes, 16, 20);

        //    string savedPasswordHash = Convert.ToBase64String(hashBytes);

        //    /* Fetch the stored value */
        //    string savedPasswordHash = DBContext.GetUser(u => u.UserName == user).Password;
        //    /* Extract the bytes */
        //    byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
        //    /* Get the salt */
        //    byte[] salt = new byte[16];
        //    Array.Copy(hashBytes, 0, salt, 0, 16);
        //    /* Compute the hash on the password the user entered */
        //    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        //    byte[] hash = pbkdf2.GetBytes(20);
        //    /* Compare the results */
        //    for (int i = 0; i < 20; i++)
        //        if (hashBytes[i + 16] != hash[i])
        //            throw new UnauthorizedAccessException();


        //    var data = Encoding.ASCII.GetBytes(password);
        //    var sha1 = new SHA1CryptoServiceProvider();
        //    var sha1data = sha1.ComputeHash(data);
        //    return new ASCIIEncoding().GetString(sha1data);
        //}

        //public static string DeHashPassword(this string password)
        //{
        //    var data = Encoding.ASCII.GetBytes(password);
        //    var sha1 = new SHA1CryptoServiceProvider();
        //    var sha1data = sha1.ComputeHash(data);
        //    return new ASCIIEncoding().GetString(sha1data);
        //}

        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }
    }
}
