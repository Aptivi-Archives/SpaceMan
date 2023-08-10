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

using System.Collections.Generic;
using System.Text;

namespace SpaceMan.References
{
    internal static class Spaces
    {
        internal static readonly Dictionary<string, byte[]> badSpaces = new()
        {
            { "CHARACTER TABULATION",           Encoding.UTF8.GetBytes("\u0009") },
            { "NON-BREAKING SPACE",             Encoding.UTF8.GetBytes("\u00a0") },
            { "OGHAM SPACE MARK",               Encoding.UTF8.GetBytes("\u1680") },
            { "EN QUAD",                        Encoding.UTF8.GetBytes("\u2000") },
            { "EM QUAD",                        Encoding.UTF8.GetBytes("\u2001") },
            { "EN SPACE",                       Encoding.UTF8.GetBytes("\u2002") },
            { "EM SPACE",                       Encoding.UTF8.GetBytes("\u2003") },
            { "THREE-PER-EM SPACE",             Encoding.UTF8.GetBytes("\u2004") },
            { "FOUR-PER-EM SPACE",              Encoding.UTF8.GetBytes("\u2005") },
            { "SIX-PER-EM SPACE",               Encoding.UTF8.GetBytes("\u2006") },
            { "FIGURE SPACE",                   Encoding.UTF8.GetBytes("\u2007") },
            { "PUNCTUATION SPACE",              Encoding.UTF8.GetBytes("\u2008") },
            { "THIN SPACE",                     Encoding.UTF8.GetBytes("\u2009") },
            { "HAIR SPACE",                     Encoding.UTF8.GetBytes("\u200A") },
            { "NARROW NON-BREAKING SPACE",      Encoding.UTF8.GetBytes("\u202F") },
            { "MEDIUM MATHEMATICAL SPACE",      Encoding.UTF8.GetBytes("\u205F") },
            { "IDEOGRAPHIC SPACE",              Encoding.UTF8.GetBytes("\u3000") },
            { "MONGOLIAN VOWEL SEPARATOR",      Encoding.UTF8.GetBytes("\u180E") },
            { "ZERO WIDTH SPACE",               Encoding.UTF8.GetBytes("\u200B") },
            { "ZERO WIDTH NON-JOINER",          Encoding.UTF8.GetBytes("\u200C") },
            { "ZERO WIDTH JOINER",              Encoding.UTF8.GetBytes("\u200D") },
            { "WORD JOINER",                    Encoding.UTF8.GetBytes("\u2060") },
            { "ZERO WIDTH NON-BREAKING SPACE",  Encoding.UTF8.GetBytes("\uFEFF") },
        };
    }
}
