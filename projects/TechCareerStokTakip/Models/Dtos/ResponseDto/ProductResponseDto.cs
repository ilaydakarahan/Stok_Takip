using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.ResponseDto;
//response veriyi kullanıcıya göstermek için kullanılır
public record ProductResponseDto(Guid Id, string Name, int Stock, decimal Price, int CategoryId)
{
    public static ProductResponseDto ConvertToResponse(Product product)
    {
        return new ProductResponseDto(
            Id:product.Id,
            Name:product.Name,
            Stock:product.Stock,
            Price:product.Price,
            CategoryId:product.CategoryId
            );
       
    }
}
