namespace Zooper.Lion.Domain
{
	/// <summary>
	/// Defines an aggregate root which is the primary entity through which we interact with the aggregate.
	/// It's responsible for enforcing the invariants (rules) of the aggregate and encapsulates
	/// access to its members.
	/// </summary>
	/// <typeparam name="TId">The type of the identifier</typeparam>
	public interface IAggregateRoot<TId> : IEntity<TId> where TId : notnull
	{
		// Marker interface
	}
}