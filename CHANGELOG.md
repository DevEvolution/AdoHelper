# Changelog

 - ## 1.1.0
	 - Added support for tuples of any length
		 Example:
		 ```csharp
		var veryLongValueTupleCollection = new AdoHelper<(int, string, int, TimeSpan, int, int, string, int, int, int, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, int, int, int, int, DateTime, int, int, string, int, TimeSpan, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string, int, int, int, int, DateTime, int, int, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, string, int, int, int, int, int, int, int, DateTime, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, int, int, int, int)>(connection)
		.Query("SELECT * FROM verylong_table")
		.ExecuteReader();
		
		var longValueTupleCollection = new AdoHelper<Tuple<int, int, string, int, DateTime, int, int, Tuple<int, int, string, int, int, string, int, Tuple<int, int, int, string, int, int, int, Tuple<int, string, int, int, string, int, int>>>>>(connection)
		.Query("SELECT * FROM long_table")
		.ExecuteReader();
		```
	 - Added the ability to return the result in the form of collections
		Example:
		```csharp
		IEnumerable<IEnumerable<string>> tableContent = new AdoHelper<IEnumerable<string>>(connection)
		.Query("SELECT * FROM TestTable")
		.ExecuteReader();
		```
	- Unified approach to creating tuples such as `System.ValueTuple` and` System.Tuple`
	- Added auto-generated documentation.

