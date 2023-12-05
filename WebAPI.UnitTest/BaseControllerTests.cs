using Core.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;


namespace WebAPI.UnitTest;

public class BaseControllerTests
{
    private BaseController _controller;

    [SetUp]
    public void Setup()
    {
        _controller = new BaseController();
    }

    [Test]
    public void ActionResultInstance_WhenStatusCodeOk_ReturnsOk()
    {
        //Arrange
        var response = new Response<string>() {Data="Test",Message="Test",StatusCode=System.Net.HttpStatusCode.OK};

        //Act
        var result = _controller.ActionResultInstance(response);

        //Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void ActionResultInstance_WhenStatusCodeNotFound_ReturnsNotFound()
    {
        //Arrange
        var response = new Response<string>() { Data = "Test", Message = "Test", StatusCode = System.Net.HttpStatusCode.NotFound};

        //Act
        var result = _controller.ActionResultInstance(response);

        //Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
    }

    [Test]
    public void ActionResultInstance_WhenStatusCodeBadRequest_ReturnsBadRequest()
    {
        //Arrange
        var response = new Response<string>() { Data = "Test", Message = "Test", StatusCode = System.Net.HttpStatusCode.BadRequest};

        //Act
        var result = _controller.ActionResultInstance(response);

        //Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public void ActionResultInstance_WhenStatusCodeCreated_ReturnsCreated()
    {
        //Arrange
        var response = new Response<string>() { Data = "Test", Message = "Test", StatusCode = System.Net.HttpStatusCode.Created};

        //Act
        var result = _controller.ActionResultInstance(response);

        //Assert
        Assert.IsInstanceOf<CreatedResult>(result);
    }
}
