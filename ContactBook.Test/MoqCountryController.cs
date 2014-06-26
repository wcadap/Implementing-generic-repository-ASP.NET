using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using Moq;

using ContactBook.Core.Models;
using ContactBook.Core.Repository;
using ContactBook.MVC.Controllers;
using System.Web.Mvc;

namespace ContactBook.Test
{
    [TestClass]
    public class MoqCountryController
    {
        private Mock<IGenericRepo> GenericToMock;
        private IList<Country> Countries;
        private CountryController countryConroller;

        [TestInitialize]
        public void TestInitialize()
        {
            GenericToMock = new Mock<IGenericRepo>();
            Countries = new List<Country>();
            countryConroller = new CountryController(GenericToMock.Object);

            Countries.Add(new Country { Id = 1, CountryName = "Japan" });
            Countries.Add(new Country { Id = 2, CountryName = "Singapore" });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GenericToMock = null;
            Countries = null;
            countryConroller = null;
            
        }
        #region Test for Index
        [TestMethod]
        public void CountryController_Index()
        {
            //Arrange
            GenericToMock
                .Setup(r => r.Query<Country>())
                .Returns(Countries.AsQueryable());

            //Act
            ViewResult result = countryConroller.Index() as ViewResult;
            IEnumerable<Country> model = result.Model as IEnumerable<Country>;

            // Assert
            Assert.AreEqual(2, model.Count());

            GenericToMock.Verify(x => x.Query<Country>(),
                Times.Exactly(1));
        }
        #endregion End for Index
        #region Test for Create
        [TestMethod]
        public void CountryController_Create()
        {
            //Arrange
            //Act
            ViewResult result = countryConroller.Create() as ViewResult;
            
            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CountryController_Create_Record_Redirect_To_Index_On_Success()
        {
            //Arrange
            var _country = new Country() { Id=4, CountryName = "Netherland" };

            // Act

            var result = countryConroller.Create(_country) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);

            GenericToMock.Verify(x => x.Add<Country>(It.IsAny<Country>()));
        }

        [TestMethod]
        public void CountryController_Create_Save_Record_Save_Method_Was_Called_On_Success()
        {
            //Arrange
            var _country = new Country() { Id = 4, CountryName = "Netherland" };

            GenericToMock
                .Setup(r => r.Save())
                .Returns(true);

            //Act
            var result = countryConroller.Create(_country) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);

            GenericToMock.Verify(x => x.Save());
        }

