using System;
using System.Collections.Generic;
using System.Linq;

namespace Zooper.Lion.Domain.ValueObjects
{
	/// <summary>
	/// Extension methods for value object implementations (class or record)
	/// </summary>
	public static class ValueObjectExtensions
	{
		/// <summary>
		/// Determines whether two value objects are equal based on their equality components
		/// </summary>
		/// <param name="self">The value object</param>
		/// <param name="other">The object to compare with</param>
		/// <param name="getEqualityComponents">A function to get the equality components</param>
		public static bool ValueObjectEquals(
			this IValueObject self,
			object? other,
			Func<IEnumerable<object?>> getEqualityComponents)
		{
			if (other is null) return false;
			if (ReferenceEquals(self, other)) return true;
			if (other.GetType() != self.GetType()) return false;

			var selfComponents = getEqualityComponents();

			// If we can cast to IValueObjectWithComponents, use its GetEqualityComponents method
			if (other is IValueObjectWithComponents componentProvider)
			{
				var otherComponents = componentProvider.GetEqualityComponents();
				return selfComponents.SequenceEqual(otherComponents);
			}

			// Otherwise, use the same function that generated the self components
			// This works for record types and simple value objects
			var otherComponentsFromFunc = getEqualityComponents();
			return selfComponents.SequenceEqual(otherComponentsFromFunc);
		}

		/// <summary>
		/// Gets a hash code for a value object based on its equality components
		/// </summary>
		/// <param name="self">The value object</param>
		/// <param name="getEqualityComponents">A function to get the equality components</param>
		public static int ValueObjectGetHashCode(
			this IValueObject self,
			Func<IEnumerable<object?>> getEqualityComponents)
		{
			return getEqualityComponents()
				.Select(x => x?.GetHashCode() ?? 0)
				.Aggregate((x, y) => x ^ y);
		}
	}
}