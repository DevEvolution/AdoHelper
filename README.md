
# AdoHelper
*This page is available in the following languages:*
 - [English ![English](https://i.ibb.co/LRZcgYS/united-kingdom.png)](README.md)
 - [Русский ![Русский](https://i.ibb.co/frNGG0z/russia-1.png)](README-RU.md)
---
[![Build Status](https://travis-ci.org/DevEvolution/AdoHelper.svg?branch=master)](https://travis-ci.org/DevEvolution/AdoHelper) <br/>
**AdoHelper**  – a small ORM (_objective-relational mapping_), built on top of ADO.NET technology and making its use smarter.
![AdoHelper](https://i.ibb.co/j4HDHTX/ADO-Helper.png)
## Download links
 - [Nuget-package](https://www.nuget.org/packages/DevEvolution.AdoHelper/1.1.0)
 - [Dowload dll file](https://yadi.sk/d/uK6gsNHz2Y2mTw)
## Installation
To install a project, simply download the nuget package by running the `Install-Package DevEvolution.AdoHelper` command in the Nuget packet manager or install dependencies manually by adding the dll file in the project dependency column in VisualStudio.
## Documentation
 - [Changelog](CHANGELOG.md)
 - [Api reference](https://devevolution.github.io/AdoHelper/docs/api/index.html)
## Features
Automatic mapping query results to a collection of instances of the specified type:
```csharp
public  class  SimpleTestEntity
    {
    public  long Id { get; set; }
    public  string TextField { get; set; }
    public  double FloatField { get; set; }
    public  decimal NumericField { get; set; }
    public  long IntegerField { get; set; }
    }
IEnumerable<SimpleTestEntity> entities = new AdoHelper<SimpleTestEntity>(_connection)
	.Query("SELECT * FROM TestTable")
	.ExecuteReader();
``` 
The displayed type can be a class, structure, tuple (`System.Tuple`) or (`System.ValueTuple`) as well as a generic collection (`IEnumerable<>`) or a list (`List<>`). Mapping is made on public properties, available for writing (`set;`) and public fields.
```csharp
public class ClassEntity 
{
     public int Id {get; set;}
     public string text;
}
public struct StructEntity 
{
     public int id;
     public string Text {get; set;}
}

_connection.Open();
var classEntity = new AdoHelper<ClassEntity>(_connection)
     .Query("SELECT * FROM TestTable WHERE id=@id")
     .Parameters((“@id”, 1))
     .ExecuteReader().First();

var structEntity = new AdoHelper<StructEntity>(_connection)
     .Query("SELECT * FROM TestTable WHERE id=@id")
     .Parameters((“@id”, 1))
     .ExecuteReader().First();

var valueTupleEntity = new AdoHelper<(int id, string text)>(_connection)
     .Query("SELECT id, text FROM TestTable WHERE id=@id")
     .Parameters((“@id”, 1))
     .ExecuteReader().First();

var tupleEntity = new AdoHelper<Tuple<int, string>>(_connection)
     .Query("SELECT id, text FROM TestTable WHERE id=@id")
     .Parameters((“@id”, 1))
     .ExecuteReader().First();

var enumerableEntity = new AdoHelper<IEnumerable<string>>(_connection)
     .Query("SELECT id, text FROM TestTable WHERE id=@id")
     .Parameters((“@id”, 1))
     .ExecuteReader().First();

Assert.AreEqual(classEntity.text, structEntity.Text);
Assert.AreEqual(structEntity.Text, valueTupleEntity.text);
Assert.AreEqual(valueTupleEntity.text, tupleEntity.Item2);
```
## Usage instruction
The query to the database (DB) is as follows:
```csharp
[var  Return value] = new  AdoHelper<Return value type>(DB connection object)
[.Parameters(Params)]
[.Transaction(Transaction object)]
.ExecuteNonQuery() || .ExecuteScalar() || .ExecuteReader()
```

- **`Return value`** is the value that will be returned as a result of the query. Depending on the type of request, it can be a collection of objects, a single value, and the value may not be returned at all.
**Important!** The number of elements specified in `ValueTuple` or `Tuple` of the `return value`, the order of declaration and the type of element must match the number of columns and the order of elements in the resulting query table.

- **`DB connection object`** is an object of type IDbConnection (for example, SqlConnection).

- **`Params`** - a collection of parameters that represent a pair (parameter - value). The default type of parameters is `AdoParameter`. You can also use tuples (`ValueTuple<string, object>` and `Tuple<string, object>`) and `DbParameter` objects to specify each parameter.

Example code :
```csharp
var entity = new AdoHelper<SimpleTestEntity>(_connection)
                .Query("SELECT * FROM TestTable WHERE IntegerField = @intParam AND TextField = @textParam" +
                "AND FloatField = @floatParam")
                .Parameters(
                new AdoParameter("@intParam", 123), // AdoHelper param
                ("@textParam", "Hello"), // ValueTuple param
                new Tuple<string, object>("@floatParam", 123.123f)) // Tuple param
                .ExecuteReader()
                .FirstOrDefault();
```
- **`Transaction object`** is an object of type `IDbTransaction` (for example, `SqlTransaction`).

The result of the query (`return value`) may be as follows:
- A collection of rows of the resulting table in the form `IEnumerable<T>`. To get this return value, you must use the `ExecuteReader()` command.
- A single value in the form of object of type `T`. To get a single value, you must run the final command `ExecuteScalar()`.
- Do not return value. For this, there is the `ExecuteNonQuery()` method.

## Mapped objects
AdoHelper can independently match the names of class / structure members and columns of the resulting table, and the order of the columns does not matter. The comparison is not case sensitive.
Example:
```csharp
public struct TestEntity
    {
        public int id { get; set; }
        public DateTime DeliverDate;
        public double longitude { get; set; }
        public double LATITUDE { get; set; }
        public int customerId;
    }
```
However, in some situations it makes sense to give the members of the mapping class names that differ from the column names of the resulting table. In this case, you should use the attributes `[Field(Name="Name of the column in the table")]`. There are also cases in which it is necessary to abandon the mapping of some class members with table columns. For this there is an attribute `[NonMapped]`.
An example of such a class:
```csharp
public class ExcludedFieldTestEntity
    {
        public int Id { get; set; }

        [NonMapped]
        public string TextField { get; set; } // that property is excluded from mapping

        [NonMapped]
        public double FloatField { get; set; } // that property is excluded from mapping
		
		public DateTime DateField { get; } // that property is excluded too because set property is unreachable

        [Field(Name = "NumericField")]
        public decimal Numeric { get; set; } // that property is mapped to NumericField column

        [Field(Name = "IntegerField")]
        public long Integer { get; set; } // that property is mapped to IntegerField column
    }
```
Tuples can also be used as an object for matching. However, in this case, the elements of which the tuple consists must go in the same order as in the resulting table. Tuples of any size are fully supported.

**Note.** The names of the `ValueTuple` elements do not participate in the comparison, as they are only syntactic sugar and are not used in the compiled application.
```csharp
var entities = new AdoHelper<(int id, string name, int nextId)>(_connection)
                .Query("SELECT current_id, category_name, next_id FROM categories WHERE category LIKE ‘TMP’")
                .ExecuteReader();
```
Equals to:
```csharp
var entities = new AdoHelper<Tuple<int, string, int>>(_connection)
                .Query("SELECT current_id, category_name, next_id FROM categories WHERE category LIKE ‘TMP’")
                .ExecuteReader();
```
## License
The project is published under the license [MIT](LICENSE.md) and is supplied as is, without any guarantees.

