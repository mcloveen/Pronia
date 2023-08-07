using System;
using Pronia.Models;

namespace Pronia.ViewModels.BasketVMs
{
	public record  BasketItemProductVM
	{
		public Product Product { get; set; }
		public int Count { get; set; }
	}
}


