using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using WebAPI.Controllers;

namespace WebAPI.Tests.Controllers
{
    [TestClass]
    public class DistrictControllerTest
    {
        private List<District> testData;

        public DistrictControllerTest()
        {
            this.testData = CreateTestDistricts();
        }

        [TestMethod]
        public void GetAllDistrictsTest()
        {
            //Arrange
            var controller = new DistrictController();
            //Act
            IHttpActionResult result = controller.GetAllDistricts();
            var contentResult = result as OkNegotiatedContentResult<List<District>>;
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(testData.Count, contentResult.Content.Count);
        }

        [TestMethod]
        public void GetDistrictByIdTest()
        {
            //Arrange
            var controller = new DistrictController();
            //Act
            IHttpActionResult result = controller.GetDistrict(1);
            var contentResult = result as OkNegotiatedContentResult<District>;
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        [TestMethod]
        public void GetDistrictNotFoundTest()
        {
            //Arrange
            var controller = new DistrictController();
            //Act
            IHttpActionResult result = controller.GetDistrict(100);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateDistrictTest()
        {
            //Arrange
            var controller = new DistrictController();
            District testDistrict = new District { Id = 6, Name = "Test Update", PrimarySalesperson = new Salesperson { Id = 1 }, Salespersons = new List<Salesperson>() };
            //Act
            IHttpActionResult result = controller.PutDistrict(testDistrict);
            var contentResult = result as NegotiatedContentResult<District>;
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

        private List<District> CreateTestDistricts()
        {
            var testDistricts = new List<District>();
            testDistricts.Add(new District { Id = 1, Name = "North Denmark", PrimarySalesperson = new Salesperson { Id = 1, Name = "Jens Jensen" }, Salespersons = new List<Salesperson>(), Stores = new List<Store>() });
            testDistricts.Add(new District { Id = 2, Name = "South Denmark", PrimarySalesperson = new Salesperson { Id = 2, Name = "John Smith" }, Salespersons = new List<Salesperson>(), Stores = new List<Store>() });
            testDistricts.Add(new District { Id = 3, Name = "West Denmark", PrimarySalesperson = new Salesperson { Id = 1, Name = "Alice Johnson" }, Salespersons = new List<Salesperson>(), Stores = new List<Store>() });
            testDistricts.Add(new District { Id = 4, Name = "East Denmark", PrimarySalesperson = new Salesperson { Id = 1, Name = "Mary Smith" }, Salespersons = new List<Salesperson>(), Stores = new List<Store>() });
            testDistricts.Add(new District { Id = 5, Name = "Central Denmark", PrimarySalesperson = new Salesperson { Id = 1, Name = "Lisa Stanley" }, Salespersons = new List<Salesperson>(), Stores = new List<Store>() });
            testDistricts.Add(new District { Id = 6, Name = "Test", PrimarySalesperson = new Salesperson { Id = 1, Name = "Lisa Stanley" }, Salespersons = new List<Salesperson>(), Stores = new List<Store>() });
            return testDistricts;
        }
    }
}
