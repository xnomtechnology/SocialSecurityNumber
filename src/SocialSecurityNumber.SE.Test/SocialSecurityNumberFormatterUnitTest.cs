using System.Collections.Generic;
using Xunit;

namespace SocialSecurityNumber.SE.Test
{
    public class SocialSecurityNumberFormatterUnitTest
    {

        [Fact]
        public void SocialSecurityNumberFormatterUnitTest_Standard_Success()
        {
            var testCases = new List<string>()
            {
                "570825-8412",
                "011005-5894",
                "740101-5354"
            };

            testCases
                .ForEach(_=> Assert.True(_.ValidateSocialSecurityNumber()));
        }


        [Fact]
        public void SocialSecurityNumberFormatterUnitTest_StandardWithouthyphen_Success()
        {
            var testCases = new List<string>()
            {
                "5708258412",
                "0110055894",
                "7401015354"
            };

            testCases
                .ForEach(_ => Assert.True(_.ValidateSocialSecurityNumber()));

        }

        [Fact]
        public void SocialSecurityNumberFormatterUnitTest_StandardWithCentury_Success()
        {
            var testCases = new List<string>()
            {
                "19570825-8412",
                "20011005-5894",
                "19740101-5354"
            };

            testCases
                .ForEach(_ => Assert.True(_.ValidateSocialSecurityNumber()));

        }


        [Fact]
        public void SocialSecurityNumberFormatterUnitTest_Standard_Fail()
        {

            var testCases = new List<string>()
            {
                "570825-8419",
                "011005-5899",
                "740101-5359"
            };

            testCases
                .ForEach(_ => Assert.False(_.ValidateSocialSecurityNumber()));

        }


        [Fact]
        public void SocialSecurityNumberFormatterUnitTest_StandardWithCentury_Fail()
        {


            var testCases = new List<string>()
            {
                "18570825-8412",
                "21011005-5894",
                "17740101-5354"
            };

            testCases
                .ForEach(_ => Assert.False(_.ValidateSocialSecurityNumber()));

        }
    }
}
