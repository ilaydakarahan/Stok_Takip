using Core.CrossCuttingConcerns.Exceptions;
using Core.Shared;
using DataAccess.Repositories.Abstracts;
using Models.Dtos.RequestDto;
using Models.Dtos.ResponseDto;
using Models.Entities;
using Service.Abstract;
using Service.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Concrete;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductRules _rules;   //oluşturulan concreate nesnesinin IoD kaydı için servicedependencies'e eklenir
    //Servisle alakalı yazdığımız kurallar(businessrule) buraya eklenmesi,yukarıdaki gibi

    public ProductService(IProductRepository productRepository, ProductRules rules)
    {
        _productRepository = productRepository;
        _rules = rules;
    }

    public Response<ProductResponseDto> Add(ProductAddRequest request)
    {
        try
        {
            Product product = ProductAddRequest.ConvertToEntity(request);

            _rules.ProductNameMustBeUnique(product.Name); //hata mesajı için kural eklenir.

            product.Id = new Guid();
            _productRepository.Add(product);

            var data = ProductResponseDto.ConvertToResponse(product);

            return new Response<ProductResponseDto>()
            {
                Data = data,
                Message = "Ürün Eklendi.",
                StatusCode = System.Net.HttpStatusCode.Created
            };
        }
        catch (BusinessException ex)
        {
            return new Response<ProductResponseDto>()
            {
                Message = ex.Message,
                StatusCode=System.Net.HttpStatusCode.BadRequest
            };
        }       
    }

    public Response<ProductResponseDto> Delete(Guid id)
    {
        try
        {
            _rules.ProductIsPresent(id);

            var product = _productRepository.GetById(id);
            _productRepository.Delete(product);

            var data = ProductResponseDto.ConvertToResponse(product);

            return new Response<ProductResponseDto>()
            {
                Data = data,
                Message = "Ürün Silindi",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        catch (BusinessException ex)
        {
            return new Response<ProductResponseDto>()
            {
                Message= ex.Message,
                StatusCode= System.Net.HttpStatusCode.BadRequest
            };
        }
    }

    public Response<List<ProductResponseDto>> GetAll()
    {
        var products = _productRepository.GetAll();

        var response =products.Select(x=>ProductResponseDto.ConvertToResponse(x)).ToList();
        return new Response<List<ProductResponseDto>>()
        {
            Data = response,
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public Response<List<ProductResponseDto>> GetAllByPriceRange(decimal min, decimal max)
    {
        var products =_productRepository.GetAll(x=>x.Price<=max && x.Price>=min);

        var response = products.Select(x => ProductResponseDto.ConvertToResponse(x)).ToList();
        return new Response<List<ProductResponseDto>>()
        {
            Data = response,
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public Response<List<ProductDetailDto>> GetAllDetails()
    {
        var details =_productRepository.GetAllProductDetails();

        return new Response<List<ProductDetailDto>>()
        {
            Data = details,
            StatusCode = System.Net.HttpStatusCode.OK 
        };
    }

    public Response<List<ProductDetailDto>> GetAllDetailsByCategoryId(int categoryId)
    {
        var details = _productRepository.GetDetailsByCategoryId(categoryId);

        return new Response<List<ProductDetailDto>>()
        {
            Data = details,
            StatusCode = System.Net.HttpStatusCode.OK
        };
    }

    public Response<ProductDetailDto> GetByDetailId(Guid id)
    {
        try
        {
            _rules.ProductIsPresent(id);

            var detail = _productRepository.GetProductDetail(id);

            return new Response<ProductDetailDto>()
            {
                Data = detail,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        catch (BusinessException ex)
        {
            return new Response<ProductDetailDto>()
            {
                Message = ex.Message,
                StatusCode= System.Net.HttpStatusCode.BadRequest
            };
        }
    }

    public Response<ProductResponseDto> GetById(Guid id)
    {
        try
        {
            _rules.ProductIsPresent(id);

            var product = _productRepository.GetById(id);
            var response = ProductResponseDto.ConvertToResponse(product);
            return new Response<ProductResponseDto>()
            {
                Data = response,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        catch (BusinessException ex)
        {
            return new Response<ProductResponseDto>()
            {
                Message = ex.Message,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }
        
    }

    public Response<ProductResponseDto> Update(ProductUpdateRequest request)
    {
        try
        {
            Product product = ProductUpdateRequest.ConvertToEntity(request);

            _rules.ProductNameMustBeUnique(product.Name);
            _productRepository.Update(product);

            var response = ProductResponseDto.ConvertToResponse(product);

            return new Response<ProductResponseDto>()
            {
                Data = response,
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
        catch (BusinessException ex)
        {
            return new Response<ProductResponseDto>()
            {
                Message = ex.Message,
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }
    }
}
