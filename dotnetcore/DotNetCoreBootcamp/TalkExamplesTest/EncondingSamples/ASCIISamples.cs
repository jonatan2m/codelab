using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Text;
using Xunit;

namespace TalkExamplesTest.EncondingSamples
{
    public class ASCIISamples
    {
        [Fact]
        public void Sample1()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var text = "Aqui tem uma maçã";

            //var normalizedString1 = text.Normalize(NormalizationForm.FormC);
            //var normalizedString2 = text.Normalize(NormalizationForm.FormD);
            //var normalizedString3 = text.Normalize(NormalizationForm.FormKC);
            //var normalizedString4 = text.Normalize(NormalizationForm.FormKD);

            //Console.WriteLine(normalizedString1 +" "+ normalizedString2 + " " + normalizedString3 + " " + normalizedString4);

            //var stringBuilder = new StringBuilder();

            //foreach (var c in normalizedString2)
            //{
            //    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            //    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            //    {
            //        stringBuilder.Append(c);
            //    }
            //}
            
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(text);
            string result = System.Text.Encoding.UTF8.GetString(tempBytes);
            //var result =  stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            var expected = "Aqui tem uma maca";

            Assert.Equal(expected, result);
        }
    }
}
