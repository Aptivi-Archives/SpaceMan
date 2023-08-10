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
using System.IO;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SpaceMan.Analysis
{
    /// <summary>
    /// Space analysis tools
    /// </summary>
    public static class SpaceAnalysisTools
    {
        /// <summary>
        /// Analyzes spaces from the given text
        /// </summary>
        /// <param name="text">Text to analyze</param>
        /// <returns>Analysis results</returns>
        public static SpaceAnalysisResult AnalyzeSpaces(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            // Make a stream to analyze it
            var textStream = new MemoryStream();
            var textBytes = Encoding.UTF8.GetBytes(text);
            textStream.Write(textBytes, 0, textBytes.Length);
            return AnalyzeSpacesFrom(textStream);
        }

        /// <summary>
        /// Analyzes spaces from the given path to file
        /// </summary>
        /// <param name="pathToFile">File to analyze</param>
        /// <returns>Analysis results</returns>
        public static SpaceAnalysisResult AnalyzeSpacesFrom(string pathToFile)
        {
            if (string.IsNullOrEmpty(pathToFile))
                throw new ArgumentNullException(nameof(pathToFile));
            if (!File.Exists(pathToFile))
                throw new FileNotFoundException("File not found to analyze space from.", pathToFile);

            // Open the file, analyze it, then close it.
            var fileReader = File.OpenRead(pathToFile);
            var result = AnalyzeSpacesFrom(fileReader);
            fileReader.Close();
            return result;
        }

        /// <summary>
        /// Analyzes spaces from the given stream
        /// </summary>
        /// <param name="stream">Stream to analyze</param>
        /// <returns>Analysis results</returns>
        public static SpaceAnalysisResult AnalyzeSpacesFrom(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new InvalidOperationException("Stream can't read.");
            if (!stream.CanSeek)
                throw new InvalidOperationException("Stream can't seek.");
            return new SpaceAnalysisResult(stream);
        }

        internal static bool IsTrueSpaceOrChar(char c) =>
            !Spaces.badSpaces.Where((kvp) => !Encoding.UTF8.GetBytes($"{c}").Except(kvp.Value).Any()).Any();
    }
}
