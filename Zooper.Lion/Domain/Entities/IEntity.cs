using System;

namespace Zooper.Lion.Domain.Entities
{
	/// <summary>
	/// Defines an entity which is a domain object with a unique identity.
	/// This interface allows both class and record implementations.
	/// </summary>
	/// <typeparam name="TId">The type of the identifier</typeparam>
	public interface IEntity<TId> where TId : notnull
	{
		/// <summary>
		/// Gets the unique identifier of the entity
		/// </summary>
		TId Id { get; }
	}
}