
# Changelog
*This page is available in the following languages:*
 - [English ![English](https://i.ibb.co/LRZcgYS/united-kingdom.png)](CHANGELOG.md)
 - [Русский ![Русский](https://i.ibb.co/frNGG0z/russia-1.png)](CHANGELOG-RU.md)

- ## 1.2.0
	 - Added the ability to return `dynamic`. In this case, `ExpandoObject` is returned with fields that completely match the name of the resulting table.
	 - In case of a matching error, AdoHelper now returns an `AdoHelperException` with a detailed description of the error.
	 - In the course of type comparison, with a comparison error, another attempt is made to compare with an invariant culture.
	 - Now you can make asynchronous requests. Such requests have Async ending.
		 Example:
		 ```csharp
		var entities = await new AdoHelper<dynamic>(_connection)
                .Query("SELECT * FROM TestTable")
                .ExecuteReaderAsync();
		```
	- Access to tuples inside the library was unified. Created a class of extension methods for tuples `TupleAccessExtensions`.
	- The structure and composition of unit tests has been changed.

 - ## 1.1.0
	 - Added support for tuples of any length.
		 Example:
		 ```csharp
		var veryLongValueTupleCollection = new AdoHelper<(int, string, int, TimeSpan, int, int, string, int, int, int, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, int, int, int, int, DateTime, int, int, string, int, TimeSpan, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string, int, int, int, int, DateTime, int, int, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, string, int, int, int, int, int, int, int, DateTime, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, int, int, int, int)>(connection)
		.Query("SELECT * FROM verylong_table")
		.ExecuteReader();
		
		var longValueTupleCollection = new AdoHelper<Tuple<int, int, string, int, DateTime, int, int, Tuple<int, int, string, int, int, string, int, Tuple<int, int, int, string, int, int, int, Tuple<int, string, int, int, string, int, int>>>>>(connection)
		.Query("SELECT * FROM long_table")
		.ExecuteReader();
		```
	 - Added the ability to return the result in the form of collections.
		Example:
		```csharp
		IEnumerable<IEnumerable<string>> tableContent = new AdoHelper<IEnumerable<string>>(connection)
		.Query("SELECT * FROM TestTable")
		.ExecuteReader();
		```
	- Unified approach to creating tuples such as `System.ValueTuple` and` System.Tuple`.
	- Added auto-generated documentation.

