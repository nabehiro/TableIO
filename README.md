# TableIO

TableIO provides common interaface for reading and writing CSV, Excel and other table format content.  

# Nuget

[TableIO](https://www.nuget.org/packages/TableIO/)  
provides core functionality, reading and writing CSV file.

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

## Read and Write CSV
```csharp
using TableIO;

class Model
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Remarks { get; set; }
}

public void Main()
{
    IList<Model> models = null;

    // read file to models.
    using (var stmReader = new StreamReader("readfile.csv"))
    {
        // parameters is (textReader, hasHeader)
        var tableReader = new TableFactory().CreateCsvReader<Model>(stmReader, true);
        var models = tableReader.Read();

        Assert.AreEqual(5, models.Count);
        Assert.AreEqual(1, model[0].Id);
    }

    // write models to file.
    using (var stmWriter = new StreamWriter("writefile.csv"))
    {
        var tableWriter = new TableFactory().CreateCsvWriter<ValidCsvModel>(stmWriter);
        // parameter is (models, header)
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

public void Main()
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

## IRowReader
IRowReader reads table row by row.

## IRowWriter
IRowWriter writes row to table.

## IPropertyMapper
IPropertyMapper maps class property to table column index.
### AutoIndexPropertyMapper (Default)
AutoIndexPropertyMapper mapping rule is that column index is class property definition order.
```csharp
class Model
{
    public int Id  { get; set; }            // assign to 0 column index.
    public string Name { get; set; }        // assign to 1 column index.
    public decimal Price { get; set; }      // assign to 2 column index.
    public string Remarks { get; set; }     // assign to 3 column index.
}

...

var mapper = new AutoIndexPropertyMapper();
var tableReader = new TableFactory().CreateCsvReader<Model>(textReader, propertyMapper:mapper);
```

## IModelValidator
IModelValidator validates model that is generated from table row while reading table.
### NullModelValidator (Default)
NullModelValidator doesn't validate.
### DefaultModelValidator
DefaultModelValidator validates model using DataAnnotations attribute and System.ComponentModel.DataAnnotations.Validator.
```csharp
var validator = new DefaultModelValidator();
var tableReader = new TableFactory().CreateCsvReader<Model>(textReader, modelValidator:validator);
```

## ITypeConverter
ITypeConverter convert from table field to class property or convert from class property to table field.

## ITypeConverterResolver
ITypeConverterResolver provide appropriate ITypeConverter for class property.


