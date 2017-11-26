﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TableIO.PropertyMappers;
using TableIO.RowWriters;
using TableIO.TypeConverters;

namespace TableIO.Tests
{
    [TestClass()]
    public class TableWriterTests
    {
        class Model
        {
            public int PInt { get; set; }
            public string PString { get; set; }
            public int? PNInt { get; set; }
        }

        class TypeConvertTestModel
        {
            public int PInt { get; set; }

            public decimal PDecimal { get; set; }

            public DateTime PDateTime { get; set; }
        }

        private TableWriter<TModel> CreateTableWriter<TModel>(StringWriter strWriter,
            IRowWriter rowWriter = null, ITypeConverterResolver typeConvResolver = null,
            IPropertyMapper mapper = null)
        {
            rowWriter = rowWriter ?? new CsvRowWriter(strWriter);
            typeConvResolver = typeConvResolver ?? new DefaultTypeConverterResolver<TModel>();
            mapper = mapper ?? new AutoIndexPropertyMapper();

            var tableWriter = new TableWriter<TModel>(rowWriter, typeConvResolver, mapper);
            return tableWriter;
        }

        [TestMethod]
        public void ConvertToRow()
        {
            var strWriter = new StringWriter();
            var writer = CreateTableWriter<Model>(strWriter);
            var maps = new AutoIndexPropertyMapper().CreatePropertyMaps(typeof(Model), null);

            var row = writer.ConvertToRow(new Model { PInt = 1, PString = "aaa", PNInt = 10 }, maps, 3);

            Assert.AreEqual(3, row.Count);
            Assert.AreEqual(1, row[0]);
            Assert.AreEqual("aaa", row[1]);
            Assert.AreEqual(10, row[2]);

            row = writer.ConvertToRow(new Model { PInt = 1, PString = null, PNInt = null }, maps, 4);
            Assert.AreEqual(4, row.Count);
            Assert.AreEqual(1, row[0]);
            Assert.AreEqual(null, row[1]);
            Assert.AreEqual(null, row[2]);
        }

        [TestMethod()]
        public void Write()
        {
            var strWriter = new StringWriter();
            var writer = CreateTableWriter<Model>(strWriter);

            writer.Write(new[]
            {
                new Model { PInt = 1, PString = "aaa", PNInt = 10 },
                new Model { PInt = 2, PString = "bbb", PNInt = 20 },
            });

            Assert.AreEqual("1,aaa,10\r\n2,bbb,20\r\n", strWriter.ToString());
        }

        [TestMethod()]
        public void Write_HasHeader()
        {
            var strWriter = new StringWriter();
            var writer = CreateTableWriter<Model>(strWriter);

            writer.Write(new[]
            {
                new Model { PInt = 1, PString = "aaa", PNInt = 10 },
                new Model { PInt = 2, PString = "bbb", PNInt = 20 },
            }, new[] { "h1", "h2", "h3" });

            Assert.AreEqual("h1,h2,h3\r\n1,aaa,10\r\n2,bbb,20\r\n", strWriter.ToString());
        }

        [TestMethod()]
        public void WriteFailed_InvalidColumnSize()
        {
            var strWriter = new StringWriter();
            var writer = CreateTableWriter<Model>(strWriter);
            writer.ColumnSize = 2;

            try
            {
                writer.Write(new[] { new Model { PInt = 1, PString = "aaa", PNInt = 10 } });
                Assert.Fail();
            }
            catch(TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);
                Assert.AreEqual("InvalidColumnSize", ex.Errors[0].Type);
            }
        }

        [TestMethod()]
        public void WriteFailed_InvalidTableHeader()
        {
            var strWriter = new StringWriter();
            var writer = CreateTableWriter<Model>(strWriter);

            try
            {
                writer.Write(new[] { new Model { PInt = 1, PString = "aaa", PNInt = 10 } },
                    new[] { "h1", "h2" });
                Assert.Fail();
            }
            catch (TableIOException ex)
            {
                Assert.AreEqual(1, ex.Errors.Count);
                Assert.AreEqual("InvalidTableHeader", ex.Errors[0].Type);
            }
        }

        [TestMethod]
        public void Write_CustomTypeConvert()
        {
            var strWriter = new StringWriter();
            var mapper = new ManualIndexPropertyMapper<TypeConvertTestModel>();
            mapper
                .Map(x=> x.PInt, 0)
                .Map(x=> x.PDecimal, 1, new FuncTypeConverter(){ ConvertToFieldFunc = new Func<object, object>(x=> ((decimal)x).ToString("N5"))})
                .Map(x => x.PDateTime, 2, new DateTimeConverter("yyyy-MM-dd"))
                ;
            var writer = CreateTableWriter<TypeConvertTestModel>(strWriter, mapper:mapper);
            writer.Write(new[]
{
                    new TypeConvertTestModel() {PInt = 100, PDateTime = new DateTime(2000, 1, 1), PDecimal = (decimal)11001.12345}
                });
            var result = strWriter.ToString();
            Assert.AreEqual("100,\"11,001.12345\",2000-01-01\r\n", result);
        }
    }
}