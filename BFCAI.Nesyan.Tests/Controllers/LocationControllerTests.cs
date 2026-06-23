using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BFCAI.Nesyan.Application.Abstraction.Models.Location;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.Location;
using BFCAI.Nesyan.Controllers.Controllers.Location;
using BFCAI.Nesyan.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BFCAI.Nesyan.Tests.Controllers
{
    public class LocationControllerTests
    {
        private readonly Mock<IServiceManager> _mockServiceManager;
        private readonly Mock<ILocationService> _mockLocationService;
        private readonly LocationController _controller;

        public LocationControllerTests()
        {
            _mockServiceManager = new Mock<IServiceManager>();
            _mockLocationService = new Mock<ILocationService>();
            
            _mockServiceManager.Setup(s => s.LocationService).Returns(_mockLocationService.Object);
            _controller = new LocationController(_mockServiceManager.Object);
        }

        [Fact]
        public async Task GetLocationHistory_WithValidParams_ReturnsOkWithData()
        {
            // Arrange
            int patientId = 1;
            var fromDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5));
            var toDate = DateOnly.FromDateTime(DateTime.UtcNow);
            int limit = 50;
            
            var expectedHistory = new List<LocationHistoryDto>
            {
                new() { Lat = 30.1234, Lng = 31.5678, RecordedAt = DateTime.UtcNow.AddDays(-4), PlaceName = "Test Place 1" },
                new() { Lat = 30.1235, Lng = 31.5679, RecordedAt = DateTime.UtcNow.AddDays(-3), PlaceName = "Test Place 2" }
            };

            _mockLocationService
                .Setup(s => s.GetLocationHistoryAsync(patientId, fromDate.ToDateTime(TimeOnly.MinValue), toDate.ToDateTime(TimeOnly.MaxValue), limit))
                .ReturnsAsync(expectedHistory);

            // Act
            var result = await _controller.GetLocationHistory(patientId, fromDate, toDate, limit);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedHistory = Assert.IsAssignableFrom<IEnumerable<LocationHistoryDto>>(okResult.Value);
            Assert.Equal(expectedHistory, returnedHistory);
        }

        [Fact]
        public async Task GetLocationHistory_WithDefaultFromDate_ReturnsBadRequest()
        {
            // Arrange
            int patientId = 1;
            DateOnly fromDate = default;
            var toDate = DateOnly.FromDateTime(DateTime.UtcNow);
            int limit = 50;

            // Act
            var result = await _controller.GetLocationHistory(patientId, fromDate, toDate, limit);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
            Assert.Equal(400, apiResponse.StatusCode);
            Assert.Equal("Both 'from' and 'to' date parameters are required and must be valid ISO dates.", apiResponse.Message);
        }

        [Fact]
        public async Task GetLocationHistory_WithDefaultToDate_ReturnsBadRequest()
        {
            // Arrange
            int patientId = 1;
            var fromDate = DateOnly.FromDateTime(DateTime.UtcNow);
            DateOnly toDate = default;
            int limit = 50;

            // Act
            var result = await _controller.GetLocationHistory(patientId, fromDate, toDate, limit);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
            Assert.Equal(400, apiResponse.StatusCode);
            Assert.Equal("Both 'from' and 'to' date parameters are required and must be valid ISO dates.", apiResponse.Message);
        }

        [Fact]
        public async Task GetLocationHistory_WhenServiceThrows_ReturnsBadRequestWithExceptionMessage()
        {
            // Arrange
            int patientId = 1;
            var fromDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5));
            var toDate = DateOnly.FromDateTime(DateTime.UtcNow);
            int limit = 50;
            string exceptionMessage = "Database connection error";

            _mockLocationService
                .Setup(s => s.GetLocationHistoryAsync(patientId, fromDate.ToDateTime(TimeOnly.MinValue), toDate.ToDateTime(TimeOnly.MaxValue), limit))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetLocationHistory(patientId, fromDate, toDate, limit);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
            Assert.Equal(400, apiResponse.StatusCode);
            Assert.Equal(exceptionMessage, apiResponse.Message);
        }

        [Fact]
        public async Task GetLocationHistory_WhenNoData_ReturnsNoContent()
        {
            // Arrange
            int patientId = 1;
            var fromDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5));
            var toDate = DateOnly.FromDateTime(DateTime.UtcNow);
            int limit = 50;

            _mockLocationService
                .Setup(s => s.GetLocationHistoryAsync(patientId, fromDate.ToDateTime(TimeOnly.MinValue), toDate.ToDateTime(TimeOnly.MaxValue), limit))
                .ReturnsAsync(new List<LocationHistoryDto>());

            // Act
            var result = await _controller.GetLocationHistory(patientId, fromDate, toDate, limit);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
