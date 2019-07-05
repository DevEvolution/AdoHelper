﻿# Список изменений
*Эта страница доступна на следующих языках:*
 - [English ![English](https://i.ibb.co/LRZcgYS/united-kingdom.png)](CHANGELOG.md)
 - [Русский ![Русский](https://i.ibb.co/frNGG0z/russia-1.png)](CHANGELOG-RU.md)

 - ## 1.1.0
	 - Добавлена поддержка кортежей любой длины.
		 Пример:
		 ```csharp
		var veryLongValueTupleCollection = new AdoHelper<(int, string, int, TimeSpan, int, int, string, int, int, int, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, int, int, int, int, DateTime, int, int, string, int, TimeSpan, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string, int, int, int, int, DateTime, int, int, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, string, int, int, int, int, int, int, int, DateTime, int, int, int, int, int, int, int, TimeSpan, int, int, int, int, int, int, int, int, int, int)>(connection)
		.Query("SELECT * FROM verylong_table")
		.ExecuteReader();
		
		var longValueTupleCollection = new AdoHelper<Tuple<int, int, string, int, DateTime, int, int, Tuple<int, int, string, int, int, string, int, Tuple<int, int, int, string, int, int, int, Tuple<int, string, int, int, string, int, int>>>>>(connection)
		.Query("SELECT * FROM long_table")
		.ExecuteReader();
		```
	 - Добавлена возможность возвращать результат в виде коллекций.
		Пример:
		```csharp
		IEnumerable<IEnumerable<string>> tableContent = new AdoHelper<IEnumerable<string>>(connection)
		.Query("SELECT * FROM TestTable")
		.ExecuteReader();
		```
	- Унифицирован подход к созданию кортежей типа `System.ValueTuple` и `System.Tuple`.
	- Добавлена автогенерируемая документация.
