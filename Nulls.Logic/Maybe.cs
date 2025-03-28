using System;
using NullGuard;

namespace Nulls.Logic
{
    public struct Maybe<T> : IEquatable<Maybe<T>>
        where T : class
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                if (HasNoValue)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        public bool HasValue => _value != null;
        public bool HasNoValue => !HasValue;

        private Maybe([AllowNull] T value) => _value = value;

        public static implicit operator Maybe<T>([AllowNull] T value) =>
            new Maybe<T>(value);

        public static bool operator ==(Maybe<T> maybe, T value) => 
            maybe.HasValue && maybe.Value.Equals(value);

        public static bool operator !=(Maybe<T> maybe, T value) =>
            !(maybe == value);

        public static bool operator ==(Maybe<T> first, Maybe<T> second) =>
            first.Equals(second);

        public static bool operator !=(Maybe<T> first, Maybe<T> second) =>
            !(first == second);

        public override bool Equals(object obj) =>
            obj is Maybe<T> && Equals((Maybe<T>)obj);

        public bool Equals(Maybe<T> other) =>
            HasNoValue && other.HasNoValue
            || HasValue && other.HasValue && _value.Equals(other._value);

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => HasNoValue ? "No value" : Value.ToString();

        [return: AllowNull]
        public T Unwrap([AllowNull] T defaultValue = default(T)) => HasValue ? Value : defaultValue;
    }
}
