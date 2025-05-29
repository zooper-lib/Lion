using System;
using System.Collections.Generic;
using System.Linq;
using Zooper.Lion.Domain.Entities;
using Zooper.Lion.Domain.ValueObjects;
using Zooper.Lion.Extensions.Records;

namespace Zooper.Lion.Examples
{
	// A domain for online shopping

	// Using a record for a product (immutable reference data)
	public record Product : IEntity<Guid>, IValueObjectWithComponents
	{
		public Guid Id { get; init; }
		public string Name { get; init; }
		public decimal Price { get; init; }

		public Product(Guid id, string name, decimal price)
		{
			Id = id;
			Name = name;
			Price = price;
		}

		// Implement GetEqualityComponents for value-based equality
		public IEnumerable<object?> GetEqualityComponents()
		{
			yield return Name;
			yield return Price;
		}

		// For entity records, we rely on record's built-in equality
		// Records compare all properties by default

		public void Validate()
		{
			if (string.IsNullOrEmpty(Name))
				throw new ArgumentException("Product name cannot be empty");
			if (Price <= 0)
				throw new ArgumentException("Product price must be greater than zero");
		}
	}

	// Using a class for a shopping cart (mutable entity with behavior)
	public class ShoppingCart : IEntity<Guid>, IAggregateRoot<Guid>
	{
		public Guid Id { get; protected set; }
		private readonly List<CartItem> _items = new();

		public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
		public decimal TotalAmount => _items.Sum(i => i.Quantity * i.Product.Price);

		public ShoppingCart(Guid id)
		{
			Id = id;
		}

		// Domain behavior for adding items
		public void AddItem(Product product, int quantity)
		{
			if (product == null) throw new ArgumentNullException(nameof(product));
			if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));

			var existingItem = _items.FirstOrDefault(i => i.Product.Id == product.Id);

			if (existingItem != null)
			{
				existingItem.IncreaseQuantity(quantity);
			}
			else
			{
				_items.Add(new CartItem(product, quantity));
			}
		}

		// Domain behavior for removing items
		public void RemoveItem(Guid productId)
		{
			var item = _items.FirstOrDefault(i => i.Product.Id == productId);
			if (item != null)
			{
				_items.Remove(item);
			}
		}

		// Use the extension methods for equality comparison
		public override bool Equals(object? obj) => this.EntityEquals(obj);
		public override int GetHashCode() => this.EntityGetHashCode();
	}

	// Using a record for cart items (value objects within the aggregate)
	public record CartItem : IValueObject, IValueObjectWithComponents
	{
		public Product Product { get; }
		public int Quantity { get; private set; }

		public CartItem(Product product, int quantity)
		{
			Product = product ?? throw new ArgumentNullException(nameof(product));
			Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive", nameof(quantity));
		}

		// Internal method to modify quantity
		internal void IncreaseQuantity(int additionalQuantity)
		{
			if (additionalQuantity <= 0)
				throw new ArgumentException("Additional quantity must be positive", nameof(additionalQuantity));

			Quantity += additionalQuantity;
		}

		// Implement GetEqualityComponents for value-based equality
		public IEnumerable<object?> GetEqualityComponents()
		{
			yield return Product;
			yield return Quantity;
		}

		// Value objects using records already have proper equality comparison

		public void Validate()
		{
			if (Product == null)
				throw new InvalidOperationException("Product cannot be null");
			if (Quantity <= 0)
				throw new InvalidOperationException("Quantity must be positive");
		}
	}

	// Example usage code showing how these can work together
	public class ShoppingService
	{
		public void DemonstrateUsage()
		{
			// Create immutable products (records)
			var product1 = new Product(Guid.NewGuid(), "Laptop", 1200);
			var product2 = new Product(Guid.NewGuid(), "Mouse", 25);

			// Create a mutable shopping cart (class)
			var cart = new ShoppingCart(Guid.NewGuid());

			// Add products to cart
			cart.AddItem(product1, 1);
			cart.AddItem(product2, 2);

			// Get the total amount
			decimal total = cart.TotalAmount; // 1250

			// Remove an item
			cart.RemoveItem(product2.Id);

			// Updated total
			total = cart.TotalAmount; // 1200
		}
	}
}