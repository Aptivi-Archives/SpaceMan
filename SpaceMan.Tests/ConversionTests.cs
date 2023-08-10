/*
 * MIT License
 *
 * Copyright (c) 2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using NUnit.Framework;
using Shouldly;
using SpaceMan.Analysis;
using SpaceMan.References;
using System.IO;
using System.Text;

namespace SpaceMan.Tests
{
    public class ConversionTests
    {
        [Test]
        public void TestConvertSpacesNormal()
        {
            //                  v~~~~ This is a normal space
            string text = "Hello world!";
            byte[] expectedBytes = Encoding.UTF8.GetBytes(text);
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            byte[] result = SpaceConversionTools.ConvertSpaces(analysisResult);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.ShouldBe(expectedBytes);
            result.Length.ShouldBe(expectedBytes.Length);
        }

        [Test]
        public void TestConvertSpacesNonBreakingSpace()
        {
            //                  v~~~~ This is a non-breaking space
            string text = "Hello world!";
            byte[] expectedBytes = Encoding.UTF8.GetBytes("Hello world!");
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            byte[] result = SpaceConversionTools.ConvertSpaces(analysisResult);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.ShouldBe(expectedBytes);
            result.Length.ShouldBe(expectedBytes.Length);
            result.ShouldNotContain((byte)'\u00a0');
        }

        [Test]
        public void TestConvertSpacesMultipleNonBreakingSpaces()
        {
            //                  v~~~v~~~~~~~v~~v~~~v~~~~ These are the non-breaking spaces
            string text = "Hello and welcome to the world!";
            byte[] expectedBytes = Encoding.UTF8.GetBytes("Hello and welcome to the world!");
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            byte[] result = SpaceConversionTools.ConvertSpaces(analysisResult);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.ShouldBe(expectedBytes);
            result.Length.ShouldBe(expectedBytes.Length);
            result.ShouldNotContain((byte)'\u00a0');
        }

        [Test]
        public void TestConvertSpacesNonBreakingSpaceExplicit()
        {
            //                  vvvvvv~~~~ This is a non-breaking space
            string text = "Hello\u00a0world!";
            byte[] expectedBytes = Encoding.UTF8.GetBytes("Hello world!");
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            byte[] result = SpaceConversionTools.ConvertSpaces(analysisResult);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.ShouldBe(expectedBytes);
            result.Length.ShouldBe(expectedBytes.Length);
            result.ShouldNotContain((byte)'\u00a0');
        }

        [Test]
        public void TestConvertSpacesMultipleNonBreakingSpacesExplicit()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u00a0welcome\u00a0to\u00a0the\u00a0world!";
            byte[] expectedBytes = Encoding.UTF8.GetBytes("Hello and welcome to the world!");
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            byte[] result = SpaceConversionTools.ConvertSpaces(analysisResult);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.ShouldBe(expectedBytes);
            result.Length.ShouldBe(expectedBytes.Length);
            result.ShouldNotContain((byte)'\u00a0');
        }

        [Test]
        public void TestConvertSpacesWithBadSpacesExplicit()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~~ This is a bad space
                string text = $"Hello{whiteSpace}world!";
                byte[] expectedBytes = Encoding.UTF8.GetBytes("Hello world!");
                var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
                byte[] result = SpaceConversionTools.ConvertSpaces(analysisResult);
                result.ShouldNotBeNull();
                result.ShouldNotBeEmpty();
                result.ShouldBe(expectedBytes);
                result.Length.ShouldBe(expectedBytes.Length);
                result.ShouldNotContain((byte)whiteSpace);
            }
        }

        [Test]
        public void TestConvertSpacesWithMultipleBadSpacesExplicit()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~~~~vvvvvvvvvvvv~~vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~ These are bad spaces
                string text = $"Hello{whiteSpace}and{whiteSpace}welcome{whiteSpace}to{whiteSpace}the{whiteSpace}world!";
                byte[] expectedBytes = Encoding.UTF8.GetBytes("Hello and welcome to the world!");
                var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
                byte[] result = SpaceConversionTools.ConvertSpaces(analysisResult);
                result.ShouldNotBeNull();
                result.ShouldNotBeEmpty();
                result.ShouldBe(expectedBytes);
                result.Length.ShouldBe(expectedBytes.Length);
                result.ShouldNotContain((byte)whiteSpace);
            }
        }

        [Test]
        public void TestConvertSpacesMultipleDifferentSpacesExplicit()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u200Bwelcome\u2008to\u200Bthe\u00a0world!";
            byte[] expectedBytes = Encoding.UTF8.GetBytes("Hello and welcome to the world!");
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            byte[] result = SpaceConversionTools.ConvertSpaces(analysisResult);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.ShouldBe(expectedBytes);
            result.Length.ShouldBe(expectedBytes.Length);
            result.ShouldNotContain((byte)'\u00a0');
            result.ShouldNotBeOneOf(Spaces.badSpaces["ZERO WIDTH SPACE"]);
            result.ShouldNotBeOneOf(Spaces.badSpaces["PUNCTUATION SPACE"]);
        }

        [Test]
        public void TestConvertSpacesNormalToText()
        {
            //                  v~~~~ This is a normal space
            string text = "Hello world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            string result = SpaceConversionTools.ConvertSpacesToString(analysisResult);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesNonBreakingSpaceToText()
        {
            //                  v~~~~ This is a non-breaking space
            string text = "Hello world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            string result = SpaceConversionTools.ConvertSpacesToString(analysisResult);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesMultipleNonBreakingSpacesToText()
        {
            //                  v~~~v~~~~~~~v~~v~~~v~~~~ These are the non-breaking spaces
            string text = "Hello and welcome to the world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            string result = SpaceConversionTools.ConvertSpacesToString(analysisResult);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesNonBreakingSpaceExplicitToText()
        {
            //                  vvvvvv~~~~ This is a non-breaking space
            string text = "Hello\u00a0world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            string result = SpaceConversionTools.ConvertSpacesToString(analysisResult);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesMultipleNonBreakingSpacesExplicitToText()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u00a0welcome\u00a0to\u00a0the\u00a0world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            string result = SpaceConversionTools.ConvertSpacesToString(analysisResult);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesWithBadSpacesExplicitToText()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~~ This is a bad space
                string text = $"Hello{whiteSpace}world!";
                string expectedResult = "Hello world!";
                var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
                string result = SpaceConversionTools.ConvertSpacesToString(analysisResult);
                result.ShouldNotBeNullOrEmpty();
                result.ShouldBe(expectedResult);
            }
        }

        [Test]
        public void TestConvertSpacesWithMultipleBadSpacesExplicitToText()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~~~~vvvvvvvvvvvv~~vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~ These are bad spaces
                string text = $"Hello{whiteSpace}and{whiteSpace}welcome{whiteSpace}to{whiteSpace}the{whiteSpace}world!";
                string expectedResult = "Hello and welcome to the world!";
                var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
                string result = SpaceConversionTools.ConvertSpacesToString(analysisResult);
                result.ShouldNotBeNullOrEmpty();
                result.ShouldBe(expectedResult);
            }
        }

        [Test]
        public void TestConvertSpacesMultipleDifferentSpacesExplicitToText()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u200Bwelcome\u2008to\u200Bthe\u00a0world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            string result = SpaceConversionTools.ConvertSpacesToString(analysisResult);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesNormalToStream()
        {
            //                  v~~~~ This is a normal space
            string text = "Hello world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            var stream = new MemoryStream();
            SpaceConversionTools.ConvertSpacesTo(analysisResult, stream);
            byte[] array = new byte[stream.Length];
            stream.ShouldNotBeNull();
            stream.Length.ShouldBe(expectedResult.Length);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(array, 0, array.Length);
            string result = Encoding.UTF8.GetString(array);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesNonBreakingSpaceToStream()
        {
            //                  v~~~~ This is a non-breaking space
            string text = "Hello world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            var stream = new MemoryStream();
            SpaceConversionTools.ConvertSpacesTo(analysisResult, stream);
            byte[] array = new byte[stream.Length];
            stream.ShouldNotBeNull();
            stream.Length.ShouldBe(expectedResult.Length);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(array, 0, array.Length);
            string result = Encoding.UTF8.GetString(array);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesMultipleNonBreakingSpacesToStream()
        {
            //                  v~~~v~~~~~~~v~~v~~~v~~~~ These are the non-breaking spaces
            string text = "Hello and welcome to the world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            var stream = new MemoryStream();
            SpaceConversionTools.ConvertSpacesTo(analysisResult, stream);
            byte[] array = new byte[stream.Length];
            stream.ShouldNotBeNull();
            stream.Length.ShouldBe(expectedResult.Length);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(array, 0, array.Length);
            string result = Encoding.UTF8.GetString(array);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesNonBreakingSpaceExplicitToStream()
        {
            //                  vvvvvv~~~~ This is a non-breaking space
            string text = "Hello\u00a0world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            var stream = new MemoryStream();
            SpaceConversionTools.ConvertSpacesTo(analysisResult, stream);
            byte[] array = new byte[stream.Length];
            stream.ShouldNotBeNull();
            stream.Length.ShouldBe(expectedResult.Length);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(array, 0, array.Length);
            string result = Encoding.UTF8.GetString(array);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesMultipleNonBreakingSpacesExplicitToStream()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u00a0welcome\u00a0to\u00a0the\u00a0world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            var stream = new MemoryStream();
            SpaceConversionTools.ConvertSpacesTo(analysisResult, stream);
            byte[] array = new byte[stream.Length];
            stream.ShouldNotBeNull();
            stream.Length.ShouldBe(expectedResult.Length);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(array, 0, array.Length);
            string result = Encoding.UTF8.GetString(array);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesWithBadSpacesExplicitToStream()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~~ This is a bad space
                string text = $"Hello{whiteSpace}world!";
                string expectedResult = "Hello world!";
                var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
                var stream = new MemoryStream();
                SpaceConversionTools.ConvertSpacesTo(analysisResult, stream);
                byte[] array = new byte[stream.Length];
                stream.ShouldNotBeNull();
                stream.Length.ShouldBe(expectedResult.Length);
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(array, 0, array.Length);
                string result = Encoding.UTF8.GetString(array);
                result.ShouldNotBeNullOrEmpty();
                result.ShouldBe(expectedResult);
            }
        }

        [Test]
        public void TestConvertSpacesWithMultipleBadSpacesExplicitToStream()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~~~~vvvvvvvvvvvv~~vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~ These are bad spaces
                string text = $"Hello{whiteSpace}and{whiteSpace}welcome{whiteSpace}to{whiteSpace}the{whiteSpace}world!";
                string expectedResult = "Hello and welcome to the world!";
                var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
                var stream = new MemoryStream();
                SpaceConversionTools.ConvertSpacesTo(analysisResult, stream);
                byte[] array = new byte[stream.Length];
                stream.ShouldNotBeNull();
                stream.Length.ShouldBe(expectedResult.Length);
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(array, 0, array.Length);
                string result = Encoding.UTF8.GetString(array);
                result.ShouldNotBeNullOrEmpty();
                result.ShouldBe(expectedResult);
            }
        }

        [Test]
        public void TestConvertSpacesMultipleDifferentSpacesExplicitToStream()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u200Bwelcome\u2008to\u200Bthe\u00a0world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            var stream = new MemoryStream();
            SpaceConversionTools.ConvertSpacesTo(analysisResult, stream);
            byte[] array = new byte[stream.Length];
            stream.ShouldNotBeNull();
            stream.Length.ShouldBe(expectedResult.Length);
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(array, 0, array.Length);
            string result = Encoding.UTF8.GetString(array);
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesNormalToFile()
        {
            //                  v~~~~ This is a normal space
            string text = "Hello world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            SpaceConversionTools.ConvertSpacesTo(analysisResult, "file.txt");
            string result = File.ReadAllText("file.txt");
            File.Delete("file.txt");
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesNonBreakingSpaceToFile()
        {
            //                  v~~~~ This is a non-breaking space
            string text = "Hello world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            SpaceConversionTools.ConvertSpacesTo(analysisResult, "file.txt");
            string result = File.ReadAllText("file.txt");
            File.Delete("file.txt");
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesMultipleNonBreakingSpacesToFile()
        {
            //                  v~~~v~~~~~~~v~~v~~~v~~~~ These are the non-breaking spaces
            string text = "Hello and welcome to the world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            SpaceConversionTools.ConvertSpacesTo(analysisResult, "file.txt");
            string result = File.ReadAllText("file.txt");
            File.Delete("file.txt");
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesNonBreakingSpaceExplicitToFile()
        {
            //                  vvvvvv~~~~ This is a non-breaking space
            string text = "Hello\u00a0world!";
            string expectedResult = "Hello world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            SpaceConversionTools.ConvertSpacesTo(analysisResult, "file.txt");
            string result = File.ReadAllText("file.txt");
            File.Delete("file.txt");
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesMultipleNonBreakingSpacesExplicitToFile()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u00a0welcome\u00a0to\u00a0the\u00a0world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            SpaceConversionTools.ConvertSpacesTo(analysisResult, "file.txt");
            string result = File.ReadAllText("file.txt");
            File.Delete("file.txt");
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public void TestConvertSpacesWithBadSpacesExplicitToFile()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~~ This is a bad space
                string text = $"Hello{whiteSpace}world!";
                string expectedResult = "Hello world!";
                var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
                SpaceConversionTools.ConvertSpacesTo(analysisResult, "file.txt");
                string result = File.ReadAllText("file.txt");
                File.Delete("file.txt");
                result.ShouldNotBeNullOrEmpty();
                result.ShouldBe(expectedResult);
            }
        }

        [Test]
        public void TestConvertSpacesWithMultipleBadSpacesExplicitToFile()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~~~~vvvvvvvvvvvv~~vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~ These are bad spaces
                string text = $"Hello{whiteSpace}and{whiteSpace}welcome{whiteSpace}to{whiteSpace}the{whiteSpace}world!";
                string expectedResult = "Hello and welcome to the world!";
                var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
                SpaceConversionTools.ConvertSpacesTo(analysisResult, "file.txt");
                string result = File.ReadAllText("file.txt");
                File.Delete("file.txt");
                result.ShouldNotBeNullOrEmpty();
                result.ShouldBe(expectedResult);
            }
        }

        [Test]
        public void TestConvertSpacesMultipleDifferentSpacesExplicitToFile()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u200Bwelcome\u2008to\u200Bthe\u00a0world!";
            string expectedResult = "Hello and welcome to the world!";
            var analysisResult = SpaceAnalysisTools.AnalyzeSpaces(text);
            SpaceConversionTools.ConvertSpacesTo(analysisResult, "file.txt");
            string result = File.ReadAllText("file.txt");
            File.Delete("file.txt");
            result.ShouldNotBeNullOrEmpty();
            result.ShouldBe(expectedResult);
        }
    }
}
