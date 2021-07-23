using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SocialSecurityNumber.SE.Exceptions;

namespace SocialSecurityNumber.SE
{
    public sealed class SocialSecurityNumber : IComparable, IComparable<SocialSecurityNumber>, IEquatable<SocialSecurityNumber>, IFormattable
    {
        private DateTime BirthDate;
        private int Birthnumber;
        private int Checksum;

        
        public Gender Gender { get; private set; }

        public SocialSecurityNumber(string value)
        {
            if (!value.ValidateSocialSecurityNumber())
            {
                throw new SocialSecurityNumberException("Social security number is invalid");
            }

            string tmpSsn = Regex.Replace(value, @"[^\d]", string.Empty);
            
            SetBirthDate(tmpSsn);
            SetBirthnumber(tmpSsn);
            SetChecksum(tmpSsn);

            SetGender(tmpSsn);

        
            void SetBirthDate(string value) =>
                BirthDate = value.Length switch
                {
                    12 when DateTime.TryParseExact(value.Substring(0, 8), "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _)
                        => DateTime.ParseExact(value.Substring(0, 8), "yyyyMMdd", CultureInfo.CurrentCulture),
                    10 when DateTime.TryParseExact(value.Substring(0, 6), "yyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _)
                        => DateTime.ParseExact(value.Substring(0, 6), "yyMMdd", CultureInfo.CurrentCulture),
                    _ => throw new SocialSecurityNumberException("Invalid social security number")
                };
            
            void SetBirthnumber(string value) => Birthnumber = int.Parse(value.Substring(value.Length - 4, 3));

            void SetChecksum(string value) => Checksum = int.Parse(value.Substring(value.Length- 1, 1));

            void SetGender(string value) => Gender = int.Parse(value.Substring(value.Length - 4, 3)) % 2 == 1 ? Gender.Male : Gender.Female;
        }
 

        /// <summary>
        /// Define format:YYMMDDNNNC\n
        /// Define format:YYMMDD-NNNC\n
        /// Define format:YYYYMMDDNNNC\n
        /// Define format:YYYYMMDD-NNNC
        /// </summary>
        /// <param name="YYMMDDNNNC">Define format:YYMMDDNNNC</param>
        /// <param name="YYMMDD-NNNC">Define format:YYMMDD-NNNC</param>
        /// <param name="YYYYMMDDNNNC">Define format:YYYYMMDDNNNC</param>
        /// <param name="YYYYMMDD-NNNC">Define format:YYYYMMDD-NNNC</param>
        /// <returns>SocialSecurityNumber</returns>
        public static SocialSecurityNumber Parse(string value)
        {
            if (!value.ValidateSocialSecurityNumber())
            {
                throw new ArgumentException($"Given value cant be parsed as social security number");
            }

            return new SocialSecurityNumber(value);
        }

        public override string ToString() => ToString("yyyyMMdd-nnnc", CultureInfo.CurrentCulture);
        

        public int CompareTo(object? obj)
        {
            return BirthDate.CompareTo(obj);
        }

        public int CompareTo(SocialSecurityNumber? other)
        {
            return CompareTo(other);
        }

        public bool Equals(SocialSecurityNumber? other)
        {
            return this.Equals(other);
        }


        /// <summary>
        /// standard SocialSecurityNumber supported format
        /// <br>supported format:  yyMMdd - datetime</br>
        /// <br>supported format:  nnn - birthnumber</br>
        /// <br>supported format:  c - Checksum number</br>        
        /// </summary>    
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var value = string.Empty;

            if (format != null && (format.Contains("y") || format.Contains("M") || format.Contains("d")))
            {
                value += BirthDate.ToString(format, formatProvider);
            }

            if (format != null && format.Contains("n", StringComparison.CurrentCultureIgnoreCase))
            {
                value = ParsBirthnumber(value);
            }

            if (format != null && format.Contains("c", StringComparison.CurrentCultureIgnoreCase))
            {
                var parsedChecksum = Checksum
                    .ToString()
                    .Substring(0, format.Split('c').Length - 1);

                value = value.Replace("c", parsedChecksum);
            }

            return value;

            string ParsBirthnumber(string s)
            {
                var pos = 0;
                var result = string.Empty;

                s.ToCharArray()
                    .Select(_=>_.ToString())
                    .ToList()
                    .ForEach(_ =>
                    {
                        result += _.Equals("n", StringComparison.CurrentCultureIgnoreCase) 
                            ? Birthnumber.ToString().Substring(pos++, 1)
                            : _;
                    });
                return result;
            }
        }
    }
}
