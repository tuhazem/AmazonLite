﻿﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        public int StockQuantity { get; private set; }

        public int CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        public byte[] RowVersion { get; private set; } = null!;


        public Product(string name , string description , decimal price , int stockQuantity , int categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            CategoryId = categoryId;
        }

        public void UpdatePrice(decimal newPrice) {

            if (newPrice < 0) { 
                throw new ArgumentException("Price cannot be negative.");
            }

            Price = newPrice;
        }

        public void ReduceStock(int quantity) { 
        
            if(quantity > StockQuantity)
                throw new InvalidOperationException("Not enough stock available.");
            StockQuantity -= quantity;
        }

    }
}
