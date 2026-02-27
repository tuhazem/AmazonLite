using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public Category(string name)
        {
            Name = name;
        }

        public void UpdateName(string newName) {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty.");
            Name = newName;
        }
    }
}
