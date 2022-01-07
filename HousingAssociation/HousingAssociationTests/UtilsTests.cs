using System;
using NUnit.Framework;
using System.Threading.Tasks;
using HousingAssociation.Utils;

namespace HousingAssociationTests
{
    public class UtilsTests
    {
        [Test]
        public async Task ShouldCreateTwoDifferentMd5Hashes()
        {
            // given
            var firstBytesArray = new byte[100];
            var secondBytesArray = new byte[100];
            
            Random rnd = new Random();
            rnd.NextBytes(firstBytesArray);
            rnd.NextBytes(secondBytesArray);
            
            // when

            var hash1 = HashGenerator.CalculateMd5StringFromBytes(firstBytesArray);
            var hash2 = HashGenerator.CalculateMd5StringFromBytes(secondBytesArray);
            
            // then
            Assert.AreNotEqual(hash1, hash2);
        }
        
        [Test]
        public async Task ShouldCreateEqualHashes()
        {
            // given
            var firstBytesArray = new byte[100];
            
            Random rnd = new Random();
            rnd.NextBytes(firstBytesArray);
            var secondBytesArray = firstBytesArray;
            
            // when

            var hash1 = HashGenerator.CalculateMd5StringFromBytes(firstBytesArray);
            var hash2 = HashGenerator.CalculateMd5StringFromBytes(secondBytesArray);
            
            // then
            Assert.AreEqual(hash1, hash2);
        }

    }
}