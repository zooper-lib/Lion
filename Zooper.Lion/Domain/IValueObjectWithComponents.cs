using System.Collections.Generic;

namespace Zooper.Lion.Domain
{
	/// <summary>
	/// Interface for value objects that can provide their equality components.
	/// This is used to bridge between class implementations and record implementations.
	/// </summary>
	public interface IValueObjectWithComponents
	{
		/// <summary>
		/// Gets the components that determine the value object's identity
		/// </summary>
		IEnumerable<object?> GetEqualityComponents();
	}
}