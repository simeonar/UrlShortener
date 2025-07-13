using Xunit;
using UrlShortener.Core.Services;

namespace UrlShortener.Core.Tests.Services
{
    public class Base62EncoderTests
    {
        [Theory]
        [InlineData(0, "000000")]
        [InlineData(1, "000001")]
        [InlineData(61, "00000z")]
        [InlineData(62, "000010")]
        [InlineData(123456789, null)] // just check roundtrip
        public void Encode_And_Decode_AreInverse(long value, string? expected)
        {
            var encoded = Base62Encoder.Encode(value).PadLeft(6, '0');
            var decoded = Base62Encoder.Decode(encoded);
            if (expected != null)
                Assert.Equal(expected, encoded);
            Assert.Equal(value, decoded);
        }

        [Fact]
        public void Decode_InvalidString_Throws()
        {
            Assert.Throws<System.ArgumentException>(() => Base62Encoder.Decode("!@#bad"));
        }
    }
}
