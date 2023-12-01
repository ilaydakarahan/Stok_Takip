using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RequestDto;

public  record ProductAddRequest(string Name, int Stock, decimal Price, int CategoryId)
{
    public static Product ConvertToEntity(ProductAddRequest request)
    {
        return new Product
        {
            Name=request.Name,
            Stock=request.Stock,
            Price=request.Price,
            CategoryId=request.CategoryId
        };
    }
}
