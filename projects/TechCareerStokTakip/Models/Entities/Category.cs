using Core.Persistance.EntityBaseModel;
using Models.Dtos.RequestDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities;

public class Category : Entity<int>
{
    public string Name { get; set; }
    public List<Product> Products { get; set; }

    public static implicit operator Category(CategoryAddRequest categoryAddRequest) =>
        new Category { Name = categoryAddRequest.Name };
    public static implicit operator Category(CategoryUpdateRequest request) =>
        new Category { Name = request.Name,Id = request.Id };
}
