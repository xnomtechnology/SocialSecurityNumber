using System.Collections.Generic;
using System.Globalization;
using SocialSecurityNumber.SE.Exceptions;
using Xunit;

namespace SocialSecurityNumber.SE.Test
{

    public class SocialSecurityNumberEqualUnitTest
    {
        [Fact]
        public void SocialSecurityNumber_CompareTest_NotEqual()
        {
            var testSsnObj1 = new SocialSecurityNumber("671221-2528");
            var testSsnObj2 = new SocialSecurityNumber("790118-0526");

            var result = testSsnObj1.Equals(testSsnObj2);

            Assert.False(result);
        }

        [Fact]
        public void SocialSecurityNumber_CompareTest_Equal()
        {
            var testSsnObj1 = new SocialSecurityNumber("790118-0526");
            var testSsnObj2 = new SocialSecurityNumber("790118-0526");

            var result = testSsnObj1.Equals(testSsnObj2);

            Assert.True(result);
        }
    }

}