        [TestMethod]
        public void CountryController_Create_Record_Return_View_On_Invalid()
        {
            //Arrange
            var _country = new Country() { Id = 0, CountryName=""};

            countryConroller.ViewData.ModelState.Clear();
            countryConroller.ModelState.AddModelError("Error","Model is invalid.");

            //Act
            var result = countryConroller.Create(_country) as ViewResult;

            //Assert

            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void CountryController_Create_Record_Return_Same_Model_On_Invalid()
        {
            //Arrange
            var _country = new Country() { Id = 0, CountryName = "" };

            countryConroller.ViewData.ModelState.Clear();
            countryConroller.ModelState.AddModelError("Error", "Model is invalid.");

            //Act
            var result = countryConroller.Create(_country) as ViewResult;

            var model = result.Model as Country;
            //Assert

            Assert.AreEqual(result.Model, _country);
            Assert.AreEqual(0, model.Id);
            //GenericToMock.
        }
#endregion End for Create Test
        #region Test for Details
        [TestMethod]
        public void CountryController_Details()
        {
            //Arrange
            GenericToMock
                .Setup(r => r.GetById<Country>(1))
                .Returns(new Country{Id=1,CountryName="Japan"});

            //Act
            ViewResult result = countryConroller.Details(1) as ViewResult;
            Country model = result.Model as Country;

            // Assert
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Japan", model.CountryName);

            GenericToMock.Verify(x => x.GetById<Country>(It.IsAny<int>()));
        }

        [TestMethod]
        public void CountryController_Details_Returns_404_If_No_Country_Code()
        {
            //Arrange
            
            //Act
            var result = countryConroller.Details(5);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }
        #endregion End for Test for Details
        #region Test for Edit
        [TestMethod]
        public void CountryController_Edit_Returns_CountryModel_On_Valid()
        {
            //Arrange
            GenericToMock
                .Setup(r => r.GetById<Country>(1))
                .Returns(new Country { Id = 1, CountryName = "Japan" });

            //Act
            ViewResult result = countryConroller.Edit(1) as ViewResult;
            Country model = result.Model as Country;

            // Assert
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Japan", model.CountryName);

            GenericToMock.Verify(x => x.GetById<Country>(It.IsAny<int>()));
        }

        [TestMethod]
        public void CountryController_Edit_Returns_404_If_No_Country_Code_Found()
        {
            //Arrange
            
            //Act
            var result = countryConroller.Edit(5);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void CountryController_Edit_Save_Record_Redirect_To_Index_On_Success()
        {
            //Assert

            var _country = new Country() { Id = 4, CountryName = "Netherland" };

            // Act

            var result = countryConroller.Edit(_country) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod]
        public void CountryController_Edit_Save_Record_Save_Method_Was_Called_On_Success()
        {
            //Arrange
            var _country = new Country() { Id = 4, CountryName = "Netherland" };

            GenericToMock
                .Setup(r => r.Save())
                .Returns(true);

            //Act
            var result = countryConroller.Edit(_country) as RedirectToRouteResult;
            //Assert
            GenericToMock.Verify(x => x.Save());
        }

        [TestMethod]
        public void CountryController_Edit_Save_Record_Return_View_On_Invalid_Model()
        {
            //Arrange
            countryConroller = new CountryController(GenericToMock.Object);
            var _country = new Country() { Id = 0, CountryName = "" };

            countryConroller.ViewData.ModelState.Clear();
            countryConroller.ModelState.AddModelError("Error", "Model is invalid.");

            //Act
            var result = countryConroller.Edit(_country) as ViewResult;

            //Assert

            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod]
        public void CountryController_Edit_Save_Record_Return_Same_Model_On_Invalid()
        {
            //Arrange
            var _country = new Country() { Id = 0, CountryName = "" };

            countryConroller.ViewData.ModelState.Clear();
            countryConroller.ModelState.AddModelError("Error", "Model is invalid.");

            //Act
            var result = countryConroller.Edit(_country) as ViewResult;

            var model = result.Model as Country;
            //Assert

            Assert.AreEqual(result.Model, _country);
            Assert.AreEqual(0, model.Id);
        }

        #endregion End for Test Edit 

        #region Test for Delete
        [TestMethod]
        public void CountryController_Delete_Returns_Same_Model_When_Found()
        {
            //Arrange
            GenericToMock
                .Setup(r => r.GetById<Country>(1))
                .Returns(new Country { Id = 1, CountryName = "Japan" });

            countryConroller = new CountryController(GenericToMock.Object);

            //Act
            ViewResult result = countryConroller.Delete(1) as ViewResult;
            Country model = result.Model as Country;

            // Assert
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Japan", model.CountryName);

            GenericToMock.Verify(x => x.GetById<Country>(It.IsAny<int>()));
        }

        [TestMethod]
        public void CountryController_Delete_Return_httpNoFound_When_Country_Code_Not_Found()
        {
            //Arrange

            //Act
            var result = countryConroller.Delete(1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));

        }

        #endregion End for Delete Method

        #region Test for DeleteConfirm
        [TestMethod]
        public void CountryController_DeleteConfirmed_Redirect_To_Index_On_Success()
        {
            //Assert

            var _country = new Country() { Id = 4, CountryName = "Netherland" };

            // Act

            var result = countryConroller.DeleteConfirmed(_country) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [TestMethod]
        public void CountryController_DeleteConfirmed_Post_Action_Returns_RedirectToAction()
        {
            //Arrange
            GenericToMock.Setup(x =>
                x.Delete<Country>(1));

            var _country = new Country() { Id = 1, CountryName = "Japan" };

            //Act
            var result = countryConroller.DeleteConfirmed(_country);

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void CountryController_DeleteConfirmed_Delete_Method_Was_Called()
        {
            //Arrange
            var _country = new Country() { Id = 4, CountryName = "Netherland" };

            GenericToMock
                .Setup(r => r.Delete<Country>(4));
                //.Returns(true);

            //Act
            var result = countryConroller.DeleteConfirmed(_country) as RedirectToRouteResult;
            //Assert
            GenericToMock.Verify(x => x.Delete<Country>(It.IsAny<int>()));
        }

        [TestMethod]
        public void CountryController_DeleteConfirmed_Save_Method_Was_Called_On_Success()
        {
            //Arrange
            var _country = new Country() { Id = 4, CountryName = "Netherland" };

            GenericToMock
                .Setup(r => r.Save())
                .Returns(true);

            //Act
            var result = countryConroller.Edit(_country) as RedirectToRouteResult;
            //Assert
            GenericToMock.Verify(x => x.Save());
        }

        [TestMethod]
        public void CountryController_DeleteConfirmed_Return_Same_Model_On_Not_Found_Error()
        {
            //Arrange
            var _country = new Country() { Id = 0, CountryName = "" };

            countryConroller.ViewData.ModelState.Clear();
            countryConroller.ModelState.AddModelError("Error", "Model is invalid.");

            //Act
            var result = countryConroller.DeleteConfirmed(_country) as ViewResult;

            var model = result.Model as Country;

            //Assert

            Assert.AreEqual(result.Model, _country);
            Assert.AreEqual(0, model.Id);

            Assert.IsTrue(countryConroller.ViewData.ModelState.Count == 2);

        }

        #endregion
    }
}
