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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpaceMan.Analysis
{
    /// <summary>
    /// Tools that allow you to use the analysis results to convert the spaces
    /// </summary>
    public static class SpaceConversionTools
    {
        /// <summary>
        /// Converts spaces from the given analysis result to the normal spaces
        /// </summary>
        /// <param name="analysisResult">Given analysis result</param>
        /// <returns>Byte array containing converted spaces</returns>
        public static byte[] ConvertSpaces(SpaceAnalysisResult analysisResult)
        {
            if (analysisResult is null)
                throw new ArgumentNullException(nameof(analysisResult));

            // Now, do the job of converting the spaces in the stream.
            var stream = analysisResult.ResultingStream;
            var falseSpaces = analysisResult.FalseSpaces;
            List<byte> result = new();

            // Read the whole stream and modify the spaces if necessary
            int readByte = 0;
            long bytesRead = 0;
            stream.Seek(0, SeekOrigin.Begin);
            while (readByte != -1)
            {
                // Read a byte
                readByte = stream.ReadByte();
                if (readByte == -1)
                    break;

                // Process it and modify it
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
                    c = ' ';
                result.Add((byte)c);
                bytesRead++;
            }

            // Return the list of bytes
            return result.ToArray();
        }

        /// <summary>
        /// Converts spaces from the given analysis result to the normal spaces and saves the output to the file
        /// </summary>
        /// <param name="analysisResult">Given analysis result</param>
        public static string ConvertSpacesToString(SpaceAnalysisResult analysisResult)
        {
            if (analysisResult is null)
                throw new ArgumentNullException(nameof(analysisResult));

            // Convert the spaces to a string
            var bytes = ConvertSpaces(analysisResult);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Converts spaces from the given analysis result to the normal spaces and saves the output to the file
        /// </summary>
        /// <param name="analysisResult">Given analysis result</param>
        /// <param name="pathToFile">File to save the output to</param>
        public static void ConvertSpacesTo(SpaceAnalysisResult analysisResult, string pathToFile)
        {
            if (analysisResult is null)
                throw new ArgumentNullException(nameof(analysisResult));
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentNullException(nameof(pathToFile));

            // Save the converted output to a file
            var bytes = ConvertSpaces(analysisResult);
            string text = Encoding.UTF8.GetString(bytes);
            File.WriteAllText(pathToFile, text);
        }

        /// <summary>
        /// Converts spaces from the given analysis result to the normal spaces and saves the output to the stream
        /// </summary>
        /// <param name="analysisResult">Given analysis result</param>
        /// <param name="stream">Stream to save the output to</param>
        public static void ConvertSpacesTo(SpaceAnalysisResult analysisResult, Stream stream)
        {
            if (analysisResult is null)
                throw new ArgumentNullException(nameof(analysisResult));
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            // Save the converted output to a stream
            var bytes = ConvertSpaces(analysisResult);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
