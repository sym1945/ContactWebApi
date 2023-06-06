using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.Domain.Models;
using static ContactWebApi.App.Constants.Employee.EmployeeFiled;
using static ContactWebApi.UnitTests.Common;

namespace ContactWebApi.UnitTests.App.Features.Employee.Validator
{
    public class EmployeeDtoValidatorTest
    {
        [Test]
        public void IsValid()
        {
            var dto = CreateEmployee();
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out _);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void IsValid_NameLength()
        {
            var dto = CreateEmployee(name: CreateString(NameMax));
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out _);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void IsValid_EmailLength()
        {
            var emailSubFix = "@gmail.com";
            var email = $"{CreateString(EmailMax - emailSubFix.Length)}{emailSubFix}";
            var dto = CreateEmployee(email: email);
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out _);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void IsValid_TelLength()
        {
            var dto = CreateEmployee(tel: CreateString(TelMax));
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out ModelError[] errors);

            Assert.That(errors[0].Description, Is.Not.EqualTo($"The field {nameof(dto.Tel)} must be a string or array type with a maximum length of '{TelMax}'"));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void InvalidName_NullOrEmptyOrWhiteSpace(string? name)
        {
            var dto = CreateEmployee(name: name);
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(dto.Name)));
                Assert.That(errors[0].Description, Is.EqualTo($"The {nameof(dto.Name)} field cannot be blank"));
            });
        }

        [Test]
        public void InvalidName_OverMaxLength()
        {
            var dto = CreateEmployee(name: CreateString(NameMax + 1));
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(dto.Name)));
                Assert.That(errors[0].Description, Is.EqualTo($"The field {nameof(dto.Name)} must be a string or array type with a maximum length of '{NameMax}'"));
            });
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void InvalidEmail_NullOrEmptyOrWhiteSpace(string? email)
        {
            var dto = CreateEmployee(email: email);
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(dto.Email)));
                Assert.That(errors[0].Description, Is.EqualTo($"The {nameof(dto.Email)} field cannot be blank"));
            });
        }

        [Test]
        public void InvalidEmail_OverMaxLength()
        {
            var emailSubFix = "@gmail.com";
            var email = $"{CreateString(EmailMax - emailSubFix.Length + 1)}{emailSubFix}";

            var dto = CreateEmployee(email: email);
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(dto.Email)));
                Assert.That(errors[0].Description, Is.EqualTo($"The field {nameof(dto.Email)} must be a string or array type with a maximum length of '{EmailMax}'"));
            });
        }

        [Test]
        [TestCase("@asdf.com", ExpectedResult = false)]
        [TestCase("asdf@", ExpectedResult = false)]
        [TestCase("asdf@asdf", ExpectedResult = false)]
        [TestCase("asdf@.com", ExpectedResult = false)]
        [TestCase("asdfasdf.com", ExpectedResult = false)]
        public bool InvalidEmail_InvalidFormat(string email)
        {
            var dto = CreateEmployee(email: email);
            var validator = new EmployeeDtoValidator();
            return validator.IsValid(dto, out _);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void InvalidTel_NullOrEmptyOrWhiteSpace(string? tel)
        {
            var dto = CreateEmployee(tel: tel);
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(dto.Tel)));
                Assert.That(errors[0].Description, Is.EqualTo($"The {nameof(dto.Tel)} field cannot be blank"));
            });
        }

        [Test]
        public void InvalidTel_OverMaxLength()
        {
            var dto = CreateEmployee(tel: CreateString(TelMax + 1));
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out ModelError[] errors);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(false));
                Assert.That(errors, Has.Length.EqualTo(1));
                Assert.That(errors[0].Name, Is.EqualTo(nameof(dto.Tel)));
                Assert.That(errors[0].Description, Is.EqualTo($"The field {nameof(dto.Tel)} must be a string or array type with a maximum length of '{TelMax}'"));
            });
        }

        [Test]
        [TestCase("01012345678", ExpectedResult = false)]
        [TestCase("01-1234-5678", ExpectedResult = false)]
        [TestCase("0110-1234-5678", ExpectedResult = false)]
        [TestCase("010-123-5678", ExpectedResult = false)]
        [TestCase("010-12345-5678", ExpectedResult = false)]
        [TestCase("010-1234-567", ExpectedResult = false)]
        [TestCase("010-1234-56789", ExpectedResult = false)]
        [TestCase("aaa-aaaa-aaaa", ExpectedResult = false)]
        public bool InvalidTel_InvalidFormat(string tel)
        {
            var dto = CreateEmployee(tel: tel);
            var validator = new EmployeeDtoValidator();
            return validator.IsValid(dto, out _);
        }

        [Test]
        public void InvalidJoined_WrongData()
        {
            var dto = CreateEmployee(joined: null);
            var validator = new EmployeeDtoValidator();
            var result = validator.IsValid(dto, out ModelError[] errors);

            Assert.That(result, Is.EqualTo(false));
            Assert.That(errors, Has.Length.EqualTo(1));
            Assert.That(errors[0].Name, Is.EqualTo(nameof(dto.Joined)));
        }

    }
}
