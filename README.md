# TableIO

TableIO provides common interaface for reading and writing CSV, TSV, Excel and other table format content.

TableIO `~v2.x.x` target framework is *.NET Standard 2.0*.

TableIO `v3.0.0~` target framework is *.NET Standard 2.1*. (beta)

# Nuget

[TableIO](https://www.nuget.org/packages/TableIO/)  
provides core functionality, reading and writing CSV, TSV file.

```
PM> Install-Package TableIO
```

[TableIO.ClosedXml](https://www.nuget.org/packages/TableIO.ClosedXml)  
provides reading and writing Excel(xlsx only) file.  
TableIO.ClosedXml depends on ClosedXml and TableIO.
```
PM> Install-Package TableIO.ClosedXml
```

# How to use

## Read and Write CSV, TSV
```csharp
using TableIO;

class Model
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
}

// CSV
public void CSV()
{
    IList<Model> models = null;

    // read all rows into a array.
    using (var stmReader = new StreamReader("readfile.csv"))
    {
        // parameters is (textReader, hasHeader)
        var tableReader = new TableFactory().CreateCsvReader<Model>(stmReader, true);
        var models = tableReader.Read().ToArray();

        Assert.AreEqual(5, models.Count);
        Assert.AreEqual(1, model[0].Id);
    }

    // read row by row.(yield results)
    using (var stmReader = new StreamReader("readfile.csv"))
    {
        var tableReader = new TableFactory().CreateCsvReader<Model>(stmReader, true);
        foreach (var model in tableReader.Read())
        {
            Console.WriteLine(model.Id);
        }
    }

    // write models to file.
    using (var stmWriter = new StreamWriter("writefile.csv"))
    {
        var tableWriter = new TableFactory().CreateCsvWriter<ValidCsvModel>(stmWriter);
        // parameter is (models, header)
        tableWriter.Write(models, new[] { "ID", "NAME", "PRICE", "REMARKS" });
    }
}

// TSV
public void TSV()
{
    IList<Model> models = null;

    using (var stmReader = new StreamReader("readfile.tsv"))
    {
        var tableReader = new TableFactory().CreateTsvReader<Model>(stmReader, true);
        var models = tableReader.Read().ToArray();

        Assert.AreEqual(5, models.Count);
        Assert.AreEqual(1, model[0].Id);
    }

    using (var stmWriter = new StreamWriter("writefile.tsv"))
    {
        var tableWriter = new TableFactory().CreateTsvWriter<ValidCsvModel>(stmWriter);
        tableWriter.Write(models, new[] { "ID", "NAME", "PRICE", "REMARKS" });
    }
}
```

## Read and Write Excel
```csharp
using TableIO;
using TableIO.ClosedXml;    // nuget TableIO.ClosedXml

class Model
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
}

public void EXCEL()
{
    IList<Model> models = null;

    // read file to models.
    using (var workbook = new XLWorkbook("readfile.xlsx"))
    {
        var worksheet = workbook.Worksheet(1);

        // parameters is (worksheet, startRowNumber, startColumnNumber, columnSize, hasHeader)
        var tableReader = new TableFactory().CreateXlsxReader<Model>(worksheet, 1, 1, 4, true);
        var models = tableReader.Read();

        Assert.AreEqual(5, models.Count);
        Assert.AreEqual(1, model[0].Id);
    }

    // write models to file.
    using (var workbook = new XLWorkbook())
    {
        var worksheet = workbook.Worksheets.Add("sample");

        // parameters is (worksheet, startRowNumber, startColumnNuber)
        var tableWriter = new TableFactory().CreateXlsxWriter<Model>(worksheet, 1, 1);
        // parameter is (models, header)
        tableWriter.Write(models, new[] { "ID", "NAME", "PRICE", "REMARKS" });

        workbook.SaveAs("writefile.xlsx");
    }
}
```

# Design

![Alt Class Diagram](https://raw.githubusercontent.com/nabehiro/TableIO/master/resources/class-diagram.PNG)

TableReader and TableWriter consists of some interfaces that have single work responsiblity.  
We can replace a concrete class that impliment interface with prefer one as you like !

# Class
## TableWriter<Model>
- RowWriter
- RowSerializer
- HasHeader
- ColumnSize
- void Write(IEnumerable<Model> models)

## TableReader<Model>
- RowReader
- RowDeserializer
- ModelValidator
- Errors
- HasHeader
- ColumnSize
- IEnumerable<Model> Read()

## IRowWriter
- TrimOption
- void Write(IList&lt;object&gt; row)

### CsvRowWriter
- TextWriter
- AlwaysEncloseInQuotes

### TsvRowWriter
- TextWriter
- AlwaysEncloseInQuotes

## IRowReader
- TrimOption
- IList&lt;object&gt; Read()

### CsvRowReader
- TextReader

### TsvRowReader
- TextReader

## RowSerializer<Model>
- (private bool ExistsManualMapping = false)
- HeaderNameConnector = "."
- EnableCompositeExpansion
- this Map()
- void Build()
- IList&lt;object&gt; Serialize(Model model)
- IList&lt;string&gt; SerializeHeader()
- Model Deserialize(IList&lt;object&gt; row)

## IModelValidator<Model>
- Validate()

## ValueConverter

## Attributes
- IgnoreAttribute
- IndexAttribute
- FormatAttribute
- HeaderAttribute(name="name", connector = ".")

# Models <=> Table
Models => Model => Member

Table => Row => Cell
