using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using InstitutionPanel.Models.Common;

namespace InstitutionPanel.Models.ValueObject
{
    public sealed class Email : Common.ValueObject, IEquatable<Email>
    {
        public string Value { get; private set; }

        private Email() { }

        private Email(string value)
        {
            Value = value;
        }

        public static Email From(string input)
        {
            if (!IsValid(input))
                throw new NotSupportedException("Invalid email address format.");

            return new Email(input.Trim());
        }

        private static bool IsValid(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            Regex regex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
            return regex.IsMatch(input.Trim());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        #region Equality operators
        public bool Equals(Email? other) => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        public override bool Equals(object? obj) => Equals(obj as Email);
        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        public static bool operator ==(Email? left, Email? right) => EqualOperator(left, right);
        public static bool operator !=(Email? left, Email? right) => NotEqualOperator(left, right);
        #endregion

        public static implicit operator string(Email email) => email?.Value;
        public static explicit operator Email(string email) => From(email);

        public override string ToString() => Value;
    }
}
