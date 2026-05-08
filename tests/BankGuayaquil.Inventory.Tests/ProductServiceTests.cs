using BankGuayaquil.Inventory.Application.Services;
using BankGuayaquil.Inventory.Domain.Entities;
using BankGuayaquil.Inventory.Domain.Interfaces;
using BankGuayaquil.Inventory.Tests.Mothers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BankGuayaquil.Inventory.Tests;

public class ProductServiceTests
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new ProductService(_repository, _unitOfWork);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product> { ProductMother.Standard() };
        _repository.GetAllAsync().Returns(products);

        // Act
        var result = await _sut.GetProductsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value.First().SKU.Should().Be("MON-50-4K");
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        _repository.GetByIdAsync(Arg.Any<Guid>()).Returns((Product)null!);

        // Act
        var result = await _sut.GetProductByIdAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Product.NotFound");
    }
}
