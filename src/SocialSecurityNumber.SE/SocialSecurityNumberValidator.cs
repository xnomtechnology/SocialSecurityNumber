using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SocialSecurityNumber.SE.Exceptions;

namespace SocialSecurityNumber.SE
{
    public static class SocialSecurityNumberValidator
    {
        private static string RegEx =>
            "^(19|20)?[0-9]{2}[- ]?((0[0-9])|(10|11|12))[- ]?(([0-2][0-9])|(3[0-1])|(([7-8][0-9])|(6[1-9])|(9[0-1])))[- ]?[0-9]{4}$";
        public static bool ValidateSocialSecurityNumber(this string value)
        {
            var regEx = new Regex(RegEx);

            return regEx.IsMatch(value) && LuhnAlgorithm(value);
        }

        private static bool LuhnAlgorithm(string value)
        {
            var socialSecurityNumber = Regex.Replace(value, @"[^\d]", "");
            
            ValidateBirthStr();

            var controlNumber = int.Parse(socialSecurityNumber.Last().ToString());

            var valueArray =
                socialSecurityNumber.Length == 10
                    ? socialSecurityNumber.Substring(0, 9)
                    : socialSecurityNumber.Substring(2, 9);
            
            var pos = 0;
            var sum = 0;
            
            valueArray
                .ToCharArray()
                .Select(d => d - 48)
                .ToList()
                .ForEach(d =>
                {
                    var temp = d * (2 - (pos++ % 2));

                    if (temp > 9) temp -= 9;
                    
                    sum += temp;
                });

            var checksum = ((int) Math.Ceiling(sum / 10.0)) * 10 - sum;

            return checksum == controlNumber;


            void ValidateBirthStr()
            {
                switch (socialSecurityNumber.Length)
                {
                    case 12 when !DateTime.TryParseExact(socialSecurityNumber.Substring(0, 8), "yyyyMMdd",
                        CultureInfo.CurrentCulture, DateTimeStyles.None, out _):
                        throw new SocialSecurityNumberException($"Date is not valid");
                    case 10 when !DateTime.TryParseExact(socialSecurityNumber.Substring(0, 6), "yyMMdd",                         
                        CultureInfo.CurrentCulture, DateTimeStyles.None, out _):
                        throw new SocialSecurityNumberException($"Date is not valid");
                }
            }
        }
       
    }
}
