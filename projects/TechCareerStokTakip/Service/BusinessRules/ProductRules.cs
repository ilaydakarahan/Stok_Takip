

using Core.CrossCuttingConcerns.Exceptions;
using DataAccess.Repositories.Abstracts;

namespace Service.BusinessRules;

public class ProductRules
{
    private readonly IProductRepository _productrepository;

    public ProductRules(IProductRepository productrepository)
    {
        _productrepository = productrepository;
    }

    public void ProductNameMustBeUnique(string productName)
    {
        var product = _productrepository.GetByFilter(x=>x.Name==productName);
        if (product is not null)
        {
            throw new BusinessException("Ürün ismi benzersiz olmalı");
        }
    }
    //yanlış girilen id ye ait ürün olmadığını bildiren hata mesajjı.
    public void ProductIsPresent(Guid id)
    {
        var product = _productrepository.GetById(id);
        if (product is null)
        {
            throw new BusinessException($"Id si : {id} olan ürün bulunamadı.");
        }
    }

}
