using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SocialSecurityNumber.SE.Exceptions;

namespace SocialSecurityNumber.SE
{   
    public sealed class SocialSecurityNumber : IComparable<SocialSecurityNumber>, IEquatable<SocialSecurityNumber>, IFormattable
    {
        private DateTime _birthDate;
        private int _birthnumber;
        private int _checksum;

        
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

        
            void SetBirthDate(string birthDate) =>
                _birthDate = birthDate.Length switch
                {
                    12 when DateTime.TryParseExact(birthDate.Substring(0, 8), "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _)
                        => DateTime.ParseExact(birthDate.Substring(0, 8), "yyyyMMdd", CultureInfo.CurrentCulture),
                    10 when DateTime.TryParseExact(birthDate.Substring(0, 6), "yyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _)
                        => DateTime.ParseExact(birthDate.Substring(0, 6), "yyMMdd", CultureInfo.CurrentCulture),
                    _ => throw new SocialSecurityNumberException("Invalid social security number")
                };
            
            void SetBirthnumber(string birthNumber) => _birthnumber = int.Parse(birthNumber.Substring(birthNumber.Length - 4, 3));

            void SetChecksum(string checksum) => _checksum = int.Parse(checksum.Substring(checksum.Length- 1, 1));

            void SetGender(string gender) => Gender = int.Parse(gender.Substring(gender.Length - 4, 3)) % 2 == 1 ? Gender.Male : Gender.Female;
        }

        /// <summary>
        /// standard SocialSecurityNumber supported format
        /// <br>supported format:  YYMMDDNNNC</br>
        /// <br>supported format:  YYMMDD-NNNC</br>
        /// <br>supported format:  YYYYMMDDNNNC</br>
        /// <br>supported format:  YYYYMMDD-NNNC</br>
        /// </summary>
        /// <param name="value"></param>
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
                value += _birthDate.ToString(format, formatProvider);
            }

            if (format != null && format.Contains("n", StringComparison.CurrentCultureIgnoreCase))
            {
                value = ParsBirthnumber(value);
            }

            if (format != null && format.Contains("c", StringComparison.CurrentCultureIgnoreCase))
            {
                var parsedChecksum = _checksum
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
                            ? _birthnumber.ToString().Substring(pos++, 1)
                            : _;
                    });
                return result;
            }
        }

        public bool Equals(SocialSecurityNumber? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _birthDate.Equals(other._birthDate) && _birthnumber == other._birthnumber && _checksum == other._checksum && Gender == other.Gender;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is SocialSecurityNumber other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_birthDate, _birthnumber, _checksum, (int) Gender);
        }

        public int CompareTo(SocialSecurityNumber? other)
        {
            if (ReferenceEquals(this, other)) return 0;

            if (ReferenceEquals(null, other)) return 1;

            var birthDateComparison = _birthDate.CompareTo(other._birthDate);
            if (birthDateComparison != 0) return birthDateComparison;
            
            var birthnumberComparison = _birthnumber.CompareTo(other._birthnumber);
            if (birthnumberComparison != 0) return birthnumberComparison;

            var checksumComparison = _checksum.CompareTo(other._checksum);
            if (checksumComparison != 0) return checksumComparison;
            
            return Gender.CompareTo(other.Gender);
        }
    }
}
