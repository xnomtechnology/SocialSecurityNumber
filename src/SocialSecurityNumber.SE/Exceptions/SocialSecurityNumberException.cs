using System;

namespace SocialSecurityNumber.SE.Exceptions
{
    public class SocialSecurityNumberException : Exception 
    {
        internal SocialSecurityNumberException(string message) : base(message) { }
    }
}
