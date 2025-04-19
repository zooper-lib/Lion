namespace Zooper.Lion.Domain
{
	/// <summary>
	/// Defines a value object which is an immutable object that contains attributes but has no conceptual identity.
	/// They are often used to represent descriptors, like quantities, dates, or money.
	/// Two Value Objects with the same properties can be considered equal.
	/// </summary>
	public interface IValueObject
	{
		/// <summary>
		/// Validates the state of the value object.
		/// </summary>
		void Validate();
	}
}