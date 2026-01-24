using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using InstitutionPanel.Models.Common;

namespace InstitutionPanel.Models.ValueObject
{
    public sealed class PhoneNumber : Common.ValueObject, IEquatable<PhoneNumber>
    {
        public string Value { get; private set; }

        private PhoneNumber() { }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static PhoneNumber From(string input)
        {
            if (!IsValid(input))
                throw new NotSupportedException("Invalid Nepal phone number. Must start with 98 or 97 and be exactly 10 digits.");

            return new PhoneNumber(input.Trim());
        }

        private static bool IsValid(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            Regex regex = new(@"^(98|97)\d{8}$", RegexOptions.Compiled);
            return regex.IsMatch(input.Trim());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        #region Equality operators
        public bool Equals(PhoneNumber? other) => other is not null && Value == other.Value;
        public override bool Equals(object? obj) => Equals(obj as PhoneNumber);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(PhoneNumber? left, PhoneNumber? right) => EqualOperator(left, right);
        public static bool operator !=(PhoneNumber? left, PhoneNumber? right) => NotEqualOperator(left, right);
        #endregion

        public static implicit operator string(PhoneNumber number) => number?.Value;
        public static explicit operator PhoneNumber(string number) => From(number);

        public override string ToString() => Value;
    }
}
