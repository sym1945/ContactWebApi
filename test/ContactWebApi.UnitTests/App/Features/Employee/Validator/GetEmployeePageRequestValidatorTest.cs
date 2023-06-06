using ContactWebApi.App.Features.Employee.Queries;
using ContactWebApi.Domain.Models;
using static ContactWebApi.App.Constants.Employee.EmployeePaging;
using static ContactWebApi.UnitTests.Common;

namespace ContactWebApi.UnitTests.App.Features.Employee.Validator
{
    public class GetEmployeePageRequestValidatorTest
    {
        [Test]
        public void IsValid()
        {
            var model = CreateEmployeePageRequest();
            var validator = new GetEmployeePageRequestValidator();
            var result = validator.IsValid(model, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase(1)]
        [TestCase(100)]
        public void IsValid_PageSize(int pageSize)
        {
            var model = CreateEmployeePageRequest(pageSize: pageSize);
            var validator = new GetEmployeePageRequestValidator();
            var result = validator.IsValid(model, out _);

            Assert.That(result, Is.True);
        }

        [Test]
        public void InvalidPage_IsEmpty()
        {
            var model = CreateEmployeePageRequest(page: null);
            var validator = new GetEmployeePageRequestValidator();
            var result = validator.IsValid(model, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(model.Page)));
                Assert.That(errors[0].Description, Is.EqualTo($"The {nameof(model.Page)} filed is required."));
            });
        }

        [Test]
        public void InvalidPage_LessThan()
        {
            var model = CreateEmployeePageRequest(page: 0);
            var validator = new GetEmployeePageRequestValidator();
            var result = validator.IsValid(model, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(model.Page)));
                Assert.That(errors[0].Description, Is.EqualTo($"The field {nameof(model.Page)} can not be less than {PageMin}"));
            });
        }


        [Test]
        public void InvalidPageSize_IsEmpty()
        {
            var model = CreateEmployeePageRequest(pageSize: null);
            var validator = new GetEmployeePageRequestValidator();
            var result = validator.IsValid(model, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(model.PageSize)));
                Assert.That(errors[0].Description, Is.EqualTo($"The {nameof(model.PageSize)} filed is required."));
            });
        }

        [Test]
        [TestCase(0)]
        [TestCase(101)]
        public void InvalidPageSize_OutOfRange(int pageSize)
        {
            var model = CreateEmployeePageRequest(pageSize: pageSize);
            var validator = new GetEmployeePageRequestValidator();
            var result = validator.IsValid(model, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(model.PageSize)));
                Assert.That(errors[0].Description, Is.EqualTo($"The field {nameof(model.PageSize)} must be between {PageMin} and {PageMax}."));
            });
        }

    }
}
