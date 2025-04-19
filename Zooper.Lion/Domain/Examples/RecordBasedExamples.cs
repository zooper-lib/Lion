using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Add IsExternalInit for .NET Standard 2.0 which doesn't include it
namespace System.Runtime.CompilerServices
{
	internal static class IsExternalInit { }
}

namespace Zooper.Lion.Domain.Examples
{
	// Example of a record-based Entity
	// Note: For entities, we only care about ID equality, not structural equality
	public record RecordBasedEntity : IEntity<Guid>
	{
		public Guid Id { get; init; }
		public string Name { get; init; }

		// Default constructor for ORM/deserialization
		protected RecordBasedEntity() { }

		public RecordBasedEntity(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		// For records, we can create a method to generate a new instance with a modified ID
		protected RecordBasedEntity WithId(Guid newId) => this with { Id = newId };

		// Note: With records, we may want to actually use the default record equality
		// which compares all properties for entities that are truly value objects with identity
		// If we want to compare only by ID, we'd need to carefully implement equality
	}

	// Example of a record-based Aggregate Root
	public record RecordBasedAggregateRoot : IEntity<Guid>, IAggregateRoot<Guid>
	{
		public Guid Id { get; init; }
		public string Title { get; init; }
		public string Description { get; init; }

		// Default constructor for ORM/deserialization
		protected RecordBasedAggregateRoot() { }

		public RecordBasedAggregateRoot(Guid id, string title, string description)
		{
			Id = id;
			Title = title;
			Description = description;
		}

		// Method to create a copy with modified ID
		protected RecordBasedAggregateRoot WithId(Guid newId) => this with { Id = newId };
	}

	// Example of a record-based Value Object
	public record RecordBasedValueObject : IValueObject, IValueObjectWithComponents
	{
		public string Code { get; init; }
		public int Value { get; init; }

		// Default constructor for deserialization
		protected RecordBasedValueObject() { }

		public RecordBasedValueObject(string code, int value)
		{
			Code = code;
			Value = value;
		}

		// Implementation of IValueObjectWithComponents for equality comparison
		public IEnumerable<object?> GetEqualityComponents()
		{
			yield return Code;
			yield return Value;
		}

		// Implementation of IValueObject
		public void Validate()
		{
			// Example validation logic
			if (string.IsNullOrEmpty(Code))
				throw new InvalidOperationException("Code cannot be empty");
		}

		// Note: Records already implement structural equality by default,
		// which is what we want for value objects. No need to override.
	}

	// Example helper to demonstrate manual ID equality for records if needed
	public static class RecordEntityHelper
	{
		public static bool EntityEquals<TId>(IEntity<TId> entity, object? obj) where TId : notnull
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			if (obj is null) return false;
			if (ReferenceEquals(entity, obj)) return true;
			if (obj.GetType() != entity.GetType()) return false;

			if (obj is IEntity<TId> other)
				return entity.Id.Equals(other.Id);

			return false;
		}
	}
}