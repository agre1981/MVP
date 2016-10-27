using DevExpress.XtraEditors.Controls;
using Moq;
using MVP.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVP.Tests.UI
{
    [TestFixture]
    public class PresenterTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage="Argument view can not be null")]
        public void ConstructorViewNullExceptionTest()
        {
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(null, serviceMock.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage="Argument service can not be null")]
        public void ConstructorServiceNullExceptionTest()
        {
            var viewMock = new Mock<IView>();
            var presenter = new Presenter(viewMock.Object, null);
        }

        [Test]
        public void FormLoadEventTest()
        {
            // Setup testing data and class dependencies
            var model = new BindingList<Model> { new Model(1, "Name 1", 1, 1, 2, 1) };
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            serviceMock.Setup(s => s.GetData(It.Is<long>(id=> id == 1))).Returns(model);
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising testing event view.FormLoad
            viewMock.Raise(v => v.FormLoad += null);

            // Verifing testing results
            serviceMock.Verify(
                s => s.GetData(It.Is<long>(id => id == 1)), 
                Times.Once(), 
                "Verify pooling of method: service.GetData(1)" );

            viewMock.Verify(view => view.BindModel(It.Is<BindingList<Model>>(data => data == model)), 
                Times.Once(),
                "Verify pooling of method: view.BindModel(data)");
        }

        [Test]
        public void IncreaseAClickEventTest()
        {
            // Setup testing data and class dependencies
            var entity = new Model(1, "Name 1", 5, 2, 7, 2);
            var model = new BindingList<Model> { entity };
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            serviceMock.Setup(s => s.GetData(It.Is<long>(id => id == 1))).Returns(model);
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising event view.FormLoad to init presenter.model 
            viewMock.Raise(v => v.FormLoad += null);

            // Raising testing event view.IncreaseAClick
            viewMock.Raise(v => v.IncreaseAClick += null, entity);

            // Verifing testing results
            Assert.AreEqual(6, entity.A, "Validate increasing of model.A property");
            Assert.AreEqual(8, entity.Sum, "Validate changing of model.Sum property");
            Assert.AreEqual(3, entity.Div, "Validate changing of model.Div property");
        }

        [Test]
        public void GridCellValueChangedEventForATest()
        {
            // Setup testing data and class dependencies
            var entity = new Model(1, "Name 1", 4, 2, 0, 0);
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising testing event view.GridCellValueChanged("A", entity)
            viewMock.Raise(v => v.GridCellValueChanged += null, "A", entity);

            // Verifing testing results
            Assert.AreEqual(6, entity.Sum, "Validate changing of model.Sum property");
            Assert.AreEqual(2, entity.Div, "Validate changing of model.Div property");
        }

        [Test]
        public void GridCellValueChangedEventForBTest()
        {
            // Setup testing data and class dependencies
            var entity = new Model(1, "Name 1", 4, 2, 0, 0);
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising testing event view.GridCellValueChanged("B", entity)
            viewMock.Raise(v => v.GridCellValueChanged += null, "B", entity);

            // Verifing testing results
            Assert.AreEqual(6, entity.Sum, "Validate changing of model.Sum property");
            Assert.AreEqual(2, entity.Div, "Validate changing of model.Div property");
        }

        [Test]
        public void GridCellValueChangedEventForCTest()
        {
            // Setup testing data and class dependencies
            var entity = new Model(1, "Name 1", 4, 2, 0, 0);
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising testing event view.GridCellValueChanged("C", entity)
            viewMock.Raise(v => v.GridCellValueChanged += null, "C", entity);

            // Verifing testing results
            Assert.AreEqual(0, entity.Sum, "Validate not changing of model.Sum property. Should be recalculated for A&B properties only");
            Assert.AreEqual(0, entity.Div, "Validate not changing of model.Div property. Should be recalculated for A&B properties only");
        }

        [Test]
        public void SaveClickEventTest()
        {
            // Setup testing data and class dependencies
            var entity = new Model(1, "Name 1", 5, 2, 7, 2);
            var model = new BindingList<Model> { entity };
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            serviceMock.Setup(s => s.GetData(It.Is<long>(id => id == 1))).Returns(model);
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising event view.FormLoad to init presenter.model 
            viewMock.Raise(v => v.FormLoad += null);

            // Raising event view.IncreaseAClick to change model
            viewMock.Raise(v => v.IncreaseAClick += null, entity);

            // Raising testing event view.SaveClick()
            viewMock.Raise(v => v.SaveClick += null);

            // Verifing testing results
            serviceMock.Verify(
                s => s.SaveData(
                    It.Is<BindingList<Model>>(
                        m => m.Count == 1
                        && m[0].Id == 1
                        && m[0].Name == "Name 1"
                        && m[0].A == 6
                        && m[0].B == 2
                        && m[0].Sum == 8
                        && m[0].Div == 3
                        )),
                Times.Once(),
                "Verify pooling of method: service.SaveData(data)");
        }

        [Test]
        public void SaveClickEventShowErrorMessageTest()
        {
            // Setup testing data and class dependencies
            var model = new BindingList<Model> { };
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            serviceMock.Setup(s => s.GetData(It.Is<long>(id => id == 1))).Returns(model);
            serviceMock.Setup(s => s.SaveData(It.Is<BindingList<Model>>(m => m==model))).Throws(new ServiceException("Error during save operation"));
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising event view.FormLoad to init presenter.model 
            viewMock.Raise(v => v.FormLoad += null);

            // Raising testing event view.SaveClick()
            viewMock.Raise(v => v.SaveClick += null);

            // Verifing testing results
            viewMock.Verify(
                v => v.ShowErrorMessage(
                    It.Is<string>(s => s == "Error during save operation")),
                    Times.Once(),
                    "Verify pooling of method: view.ShowErrorMessage(message)");
        }

        [Test]
        public void GridValidatingEditorEventForNullTest()
        {
            // Setup testing data and class dependencies
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising event view.GridValidatingEditor("B", e) to change model
            var e = new BaseContainerValidateEditorEventArgs(null);
            viewMock.Raise(v => v.GridValidatingEditor += null, "B", e);

            // Verifing testing results
            Assert.AreEqual(false, e.Valid, "Validate failed validation");
            Assert.AreEqual("Field B should not be empty !", e.ErrorText, "Validate error message");
        }

        [Test]
        public void GridValidatingEditorEventForBEmptyTest()
        {
            // Setup testing data and class dependencies
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising event view.GridValidatingEditor("B", e) to change model
            var e = new BaseContainerValidateEditorEventArgs(string.Empty);
            viewMock.Raise(v => v.GridValidatingEditor += null, "B", e);

            // Verifing testing results
            Assert.AreEqual(false, e.Valid, "Validate failed validation");
            Assert.AreEqual("Field B should not be empty !", e.ErrorText, "Validate error message");
        }

        [Test]
        public void GridValidatingEditorEventForBSpaceTest()
        {
            // Setup testing data and class dependencies
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising event view.GridValidatingEditor("B", e) to change model
            var e = new BaseContainerValidateEditorEventArgs("   ");
            viewMock.Raise(v => v.GridValidatingEditor += null, "B", e);

            // Verifing testing results
            Assert.AreEqual(false, e.Valid, "Validate failed validation");
            Assert.AreEqual("Field B should not be empty !", e.ErrorText, "Validate error message");
        }

        [Test]
        public void GridValidatingEditorEventForBZeroTest()
        {
            // Setup testing data and class dependencies
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising event view.GridValidatingEditor("B", e) to change model
            var e = new BaseContainerValidateEditorEventArgs(0);
            viewMock.Raise(v => v.GridValidatingEditor += null, "B", e);

            // Verifing testing results
            Assert.AreEqual(false, e.Valid, "Validate failed validation");
            Assert.AreEqual("Field B should not be equal 0 !", e.ErrorText, "Validate error message");
        }

        [Test]
        public void GridValidatingEditorEventForBNotNumberTest()
        {
            // Setup testing data and class dependencies
            var viewMock = new Mock<IView>();
            var serviceMock = new Mock<IService>();
            var presenter = new Presenter(viewMock.Object, serviceMock.Object);

            // Raising event view.GridValidatingEditor("B", e) to change model
            var e = new BaseContainerValidateEditorEventArgs("3z12");
            viewMock.Raise(v => v.GridValidatingEditor += null, "B", e);

            // Verifing testing results
            Assert.AreEqual(false, e.Valid, "Validate failed validation");
            Assert.AreEqual("Field B should be a number !", e.ErrorText, "Validate error message");
        }

        [Test]
        public void InternalMethodSumTest()
        {
            // See details in MVP\Properties\AssemblyInfo.cs
            // [assembly: InternalsVisibleTo("MVP.Tests")]
            // Details are on https://msdn.microsoft.com/en-us/library/0tke9fxk.aspx 

            long res = Presenter.Sum(5, 10);
            Assert.AreEqual(15, res, "Validate internal method Presenter.Sum(5, 10)");
        }
    }
}
