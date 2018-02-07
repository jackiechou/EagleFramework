using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Eagle.Common.Utilities
{
    /// <summary>
    /// Provides static utility methods for encoding and decoding text within
    /// RFC 3986 URIs.
    /// </summary>
    /// <seealso href="http://www.ietf.org/rfc/rfc3986">RFC 3986: URI Generic Syntax</seealso>
    /// <threadsafety static="true" instance="false"/>
    /// <preliminary/>
    public static class UriUtility
    {
        private static readonly BitArray UnreservedCharacters;
        private static readonly BitArray AllowedHostCharacters;
        private static readonly BitArray AllowedPathCharacters;
        private static readonly BitArray AllowedQueryCharacters;
        private static readonly BitArray AllowedFragmentCharacters;

        /// <summary>
        /// This is the regular expression for a single percent-encoded character.
        /// </summary>
        private static readonly Regex PercentEncodedPattern = new Regex(@"%([0-9A-Fa-f][0-9A-Fa-f])", RegexOptions.Compiled);

        static UriUtility()
        {
            UnreservedCharacters = new BitArray(256);
            for (char i = 'a'; i <= 'z'; i++)
                UnreservedCharacters.Set(i, true);
            for (char i = 'A'; i <= 'Z'; i++)
                UnreservedCharacters.Set(i, true);
            for (char i = '0'; i <= '9'; i++)
                UnreservedCharacters.Set(i, true);
            UnreservedCharacters.Set('-', true);
            UnreservedCharacters.Set('.', true);
            UnreservedCharacters.Set('_', true);
            UnreservedCharacters.Set('~', true);

            var generalDelimiters = new BitArray(256);
            generalDelimiters.Set(':', true);
            generalDelimiters.Set('/', true);
            generalDelimiters.Set('?', true);
            generalDelimiters.Set('#', true);
            generalDelimiters.Set('[', true);
            generalDelimiters.Set(']', true);
            generalDelimiters.Set('@', true);

            var subDelimiters = new BitArray(256);
            subDelimiters.Set('!', true);
            subDelimiters.Set('$', true);
            subDelimiters.Set('&', true);
            subDelimiters.Set('(', true);
            subDelimiters.Set(')', true);
            subDelimiters.Set('*', true);
            subDelimiters.Set('+', true);
            subDelimiters.Set(',', true);
            subDelimiters.Set(';', true);
            subDelimiters.Set('=', true);
            subDelimiters.Set('\'', true);

            new BitArray(256).Or(generalDelimiters).Or(subDelimiters);

            AllowedHostCharacters = new BitArray(256).Or(UnreservedCharacters).Or(subDelimiters);

            AllowedPathCharacters = new BitArray(256).Or(UnreservedCharacters).Or(subDelimiters);
            AllowedPathCharacters.Set(':', true);
            AllowedPathCharacters.Set('@', true);

            AllowedQueryCharacters = new BitArray(256).Or(AllowedPathCharacters);
            AllowedQueryCharacters.Set('/', true);
            AllowedQueryCharacters.Set('?', true);

            AllowedFragmentCharacters = new BitArray(256).Or(AllowedPathCharacters);
            AllowedFragmentCharacters.Set('/', true);
            AllowedFragmentCharacters.Set('?', true);
        }

        /// <summary>
        /// Decodes the text of a URI by unescaping any percent-encoded character sequences and
        /// then evaluating the result using the default <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="UriDecode(string, Encoding)"/> using the default
        /// <see cref="Encoding.UTF8"/> encoding.
        /// </remarks>
        /// <param name="text">The encoded URI.</param>
        /// <returns>The decoded URI text.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="text"/> is <see langword="null"/>.</exception>
        public static string UriDecode(string text)
        {
            return UriDecode(text, Encoding.UTF8);
        }

        /// <summary>
        /// Decodes the text of a URI by unescaping any percent-encoded character sequences and
        /// then evaluating the result using the specified encoding.
        /// </summary>
        /// <param name="text">The encoded URI.</param>
        /// <param name="encoding">The encoding to use for Unicode characters in the URI. If this value is <see langword="null"/>, the <see cref="Encoding.UTF8"/> encoding will be used.</param>
        /// <returns>The decoded URI text.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="text"/> is <see langword="null"/>.</exception>
        public static string UriDecode(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            encoding = encoding ?? Encoding.UTF8;
            MatchEvaluator matchEvaluator =
                match =>
                {
                    string hexValue = match.Groups[1].Value;
                    return ((char)byte.Parse(hexValue, NumberStyles.HexNumber)).ToString();
                };
            string decodedText = PercentEncodedPattern.Replace(text, matchEvaluator);
            byte[] data = Array.ConvertAll(decodedText.ToCharArray(), c => (byte)c);
            return encoding.GetString(data);
        }

        /// <summary>
        /// Encodes text for inclusion in a URI using the <see cref="Encoding.UTF8"/> encoding.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="UriEncode(string, UriPart, Encoding)"/> using the default
        /// <see cref="Encoding.UTF8"/> encoding.
        /// </remarks>
        /// <param name="text">The text to encode for inclusion in a URI.</param>
        /// <param name="uriPart">A <see cref="UriPart"/> value indicating where in the URI the specified text will be included.</param>
        /// <returns>The URI-encoded text, suitable for the specified URI part.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="text"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="uriPart"/> is not a valid <see cref="UriPart"/>.</exception>
        public static string UriEncode(string text, UriPart uriPart)
        {
            return UriEncode(text, uriPart, Encoding.UTF8);
        }

        /// <summary>
        /// Encodes text for inclusion in a URI.
        /// </summary>
        /// <param name="text">The text to encode for inclusion in a URI.</param>
        /// <param name="uriPart">A <see cref="UriPart"/> value indicating where in the URI the specified text will be included.</param>
        /// <param name="encoding">The encoding to use for Unicode characters in the URI. If this value is <see langword="null"/>, the <see cref="Encoding.UTF8"/> encoding will be used.</param>
        /// <returns>The URI-encoded text, suitable for the specified URI part.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="text"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="uriPart"/> is not a valid <see cref="UriPart"/>.</exception>
        public static string UriEncode(string text, UriPart uriPart, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            encoding = encoding ?? Encoding.UTF8;
            switch (uriPart)
            {
                case UriPart.Any:
                    return UriEncodeAny(text, encoding);

                case UriPart.AnyUrl:
                    return UriEncodeAnyUrl(text, encoding);

                case UriPart.Host:
                    return UriEncodeHost(text, encoding);

                case UriPart.Path:
                    return UriEncodePath(text, encoding);

                case UriPart.PathSegment:
                    return UriEncodePathSegment(text, encoding);

                case UriPart.Query:
                    return UriEncodeQuery(text, encoding);

                case UriPart.QueryValue:
                    return UriEncodeQueryValue(text, encoding);

                case UriPart.Fragment:
                    return UriEncodeFragment(text, encoding);

                default:
                    throw new ArgumentException("The specified uriPart is not valid.", "uriPart");
            }
        }

        private static string UriEncodeAny(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (text.Length == 0)
                return text;

            StringBuilder builder = new StringBuilder();
            byte[] data = encoding.GetBytes(text);
            for (int i = 0; i < data.Length; i++)
            {
                if (UnreservedCharacters[data[i]])
                    builder.Append((char)data[i]);
                else
                    builder.Append('%').Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private static string UriEncodeAnyUrl(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (text.Length == 0)
                return text;

            StringBuilder builder = new StringBuilder();
            byte[] data = encoding.GetBytes(text);
            for (int i = 0; i < data.Length; i++)
            {
                if (UnreservedCharacters[data[i]])
                {
                    builder.Append((char)data[i]);
                }
                else
                {
                    switch ((char)data[i])
                    {
                        case '(':
                        case ')':
                        case '*':
                        case '!':
                            builder.Append((char)data[i]);
                            break;

                        default:
                            builder.Append('%').Append(data[i].ToString("x2"));
                            break;
                    }
                }
            }

            return builder.ToString();
        }

        private static string UriEncodeHost(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (text.Length == 0)
                return text;

            StringBuilder builder = new StringBuilder();
            byte[] data = encoding.GetBytes(text);
            for (int i = 0; i < data.Length; i++)
            {
                if (AllowedHostCharacters[data[i]])
                    builder.Append((char)data[i]);
                else
                    builder.Append('%').Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private static string UriEncodePath(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (text.Length == 0)
                return text;

            StringBuilder builder = new StringBuilder();
            byte[] data = encoding.GetBytes(text);
            for (int i = 0; i < data.Length; i++)
            {
                if (AllowedPathCharacters[data[i]] || data[i] == '/')
                    builder.Append((char)data[i]);
                else
                    builder.Append('%').Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private static string UriEncodePathSegment(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (text.Length == 0)
                return text;

            StringBuilder builder = new StringBuilder();
            byte[] data = encoding.GetBytes(text);
            for (int i = 0; i < data.Length; i++)
            {
                if (AllowedPathCharacters[data[i]])
                    builder.Append((char)data[i]);
                else
                    builder.Append('%').Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private static string UriEncodeQuery(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (text.Length == 0)
                return text;

            StringBuilder builder = new StringBuilder();
            byte[] data = encoding.GetBytes(text);
            for (int i = 0; i < data.Length; i++)
            {
                if (AllowedQueryCharacters[data[i]])
                    builder.Append((char)data[i]);
                else
                    builder.Append('%').Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private static string UriEncodeQueryValue(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (text.Length == 0)
                return text;

            StringBuilder builder = new StringBuilder();
            byte[] data = encoding.GetBytes(text);
            for (int i = 0; i < data.Length; i++)
            {
                if (AllowedQueryCharacters[data[i]] && data[i] != '&')
                    builder.Append((char)data[i]);
                else
                    builder.Append('%').Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private static string UriEncodeFragment(string text, Encoding encoding)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (text.Length == 0)
                return text;

            StringBuilder builder = new StringBuilder();
            byte[] data = encoding.GetBytes(text);
            for (int i = 0; i < data.Length; i++)
            {
                if (AllowedFragmentCharacters[data[i]])
                    builder.Append((char)data[i]);
                else
                    builder.Append('%').Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }


    public enum UriPart
    {
        /// <summary>
        /// Represents a non-specific URI part, where all characters except unreserved characters are percent-encoded.
        /// </summary>
        Any,

        /// <summary>
        /// Represents a non-specific URL part, where all characters except unreserved characters and the characters <c>(</c>, <c>)</c>, <c>!</c>, and <c>*</c> are percent-encoded.
        /// </summary>
        /// <remarks>
        /// When used with <see cref="UriUtility.UriEncode(string, UriPart)"/>, this URI part matches the behavior of <see cref="HttpUtility.UrlEncode(string)"/>, with the exception of space characters (this method percent-encodes space characters as <c>%20</c>).
        /// </remarks>
        AnyUrl,

        /// <summary>
        /// The host part of a URI.
        /// </summary>
        Host,

        /// <summary>
        /// The complete path of a URI, formed from path segments separated by slashes (<c>/</c> characters).
        /// </summary>
        Path,

        /// <summary>
        /// A single segment of a URI path.
        /// </summary>
        PathSegment,

        /// <summary>
        /// The complete query string of a URI.
        /// </summary>
        Query,

        /// <summary>
        /// The value assigned to a query parameter within the query string of a URI.
        /// </summary>
        QueryValue,

        /// <summary>
        /// The fragment part of a URI.
        /// </summary>
        Fragment,
    }
}
