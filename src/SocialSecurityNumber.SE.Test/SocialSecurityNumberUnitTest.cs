using System.Collections.Generic;
using System.Globalization;
using SocialSecurityNumber.SE.Exceptions;
using Xunit;

namespace SocialSecurityNumber.SE.Test
{
    
    public class SocialSecurityNumberUnitTest
    {
        [Fact]
        public void SocialSecurityNumber_ToStringTest_default()
        {
            var testSsn = "671221-2528";
            var testSsnObj = SocialSecurityNumber.Parse(testSsn);
            var result = testSsnObj.ToString();

            Assert.Equal("19671221-2528", result);
        }



        [Fact]
        public void SocialSecurityNumber_ToStringTest_laterCentury_default()
        {
            var testSsn = "121212-1212";
            var testSsnObj = SocialSecurityNumber.Parse(testSsn);
            var result = testSsnObj.ToString();

            Assert.Equal("20121212-1212", result);
        }


        [Fact]
        public void SocialSecurityNumber_ToStringTest()
        {
            var testSsn = "671221-2528";
            var testSsnObj = SocialSecurityNumber.Parse(testSsn);
            var result = testSsnObj.ToString("yyyyMMdd-nnnc", CultureInfo.CurrentCulture);

            Assert.Equal ("19671221-2528", result);
        }

        [Fact]
        public void SocialSecurityNumber_ToString_SuccessTest1()
        {
            var ssn = "671221-2528";
            var socialSecurityNumber = SocialSecurityNumber.Parse(ssn);
            var result = socialSecurityNumber.ToString("yyyyMMddnnnc", CultureInfo.CurrentCulture);

            Assert.Equal("196712212528", result);
        }

        [Fact]
        public void SocialSecurityNumber_ToString_SuccessTest2()
        {
            var ssn = "671221-2528";
            var socialSecurityNumber = SocialSecurityNumber.Parse(ssn);
            var result = socialSecurityNumber.ToString("yyyyMMdd-.c", CultureInfo.CurrentCulture);

            Assert.Equal("19671221-.8", result);
        }

        [Fact]
        public void SocialSecurityNumber_ToString_SuccessTest3()
        {
            var ssn = "671221-2528";
            var socialSecurityNumber = SocialSecurityNumber.Parse(ssn);
            var result = socialSecurityNumber.ToString("yyMMdd-nnn-c", CultureInfo.CurrentCulture);

            Assert.Equal("671221-252-8", result);
        }

        [Fact]
        public void SocialSecurityNumber_ToString_SuccessTest4()
        {
            var ssn = "671221-2528";
            var socialSecurityNumber = SocialSecurityNumber.Parse(ssn);
            var result = socialSecurityNumber.ToString("MMdd-yyyy-nnn-c", CultureInfo.CurrentCulture);

            Assert.Equal("1221-1967-252-8", result);
        }

        [Fact]
        public void SocialSecurityNumber_ToString_SuccessTest5()
        {
            var ssn = "671221-2528";
            var socialSecurityNumber = SocialSecurityNumber.Parse(ssn);
            var result = socialSecurityNumber.ToString("MMdd-yyyy-nn-n-c", CultureInfo.CurrentCulture);

            Assert.Equal("1221-1967-25-2-8", result);
        }

        [Fact]
        public void SocialSecurityNumber_CreateObject_SuccessTest()
        {
            var ssn = "671221-2528";
            var socialSecurityNumber = new SocialSecurityNumber(ssn);
            var result = socialSecurityNumber.ToString("MMdd-yyyy-nn-n-c", CultureInfo.CurrentCulture);

            Assert.Equal("1221-1967-25-2-8", result);
        }

        [Fact]
        public void SocialSecurityNumber_CreateObject_FailTest()
        {
            var value = "679921-2528";
            Assert.Throws<SocialSecurityNumberException>(() => new SocialSecurityNumber(value));
        }
    }
}
