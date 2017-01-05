using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TableIO.Tests
{
    [TestClass()]
    public class DefaultModelValidatorTests
    {
        public class Model
        {
            [Required(ErrorMessage = "NoProp1")]
            public string Prop1 { get; set; }
            [RegularExpression("[a-z]+", ErrorMessage = "InvalidProp2")]
            [StringLength(10, ErrorMessage = "TooLongProp2")]
            public string Prop2 { get; set; }
        }

        [TestMethod()]
        public void ValidateOK()
        {
            var model = new Model { Prop1 = "a", Prop2 = "b" };
            var errors = new DefaultModelValidator().Validate(model);
            Assert.IsFalse(errors.Any());
        }

        [TestMethod()]
        public void ValidateNG()
        {
            var model = new Model { Prop1 = null, Prop2 = "01234567890" };
            var errors = new DefaultModelValidator().Validate(model);
            Assert.AreEqual(3, errors.Count());
            var error = errors.First();
            Assert.AreEqual("ModelValidation", error.Type);
            Assert.AreEqual("NoProp1", error.Message);
            CollectionAssert.AreEqual(new[] { "Prop1" }, error.MemberNames.ToArray());

        }
    }
}