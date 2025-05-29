using System;
using Zooper.Lion.Domain.Entities;

namespace Zooper.Lion.Extensions.Records
{
	/// <summary>
	/// Extension methods for entity implementations (class or record)
	/// </summary>
	public static class EntityExtensions
	{
		/// <summary>
		/// Determines whether two entities are equal based on their IDs
		/// </summary>
		public static bool EntityEquals<TId>(this IEntity<TId> self, object? obj) where TId : notnull
		{
			if (obj is null) return false;
			if (ReferenceEquals(self, obj)) return true;
			if (obj.GetType() != self.GetType()) return false;

			if (obj is IEntity<TId> other)
				return self.Id.Equals(other.Id);

			return false;
		}

		/// <summary>
		/// Gets a hash code for an entity based on its ID
		/// </summary>
		public static int EntityGetHashCode<TId>(this IEntity<TId> self) where TId : notnull
		{
			return self.Id.GetHashCode();
		}
	}
}