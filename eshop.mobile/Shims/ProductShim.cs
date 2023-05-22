using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Mobile.Models;

namespace Eshop.Mobile.Shims
{
    public static class ProductShim
    {
        public static IEnumerable<Product> Products => new List<Product>()
        {
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
            new Product(-1, String.Empty, String.Empty, false, false, String.Empty, 0, 0, new List<AttributeList>(), new List<Review>(), new List<ImageSource>()),
        };
    }
}
