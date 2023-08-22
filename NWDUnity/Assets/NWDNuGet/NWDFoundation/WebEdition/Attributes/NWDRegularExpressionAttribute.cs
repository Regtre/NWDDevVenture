using System;
#if NETCOREAPP
using System.ComponentModel.DataAnnotations;
#endif
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NWDFoundation.WebEdition.Attributes
{
    /// <summary>
    ///     Regular expression validation attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class NWDRegularExpressionAttribute :
#if NETCOREAPP
        ValidationAttribute
#else
        Attribute
#endif
    {
#if !NETCOREAPP
        protected string ErrorMessage;
#endif


        /// <summary>
        ///     Constructor that accepts the regular expression pattern
        /// </summary>
        /// <param name="pattern">The regular expression to use.  It cannot be null.</param>
#if NETCOREAPP
        public NWDRegularExpressionAttribute([StringSyntax(StringSyntaxAttribute.Regex)] string pattern) : base(() => "No compatible with regular expression.")
#else
        public NWDRegularExpressionAttribute(string pattern)
#endif
        {
            Pattern = pattern;
            MatchTimeoutInMilliseconds = 2000;
        }

        /// <summary>
        ///     Gets or sets the timeout to use when matching the regular expression pattern (in milliseconds)
        ///     (-1 means never timeout).
        /// </summary>
        public int MatchTimeoutInMilliseconds { get; set; }

        /// <summary>
        /// Gets the timeout to use when matching the regular expression pattern
        /// </summary>
        public TimeSpan MatchTimeout => TimeSpan.FromMilliseconds(MatchTimeoutInMilliseconds);

        /// <summary>
        ///     Gets the regular expression pattern to use
        /// </summary>
        public string Pattern { get; }

        private Regex? Regex { get; set; }

        /// <summary>
        ///     Override of <see cref="ValidationAttribute.IsValid(object)" />
        /// </summary>
        /// <remarks>
        ///     This override performs the specific regular expression matching of the given <paramref name="value" />
        /// </remarks>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> if the given value matches the current regular expression pattern</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentException"> is thrown if the <see cref="Pattern" /> is not a valid regular expression.</exception>
#if NETCOREAPP
        public override bool IsValid(object? value)
#else
        public bool IsValid(object? value)
#endif
        {
            SetupRegex();

            // Convert the value to a string
            string? stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);

            // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
            if (string.IsNullOrEmpty(stringValue))
            {
                return true;
            }

#if NETCOREAPP
            foreach (ValueMatch m in Regex!.EnumerateMatches(stringValue))
            {
                // We are looking for an exact match, not just a search hit. This matches what
                // the RegularExpressionValidator control does
                return m.Index == 0 && m.Length == stringValue.Length;
            }
#endif

            return false;
        }

        /// <summary>
        ///     Override of <see cref="ValidationAttribute.FormatErrorMessage" />
        /// </summary>
        /// <remarks>This override provide a formatted error message describing the pattern</remarks>
        /// <param name="name">The user-visible name to include in the formatted message.</param>
        /// <returns>The localized message to present to the user</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentException"> is thrown if the <see cref="Pattern" /> is not a valid regular expression.</exception>
#if NETCOREAPP
        public override string FormatErrorMessage(string name)
#else
        public string FormatErrorMessage(string name)
#endif
        {
            SetupRegex();

#if NETCOREAPP
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Pattern);
#else
            return string.Format(CultureInfo.CurrentCulture, "", name, Pattern);
#endif
        }


        /// <summary>
        ///     Sets up the <see cref="Regex" /> property from the <see cref="Pattern" /> property.
        /// </summary>
        /// <exception cref="ArgumentException"> is thrown if the current <see cref="Pattern" /> cannot be parsed</exception>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> thrown if <see cref="MatchTimeoutInMilliseconds" /> is negative (except -1),
        /// zero or greater than approximately 24 days </exception>
        private void SetupRegex()
        {
            if (Regex == null)
            {
                if (string.IsNullOrEmpty(Pattern))
                {
                    throw new InvalidOperationException("No compatible with regular expression.");
                }

                Regex = MatchTimeoutInMilliseconds == -1
                    ? new Regex(Pattern)
                    : new Regex(Pattern, default(RegexOptions), TimeSpan.FromMilliseconds(MatchTimeoutInMilliseconds));
            }
        }
    }
}