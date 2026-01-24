using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using InstitutionPanel.Models.Common;

namespace InstitutionPanel.Models.ValueObject
{
    public sealed class ContactNumber : Common.ValueObject, IEquatable<ContactNumber>
    {
        public string Value { get; private set; }

        private ContactNumber() { }

        private ContactNumber(string value)
        {
            Value = value;
        }

        public static ContactNumber From(string input)
        {
            if (!IsValid(input))
                throw new NotSupportedException("Invalid contact number. Must start with 0 and be exactly 9 digits.");

            return new ContactNumber(input.Trim());
        }

        private static bool IsValid(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            Regex regex = new(@"^0\d{8}$", RegexOptions.Compiled);
            return regex.IsMatch(input.Trim());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        #region Equality operators
        public bool Equals(ContactNumber? other) => other is not null && Value == other.Value;
        public override bool Equals(object? obj) => Equals(obj as ContactNumber);
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(ContactNumber? left, ContactNumber? right) => EqualOperator(left, right);
        public static bool operator !=(ContactNumber? left, ContactNumber? right) => NotEqualOperator(left, right);
        #endregion

        public static implicit operator string(ContactNumber number) => number?.Value;
        public static explicit operator ContactNumber(string number) => From(number);

        public override string ToString() => Value;
    }
}
