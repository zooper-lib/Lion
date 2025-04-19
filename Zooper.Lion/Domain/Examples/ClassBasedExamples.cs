using System;
using System.Collections.Generic;

namespace Zooper.Lion.Domain.Examples
{
	// Example of a class-based Entity
	public class ClassBasedEntity : IEntity<Guid>
	{
		public Guid Id { get; protected set; }
		public string Name { get; private set; }

		// Default constructor for ORM/serialization
		protected ClassBasedEntity() { }

		public ClassBasedEntity(Guid id, string name)
		{
			Id = id;
			Name = name;
		}

		// Example method demonstrating how to change the ID within the entity
		protected void ChangeId(Guid newId)
		{
			// We can change it directly since it has a protected setter
			Id = newId;
		}

		// Use the extension methods for equality comparison
		public override bool Equals(object? obj) => this.EntityEquals(obj);
		public override int GetHashCode() => this.EntityGetHashCode();
	}

	// Example of a class-based Aggregate Root
	public class ClassBasedAggregateRoot : IEntity<Guid>, IAggregateRoot<Guid>
	{
		public Guid Id { get; protected set; }
		public string Title { get; private set; }
		public string Description { get; private set; }

		// Default constructor for ORM/serialization
		protected ClassBasedAggregateRoot() { }

		public ClassBasedAggregateRoot(Guid id, string title, string description)
		{
			Id = id;
			Title = title;
			Description = description;
		}

		// Example method demonstrating how to change the ID within the aggregate
		protected void ChangeId(Guid newId)
		{
			// We can change it directly since it has a protected setter
			Id = newId;
		}

		// Use the extension methods for equality comparison
		public override bool Equals(object? obj) => this.EntityEquals(obj);
		public override int GetHashCode() => this.EntityGetHashCode();
	}

	// Example of a class-based Value Object
	public class ClassBasedValueObject : IValueObject, IValueObjectWithComponents
	{
		// Value objects should be immutable, so we use getter-only properties
		public string Code { get; }
		public int Value { get; }

		public ClassBasedValueObject(string code, int value)
		{
			Code = code;
			Value = value;
		}

		// Implement GetEqualityComponents for equality comparison
		public IEnumerable<object?> GetEqualityComponents()
		{
			yield return Code;
			yield return Value;
		}

		// Implement Validate from IValueObject
		public void Validate()
		{
			if (string.IsNullOrEmpty(Code))
				throw new InvalidOperationException("Code cannot be empty");
		}

		// Use the extension methods for equality comparison
		public override bool Equals(object? obj) =>
			this.ValueObjectEquals(obj, GetEqualityComponents);

		public override int GetHashCode() =>
			this.ValueObjectGetHashCode(GetEqualityComponents);
	}
}