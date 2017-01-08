using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace TableIO.Tests
{
    [TestClass()]
    public class TableReaderTests
    {
        class Model
        {
            public int PInt { get; set; }
            [StringLength(3, ErrorMessage = "PStringError")]
            public string PString { get; set; }
            [Required(ErrorMessage = "PNIntError")]
            public int? PNInt { get; set; }
        }

        private TableReader<TModel> CreateTableReader<TModel>(string str,
            IRowReader rowReader = null, ITypeConverterResolver typeConvResolver = null,
            IPropertyMapper mapper = null, IModelValidator validator = null) 
            where TModel : new()
        {
            var strReader = new StringReader(str);
            rowReader = rowReader ?? new CsvRegexRowReader(strReader);
            typeConvResolver = typeConvResolver ?? new DefaultTypeConverterResolver<TModel>();
            mapper = mapper ?? new AutoIndexPropertyMapper();
            validator = validator ?? new NullModelValidator();

            var tableReader = new TableReader<TModel>(rowReader, typeConvResolver, mapper, validator);
            return tableReader;
        }

        #region ConvertFromRow

        [TestMethod()]
        public void ConvertFromRow()
        {
            var reader = CreateTableReader<Model>("");
            var maps = new AutoIndexPropertyMapper().CreatePropertyMaps(typeof(Model), null);

            var model = reader.ConvertFromRow(new[] { "1", "abc", "10" }, 0, maps);
            Assert.AreEqual(1, model.PInt);
            Assert.AreEqual("abc", model.PString);
            Assert.AreEqual(10, model.PNInt);

            model = reader.ConvertFromRow(new[] { "1", "abc", null }, 0, maps);
            Assert.AreEqual(null, model.PNInt);
        }

        [TestMethod]
        public void ConvertFromRow_ConvertFailed()
        {
            var reader = CreateTableReader<Model>("");
            var maps = new AutoIndexPropertyMapper().CreatePropertyMaps(typeof(Model), null);

            try
            {
                reader.ConvertFromRow(new[] { "NG", "abc", "10" }, 2, maps);
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);

                var error = ex.Errors[0];
                Assert.AreEqual("ConvertFailed", error.Type);
                Assert.AreEqual(2, error.RowIndex);
                Assert.AreEqual(0, error.ColumnIndex);
                Assert.AreEqual("PInt", error.MemberNames.First());
            }

            try
            {
                reader.ConvertFromRow(new[] { "1", "abc", "NG" }, 2, maps);
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                var error = ex.Errors[1];
                Assert.AreEqual(2, error.ColumnIndex);
                Assert.AreEqual("PNInt", error.MemberNames.First());
            }
        }

        [TestMethod]
        public void ConvertFromRow_2ConvertFailed()
        {
            var reader = CreateTableReader<Model>("");
            reader.ErrorLimit = 2;
            var maps = new AutoIndexPropertyMapper().CreatePropertyMaps(typeof(Model), null);

            try
            {
                reader.ConvertFromRow(new[] { "NG", "abc", "NG" }, 2, maps);
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(2, ex.Errors.Count);

                var error = ex.Errors[0];
                Assert.AreEqual("ConvertFailed", error.Type);
                Assert.AreEqual(2, error.RowIndex);
                Assert.AreEqual(0, error.ColumnIndex);
                CollectionAssert.AreEqual(new[] { "PInt" }, error.MemberNames.ToArray());

                error = ex.Errors[1];
                Assert.AreEqual(2, error.ColumnIndex);
                CollectionAssert.AreEqual(new[] { "PNInt" }, error.MemberNames.ToArray());
            }
        }

        [TestMethod]
        public void ConvertFromRow_ModelValidationError()
        {
            var reader = CreateTableReader<Model>("", validator: new DefaultModelValidator());
            var maps = new AutoIndexPropertyMapper().CreatePropertyMaps(typeof(Model), null);

            try
            {
                reader.ConvertFromRow(new[] { "1", "TOO_LONG", "10" }, 2, maps);
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);

                var error = ex.Errors[0];
                Assert.AreEqual("ModelValidation", error.Type);
                Assert.AreEqual("PStringError", error.Message);
                Assert.AreEqual(2, error.RowIndex);
                CollectionAssert.AreEqual(new[] { "PString" }, error.MemberNames.ToArray());
            }

            try
            {
                reader.ConvertFromRow(new[] { "1", "abc", null }, 2, maps);
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                var error = ex.Errors[1];
                Assert.AreEqual("PNIntError", error.Message);
                CollectionAssert.AreEqual(new[] { "PNInt" }, error.MemberNames.ToArray());
            }
        }

        [TestMethod]
        public void ConvertFromRow_2ModelValidationError()
        {
            var reader = CreateTableReader<Model>("", validator: new DefaultModelValidator());
            reader.ErrorLimit = 2;
            var maps = new AutoIndexPropertyMapper().CreatePropertyMaps(typeof(Model), null);

            try
            {
                reader.ConvertFromRow(new[] { "1", "TOO_LONG", null }, 2, maps);
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(2, ex.Errors.Count);

                var error = ex.Errors[0];
                Assert.AreEqual("ModelValidation", error.Type);
                Assert.AreEqual("PStringError", error.Message);
                Assert.AreEqual(2, error.RowIndex);
                CollectionAssert.AreEqual(new[] { "PString" }, error.MemberNames.ToArray());

                error = ex.Errors[1];
                Assert.AreEqual("PNIntError", error.Message);
                CollectionAssert.AreEqual(new[] { "PNInt" }, error.MemberNames.ToArray());
            }
        }

        #endregion

        #region Read

        [TestMethod]
        public void ReadNothing()
        {
            var reader = CreateTableReader<Model>("");
            var models = reader.Read();

            Assert.AreEqual(0, models.Count);
        }

        [TestMethod]
        public void ReadNothing_Failed_HasHeader()
        {
            var reader = CreateTableReader<Model>("");
            reader.HasHeader = true;

            try
            {
                reader.Read();
                Assert.Fail();
            }
            catch(TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);
                Assert.AreEqual(ex.Errors[0].Type, "NoTableHeader");
            }
        }

        [TestMethod()]
        public void Read_NoHeader()
        {
            var reader = CreateTableReader<Model>("1,aaa,10\n2,bbb,20");
            var models = reader.Read();

            Assert.AreEqual(2, models.Count);

            var model = models[0];
            Assert.AreEqual(1, model.PInt);
            Assert.AreEqual("aaa", model.PString);
            Assert.AreEqual(10, model.PNInt);

            model = models[1];
            Assert.AreEqual(2, model.PInt);
            Assert.AreEqual("bbb", model.PString);
            Assert.AreEqual(20, model.PNInt);
        }

        [TestMethod]
        public void Read_HasHeader()
        {
            var reader = CreateTableReader<Model>("header1,header2,header3\n1,aaa,10\n2,bbb,20\n");
            reader.HasHeader = true;
            var models = reader.Read();

            Assert.AreEqual(2, models.Count);

            var model = models[0];
            Assert.AreEqual(1, model.PInt);
            Assert.AreEqual("aaa", model.PString);
            Assert.AreEqual(10, model.PNInt);

            model = models[1];
            Assert.AreEqual(2, model.PInt);
            Assert.AreEqual("bbb", model.PString);
            Assert.AreEqual(20, model.PNInt);
        }

        [TestMethod()]
        public void Read_NoHeader_SetColumnSize()
        {
            var reader = CreateTableReader<Model>("1,aaa,10\n2,bbb,20");
            reader.ColumnSize = 3;
            var models = reader.Read();

            Assert.AreEqual(2, models.Count);
        }

        [TestMethod]
        public void Read_HasHeader_SetColumnSize()
        {
            var reader = CreateTableReader<Model>("header1,header2,header3\n1,aaa,10\n2,bbb,20\n");
            reader.HasHeader = true;
            reader.ColumnSize = 3;
            var models = reader.Read();

            Assert.AreEqual(2, models.Count);
        }

        [TestMethod]
        public void ReadFailed_OutOfRangeColumnIndexMapping()
        {
            var reader = CreateTableReader<Model>("1,aaa\n2,bbb");
            reader.ColumnSize = 2;

            try
            {
                reader.Read();
                Assert.Fail();
            }
            catch(TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);
                Assert.AreEqual("OutOfRangeColumnIndexMapping", ex.Errors[0].Type);
                Assert.AreEqual(0, ex.Errors[0].RowIndex);
            }
        }

        [TestMethod]
        public void ReadFailed_FirstRow_InvalidColumnSize()
        {
            var reader = CreateTableReader<Model>("1,aaa,10,NG\n2,bbb,20");
            reader.ColumnSize = 3;

            try
            {
                reader.Read();
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);
                Assert.AreEqual("InvalidColumnSize", ex.Errors[0].Type);
                Assert.AreEqual(0, ex.Errors[0].RowIndex);
            }
        }

        [TestMethod]
        public void ReadFailed_SecondRow_InvalidColumnSize()
        {
            var reader = CreateTableReader<Model>("1,aaa,10\n2,bbb,20,NG");
            //reader.ColumnSize = 3;

            try
            {
                reader.Read();
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);
                Assert.AreEqual("InvalidColumnSize", ex.Errors[0].Type);
                Assert.AreEqual(1, ex.Errors[0].RowIndex);
            }
        }

        [TestMethod]
        public void ReadFailed_2row_InvalidColumnSize()
        {
            var reader = CreateTableReader<Model>("1,aaa,10,NG\n2,bbb,20,NG");
            reader.ColumnSize = 3;
            reader.ErrorLimit = 10;

            try
            {
                reader.Read();
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(2, ex.Errors.Count);
                Assert.AreEqual("InvalidColumnSize", ex.Errors[0].Type);
                Assert.AreEqual(0, ex.Errors[0].RowIndex);
                Assert.AreEqual("InvalidColumnSize", ex.Errors[1].Type);
                Assert.AreEqual(1, ex.Errors[1].RowIndex);
            }
        }

        [TestMethod]
        public void ReadFailed_CallingConvertFromRow_ConvertFailed()
        {
            var reader = CreateTableReader<Model>("1,aaa,NG\n2,bbb,20");

            try
            {
                reader.Read();
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);

                var error = ex.Errors[0];
                Assert.AreEqual("ConvertFailed", error.Type);
                Assert.AreEqual(0, error.RowIndex);
                Assert.AreEqual(2, error.ColumnIndex);
            }
        }

        #endregion
    }
}