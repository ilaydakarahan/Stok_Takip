using Core.CrossCuttingConcerns.Exceptions;
using Core.Shared;
using DataAccess.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Models.Dtos.RequestDto;
using Models.Dtos.ResponseDto;
using Models.Entities;
using Moq;
using Service.Abstract;
using Service.BusinessRules;
using Service.BusinessRules.Abstract;
using Service.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.UnitTest.Products;

public class ProductServiceTests
{
    private ProductService _service;
    private Mock<IProductRepository> _mockRepository;
    private Mock<IProductRules> _mockRules;

    private ProductAddRequest productAddRequest;
    private ProductUpdateRequest productUpdateRequest;
    private Product product;
    private ProductResponseDto productResponseDto;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockRules = new Mock<IProductRules>();
        _service = new ProductService(_mockRepository.Object, _mockRules.Object);
        productAddRequest = new ProductAddRequest(Name: "Test",Stock:25,Price:2500,CategoryId:1);
        productUpdateRequest = new ProductUpdateRequest(Id:new Guid(), Name: "Test", Stock: 25, Price: 2500, CategoryId: 1);
        product = new Product { 
            Id= new Guid(),
            Name= "Test",
            CategoryId=1,
            Price=2500,
            Stock=25,
            Category = new Category() { Id=1, Name="Teknoloji",Products=new List<Product>() { new Product() } }        
        };

        productResponseDto = new ProductResponseDto(Id: new Guid(), Name: "Test", Stock: 25, Price: 2500, CategoryId: 1);
    }

    [Test]   //Unig olması durumunda ok dönmesini sağladık
    public void Add_WhenProductNameIsUnique_ReturnsOk()
    {
        //Arrange
        _mockRules.Setup(x=>x.ProductNameMustBeUnique(productAddRequest.Name));
        _mockRepository.Setup(x => x.Add(product));

        //Act-servisi çalıştırdığımız yer
        var result = _service.Add(productAddRequest);

        //Assert-Metodu çalıştırdıktan sonra karşılaşılacak sorunları test eden yer
        Assert.AreEqual(result.StatusCode, HttpStatusCode.Created);
        Assert.AreEqual(result.Data,productResponseDto);
        Assert.AreEqual(result.Message, "Ürün Eklendi");

    }

    [Test]  //Uniq değilse badrequest dönmesini sağlıyor.
    public void Add_WhenProductNameIsNotUnique_ReturnsBadRequest()
    {
        //Arrange
        _mockRules.Setup(x => x.ProductNameMustBeUnique(productAddRequest.Name))
            .Throws(new BusinessException("Ürün ismi benzersiz olmalı"));

        //Act
        var result = _service.Add(productAddRequest);

        //Assert
        Assert.AreEqual(result.Message, "Ürün ismi benzersiz olmalı");
        Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.BadRequest);
    }

    [Test]
    public void Delete_WhenProductIsPresent_ReturnsOk()
    {   //Arrange
        Guid id = new Guid();

        _mockRules.Setup(x => x.ProductIsPresent(id));
        _mockRepository.Setup(x => x.GetById(id,null)).Returns(product);
        _mockRepository.Setup(x => x.Delete(product));

        //Act
        var result = _service.Delete(id);

        //Assert
        Assert.AreEqual(result.StatusCode,HttpStatusCode.OK);
        Assert.AreEqual(result.Data,productResponseDto);
        Assert.AreEqual(result.Message, "Ürün Silindi");
    }

    [Test]
    public void Delete_WhenProductIsNotPresent_ReturnsBadRequest()
    {
        //Arrange
        Guid id =new Guid();
        _mockRules.Setup(x=>x.ProductIsPresent(id)).Throws(new BusinessException($"Id si : {id} olan ürün bulunamadı."));

        //Act
        var result = _service.Delete(id);

        //Assert
        Assert.AreEqual(result.Message, $"Id si : {id} olan ürün bulunamadı.");
        Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        Assert.AreNotEqual(result.Data,productResponseDto);
    }

    [Test]
    public void GetAll_ReturnsOk()
    {
        //Arrange
        var products = new List<Product>()
        {
            product
        };

        var responses = new List<ProductResponseDto>()
        {
            productResponseDto
        };

        _mockRepository.Setup(x => x.GetAll(null, null)).Returns(products);

        //Act
        var result = _service.GetAll();

        Assert.AreEqual(result.StatusCode,HttpStatusCode.OK);
        Assert.AreEqual(result.Data,responses);
    }

    [Test]
    public void GetByDetailId_WhenDetailIsPresent_ReturnsOk()
    {
        //Arrange
        ProductDetailDto dto = new ProductDetailDto()
        {        
            CategoryName = "Test",
            Id = Guid.NewGuid(),
            Name= "Test",
            Price=1000,
            Stock=25
        };

        _mockRules.Setup(x => x.ProductIsPresent(It.IsAny<Guid>()));
        _mockRepository.Setup(x => x.GetProductDetail(It.IsAny<Guid>())).Returns(dto);

        //Act
        var result = _service.GetByDetailId(It.IsAny<Guid>());

        //Assert
       
        Assert.AreEqual(result.Data, dto);
        Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
    }

    [Test]
    public void GetByDetailId_WhenDetailIsNotPresent_ReturnsBadRequest()
    {
        //Arrange
        _mockRules.Setup(x => x.ProductIsPresent(It.IsAny<Guid>())).Throws(
            new BusinessException($"Id si : {It.IsAny<Guid>()} olan ürün bulunamadı."));

        //Act
        var result = _service.GetByDetailId(It.IsAny<Guid>());

        //Assert
        Assert.AreEqual(result.Message, $"Id si : {It.IsAny<Guid>()} olan ürün bulunamadı.");
        Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
    }


}