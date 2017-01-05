using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class TableFactory
    {
        public TableReader<TModel> CreateCsvReader<TModel>(TextReader textReader,
            bool hasHeader = false,
            ITypeConverterResolver typeConverterResolver = null,
            IPropertyMapper propertyMapper = null,
            IModelValidator modelValidator = null)
            where TModel : new()
        {
            var rowReader = new CsvRowReader(textReader);
            typeConverterResolver = typeConverterResolver ?? new DefaultTypeConverterResolver<TModel>();
            propertyMapper = propertyMapper ?? new AutoIndexPropertyMapper();
            modelValidator = modelValidator ?? new NullModelValidator();

            var reader = new TableReader<TModel>(rowReader, typeConverterResolver, propertyMapper, modelValidator);
            reader.HasHeader = hasHeader;

            return reader;
        }

        public TableWriter<TModel> CreateCsvWriter<TModel>(TextWriter textWriter,
            ITypeConverterResolver typeConverterResolver = null,
            IPropertyMapper propertyMapper = null)
        {
            var rowReader = new CsvRowWriter(textWriter);
            typeConverterResolver = typeConverterResolver ?? new DefaultTypeConverterResolver<TModel>();
            propertyMapper = propertyMapper ?? new AutoIndexPropertyMapper();

            return new TableWriter<TModel>(rowReader, typeConverterResolver, propertyMapper);
        }
    }
}
