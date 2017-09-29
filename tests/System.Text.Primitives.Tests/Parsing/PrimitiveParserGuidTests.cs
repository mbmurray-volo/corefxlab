// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Globalization;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserTests
    {
        Guid guidWithAllNonZeroDigits = new Guid("12345678-9abc-def1-2345-123456789abc");

        [Theory]
        [InlineData(null)]
        [InlineData("D")]
        [InlineData("N")]
        [InlineData("P")]
        [InlineData("B")]
        public unsafe void GuidParsingUtf8(string format)
        {
            var parsedFormat = ParsedFormat.Parse(format);
            var random = new Random(1000);
            var guidBytes = new byte[16];

            var expected = guidWithAllNonZeroDigits;
            for (int i = 0; i < 100; i++)
            {
                var expectedString = expected.ToString(format, CultureInfo.InvariantCulture);
                var utf8Bytes = Text.Encoding.UTF8.GetBytes(expectedString);

                Assert.True(Parsers.Utf8.TryParseGuid(utf8Bytes, out Guid parsed, out int bytesConsumed, parsedFormat));
                Assert.Equal(expected, parsed);
                Assert.Equal(expectedString.Length, bytesConsumed);

                random.NextBytes(guidBytes);
                expected = new Guid(guidBytes);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("D")]
        [InlineData("N")]
        [InlineData("P")]
        [InlineData("B")]
        public unsafe void GuidParsingUtf16(string format)
        {
            var parsedFormat = ParsedFormat.Parse(format);
            var random = new Random(1000);
            var guidBytes = new byte[16];

            var expected = guidWithAllNonZeroDigits;
            for (int i = 0; i < 100; i++)
            {
                string expectedString = expected.ToString(format, CultureInfo.InvariantCulture);

                Assert.True(Parsers.Utf16.TryParseGuid(expectedString.ToCharArray(), out Guid parsed, out int charactersConsumed, parsedFormat));
                Assert.Equal(expected, parsed);
                Assert.Equal(expectedString.Length, charactersConsumed);

                random.NextBytes(guidBytes);
                expected = new Guid(guidBytes);
            }
        }
    }
}