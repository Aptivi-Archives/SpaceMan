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

using SpaceMan.References;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceMan.Analysis
{
    /// <summary>
    /// A class for the space analysis result
    /// </summary>
    public class SpaceAnalysisResult
    {
        /// <summary>
        /// Gets the stream from the analysis
        /// </summary>
        public Stream ResultingStream { get; }
        /// <summary>
        /// Gets the array of false spaces with their indexes in the text
        /// </summary>
        public (char, string)[] FalseSpaces { get; }

        internal SpaceAnalysisResult(Stream stream)
        {
            int readByte = 0;
            long bytesRead = 0;
            List<(char, string)> falseSpaces = new();

            // Seek through the entire stream
            stream.Seek(0, SeekOrigin.Begin);
            while (readByte != -1)
            {
                // Read a byte
                readByte = stream.ReadByte();
                if (readByte == -1)
                    break;

                // Process it and verify the whitespace
                char c = (char)readByte;
                byte b = (byte)readByte;
                var badSpaceList = Spaces.badSpaces.Where((kvp) => kvp.Value[0] == b);
                if (badSpaceList.Any())
                {
                    // Seek until we find exactly a character that we want
                    int charLen = 0;
                    int charIdx = 1;
                    while (badSpaceList.Count() > 1)
                    {
                        charIdx++;
                        readByte = stream.ReadByte();
                        badSpaceList = badSpaceList.Where((kvp) => kvp.Value.Length >= charIdx && kvp.Value[charIdx - 1] == (byte)readByte);
                    }
                    if (badSpaceList.Any())
                    {
                        var kvp = badSpaceList.Single();
                        c = Encoding.UTF8.GetString(kvp.Value)[0];
                        charLen = Encoding.UTF8.GetByteCount($"{c}") - (charIdx - 1);
                        for (int i = 1; i < charLen; i++)
                            readByte = stream.ReadByte();
                    }
                }
                if (!SpaceAnalysisTools.IsTrueSpaceOrChar(c))
                {
                    var falseSpace = (c, Spaces.badSpaces.Where((kvp) => Encoding.UTF8.GetBytes($"{c}").SequenceEqual(kvp.Value)).ToArray()[0].Key);
                    if (!falseSpaces.Contains(falseSpace))
                        falseSpaces.Add(falseSpace);
                }
                bytesRead++;
            }

            // Install the values
            ResultingStream = stream;
            FalseSpaces = falseSpaces.ToArray();
        }
    }
}
