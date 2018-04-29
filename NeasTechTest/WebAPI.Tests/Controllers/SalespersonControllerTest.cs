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
    public class SalespersonControllerTest
    {

        public SalespersonControllerTest()
        {

        }

        [TestMethod]
        public void GetAllSalespersonsTest()
        {
            //Arrange
            var controller = new SalespersonController();
            //Act
            IHttpActionResult result = controller.GetAllSalespersons();
            var contentResult = result as OkNegotiatedContentResult<List<Salesperson>>;
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(10, contentResult.Content.Count);
        }

        [TestMethod]
        public void GetSalespersonByIdTest()
        {
            //Arrange
            var controller = new SalespersonController();
            //Act
            IHttpActionResult result = controller.GetSalesperson(1);
            var contentResult = result as OkNegotiatedContentResult<Salesperson>;
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        [TestMethod]
        public void GetSalespersonNotFoundTest()
        {
            //Arrange
            var controller = new SalespersonController();
            //Act
            IHttpActionResult result = controller.GetSalesperson(100);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateDistrictTest()
        {
            //Arrange
            var controller = new SalespersonController();
            var testSalesperson = new Salesperson { Id = 10, Name = "Test Updated" }; 
            //Act
            IHttpActionResult result = controller.PutSalesperson(testSalesperson);
            var contentResult = result as NegotiatedContentResult<Salesperson>;
            //Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.IsNotNull(contentResult.Content);
        }

    }
}

