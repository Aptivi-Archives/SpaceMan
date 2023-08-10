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
using System.Text;

namespace SpaceMan.Tests
{
    public class AnalysisTests
    {
        [Test]
        public void TestAnalyzeSpacesNormal()
        {
            //                  v~~~~ This is a normal space
            string text = "Hello world!";
            int length = Encoding.UTF8.GetByteCount(text);
            var result = SpaceAnalysisTools.AnalyzeSpaces(text);
            result.ShouldNotBeNull();
            result.ResultingStream.ShouldNotBeNull();
            result.ResultingStream.Length.ShouldBe(length);
            result.FalseSpaces.ShouldBeEmpty();
        }

        [Test]
        public void TestAnalyzeSpacesNonBreakingSpace()
        {
            //                  v~~~~ This is a non-breaking space
            string text = "Hello world!";
            int length = Encoding.UTF8.GetByteCount(text);
            var result = SpaceAnalysisTools.AnalyzeSpaces(text);
            result.ShouldNotBeNull();
            result.ResultingStream.ShouldNotBeNull();
            result.ResultingStream.Length.ShouldBe(length);
            result.FalseSpaces.ShouldNotBeEmpty();
            result.FalseSpaces[0].Item1.ShouldBe('\u00a0');
            result.FalseSpaces[0].Item2.ShouldBe("NON-BREAKING SPACE");
        }

        [Test]
        public void TestAnalyzeSpacesMultipleNonBreakingSpaces()
        {
            //                  v~~~v~~~~~~~v~~v~~~v~~~~ These are the non-breaking spaces
            string text = "Hello and welcome to the world!";
            int length = Encoding.UTF8.GetByteCount(text);
            var result = SpaceAnalysisTools.AnalyzeSpaces(text);
            result.ShouldNotBeNull();
            result.ResultingStream.ShouldNotBeNull();
            result.ResultingStream.Length.ShouldBe(length);
            result.FalseSpaces.ShouldNotBeEmpty();
            result.FalseSpaces.Length.ShouldBe(1);
            foreach (var space in result.FalseSpaces)
            {
                space.Item1.ShouldBe('\u00a0');
                space.Item2.ShouldBe("NON-BREAKING SPACE");
            }
        }

        [Test]
        public void TestAnalyzeSpacesNonBreakingSpaceExplicit()
        {
            //                  vvvvvv~~~~ This is a non-breaking space
            string text = "Hello\u00a0world!";
            int length = Encoding.UTF8.GetByteCount(text);
            var result = SpaceAnalysisTools.AnalyzeSpaces(text);
            result.ShouldNotBeNull();
            result.ResultingStream.ShouldNotBeNull();
            result.ResultingStream.Length.ShouldBe(length);
            result.FalseSpaces.ShouldNotBeEmpty();
            result.FalseSpaces[0].Item1.ShouldBe('\u00a0');
            result.FalseSpaces[0].Item2.ShouldBe("NON-BREAKING SPACE");
        }

        [Test]
        public void TestAnalyzeSpacesMultipleNonBreakingSpacesExplicit()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the non-breaking spaces
            string text = "Hello\u00a0and\u00a0welcome\u00a0to\u00a0the\u00a0world!";
            int length = Encoding.UTF8.GetByteCount(text);
            var result = SpaceAnalysisTools.AnalyzeSpaces(text);
            result.ShouldNotBeNull();
            result.ResultingStream.ShouldNotBeNull();
            result.ResultingStream.Length.ShouldBe(length);
            result.FalseSpaces.ShouldNotBeEmpty();
            result.FalseSpaces.Length.ShouldBe(1);
            foreach (var space in result.FalseSpaces)
            {
                space.Item1.ShouldBe('\u00a0');
                space.Item2.ShouldBe("NON-BREAKING SPACE");
            }
        }

        [Test]
        public void TestAnalyzeSpacesWithBadSpacesExplicit()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                string spaceName = badSpace.Key;
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~~ This is a bad space
                string text = $"Hello{whiteSpace}world!";
                int length = Encoding.UTF8.GetByteCount(text);
                var result = SpaceAnalysisTools.AnalyzeSpaces(text);
                result.ShouldNotBeNull();
                result.ResultingStream.ShouldNotBeNull();
                result.ResultingStream.Length.ShouldBe(length);
                result.FalseSpaces.ShouldNotBeEmpty();
                result.FalseSpaces[0].Item1.ShouldBe(whiteSpace);
                result.FalseSpaces[0].Item2.ShouldBe(spaceName);
            }
        }

        [Test]
        public void TestAnalyzeSpacesWithMultipleBadSpacesExplicit()
        {
            foreach (var badSpace in Spaces.badSpaces)
            {
                string spaceName = badSpace.Key;
                char whiteSpace = Encoding.UTF8.GetString(badSpace.Value)[0];

                //                   vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~~~~vvvvvvvvvvvv~~vvvvvvvvvvvv~~~vvvvvvvvvvvv~~~~ These are bad spaces
                string text = $"Hello{whiteSpace}and{whiteSpace}welcome{whiteSpace}to{whiteSpace}the{whiteSpace}world!";
                int length = Encoding.UTF8.GetByteCount(text);
                var result = SpaceAnalysisTools.AnalyzeSpaces(text);
                result.ShouldNotBeNull();
                result.ResultingStream.ShouldNotBeNull();
                result.ResultingStream.Length.ShouldBe(length);
                result.FalseSpaces.ShouldNotBeEmpty();
                result.FalseSpaces.Length.ShouldBe(1);
                foreach (var space in result.FalseSpaces)
                {
                    space.Item1.ShouldBe(whiteSpace);
                    space.Item2.ShouldBe(spaceName);
                }
            }
        }

        [Test]
        public void TestAnalyzeSpacesMultipleDifferentSpacesExplicit()
        {
            //                  vvvvvv~~~vvvvvv~~~~~~~vvvvvv~~vvvvvv~~~vvvvvv~~~~ These are the bad spaces
            string text = "Hello\u00a0and\u200Bwelcome\u2008to\u200Bthe\u00a0world!";
            int length = Encoding.UTF8.GetByteCount(text);
            var result = SpaceAnalysisTools.AnalyzeSpaces(text);
            result.ShouldNotBeNull();
            result.ResultingStream.ShouldNotBeNull();
            result.ResultingStream.Length.ShouldBe(length);
            result.FalseSpaces.ShouldNotBeEmpty();
            result.FalseSpaces.Length.ShouldBe(3);
            result.FalseSpaces[0].Item1 = '\u00a0';
            result.FalseSpaces[1].Item1 = '\u200b';
            result.FalseSpaces[2].Item1 = '\u2008';
            result.FalseSpaces[0].Item2 = "NON-BREAKING SPACE";
            result.FalseSpaces[1].Item2 = "ZERO WIDTH SPACE";
            result.FalseSpaces[2].Item2 = "PUNCTUATION SPACE";
        }
    }
}
