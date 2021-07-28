using System.Collections.Generic;
using System.Globalization;
using SocialSecurityNumber.SE.Exceptions;
using Xunit;

namespace SocialSecurityNumber.SE.Test
{

    public class SocialSecurityNumberCompareUnitTest
    {
        [Fact]
        public void SocialSecurityNumber_CompareTest_Success1()
        {
            var testSsnObj1 = new SocialSecurityNumber("671221-2528");
            var testSsnObj2 = new SocialSecurityNumber("790118-0526");

            var result = testSsnObj1.CompareTo(testSsnObj2);

            Assert.Equal(result, -1);
        }

        [Fact]
        public void SocialSecurityNumber_CompareTest_Success2()
        {
            var testSsnObj1 = new SocialSecurityNumber("671221-2528");
            var testSsnObj2 = new SocialSecurityNumber("790118-0526");

            var result = testSsnObj2.CompareTo(testSsnObj1);

            Assert.Equal(result, 1);
        }

        [Fact]
        public void SocialSecurityNumber_CompareTest_Equals()
        {
            var testSsnObj1 = new SocialSecurityNumber("790118-0526");
            var testSsnObj2 = new SocialSecurityNumber("790118-0526");

            var result = testSsnObj1.CompareTo(testSsnObj2);

            Assert.Equal(result, 0);
        }
    }

}
