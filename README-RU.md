# AdoHelper
*Эта страница доступна на следующих языках:*
 - [English ![English](https://i.ibb.co/LRZcgYS/united-kingdom.png)](README.md)
 - [Русский ![Русский](https://i.ibb.co/frNGG0z/russia-1.png)](README-RU.md)
---
[![Build Status](https://travis-ci.org/DevEvolution/AdoHelper.svg?branch=master)](https://travis-ci.org/DevEvolution/AdoHelper) <br/>
**AdoHelper**  – небольшая ORM (_объектно-реляционное отображение_), построенная поверх технологии ADO.NET  и упрощающая ее использование.
![AdoHelper](https://i.ibb.co/j4HDHTX/ADO-Helper.png)
## Файлы
 - [Nuget-package](https://www.nuget.org/packages/DevEvolution.AdoHelper/1.1.0)
 - [Скачать dll-файл](https://yadi.sk/d/uK6gsNHz2Y2mTw)
## Установка
Для установки в проект достаточно скачать nuget пакет, выполнив в Nuget packet manager команду `Install-Package  DevEvolution.AdoHelper` 
или установить зависимости вручную, добавив dll файл в графу зависимостей проекта в VisualStudio.
## Документация
 - [Список изменений](CHANGELOG-RU.md)
 - [Api reference](https://devevolution.github.io/AdoHelper/api/index.html)
## Возможности
Автоматическое отображение результатов запроса в коллекцию экземпляров указанного типа:
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
Отображаемый тип может быть классом, структурой, кортежем (`System.Tuple`) или (`System.ValueTuple`), а также обобщенной  коллекцией (`IEnumerable<>`) или списком (`List<>`). Отображение производится по публичным свойствам, доступным для записи (`set;`) и полям.
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
## Инструкция по применению
Запрос к базе данных (БД) выглядит следующим образом:

```csharp
[var  возвращаемое значение] = new  AdoHelper<тип возвращаемого значения>(объект подключения к  БД)
[.Parameters(параметры)]
[.Transaction(объект транзакции)]
.ExecuteNonQuery() || .ExecuteScalar() || .ExecuteReader()
```
- **`Возвращаемое значение`** – значение, которое будет возвращено в качестве результата выполнения запроса. В зависимости от типа запроса может быть коллекцией объектов, одиночным значением, а также значение может не возвращаться вовсе.
**Важно!** Количество элементов, указанных в *ValueTuple*  или *Tuple* возвращаемого значения, порядок объявления и тип элемента должны совпадать с количеством столбцов и очередностью элементов в результирующей таблице запроса.

- **`Объект подключения к БД`** – обьект типа `IDbConnection` (например, `SqlConnection`).

- **`Параметры`** – коллекция параметров, представляющих собой пару (параметр - значение). Тип параметров – `AdoParameter`. Также можно использовать кортежи (`ValueTuple<string, object>` и `Tuple<string, object>`) и объекты `DbParameter` для задания каждого параметра.

Пример :
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
- **`Объект транзакции`** – объект  типа `IDbTransaction` (например, `SqlTransaction`).

Результат запроса (`возвращаемое значение`) может быть следующим:
- Коллекция строк результирующей таблицы в виде `IEnumerable<T>`. Для получения такого возвращаемого значения необходимо воспользоваться командой `ExecuteReader()`.
- Одиночное значение в виде объекта типа `T`. Чтобы получить одиночное значение, необходимо выполнить завершающую команду `ExecuteScalar()`.
- Не возвращать значение. Для этого существует метод `ExecuteNonQuery()`.

## Сопоставляемые объекты
AdoHelper  умеет самостоятельно сопоставлять имена членов класса/структуры и столбцов результирующей таблицы, причем порядок столбцов не имеет значения. При сопоставлении не учитывается регистр.
Пример:
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
Однако в некоторых ситуациях имеет смысл давать членам класса сопоставления имена, отличающиеся от имен столбцов результирующей таблицы. В таком случае следует использовать атрибуты `[Field(Name = "Имя столбца в таблице")]`. Также бывают случаи, при которых необходимо отказаться от сопоставления некоторых членов класса со столбцами таблицы. Для этого существует атрибут `[NonMapped]`.
Пример  такого  класса:
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
Также в качестве объекта для сопоставления могут использоваться кортежи. Однако в этом случае элементы из которых состоит кортеж должны идти в том же порядке, что и в результирующей таблице. Полностью поддерживаются кортежи любых размеров.

**Примечание.** Имена элементов `ValueTuple`  не участвуют в сопоставлении, так как являются лишь синтаксическим сахаром и не используются в скомпилированном приложении.
```csharp
var entities = new AdoHelper<(int id, string name, int nextId)>(_connection)
                .Query("SELECT current_id, category_name, next_id FROM categories WHERE category LIKE ‘TMP’")
                .ExecuteReader();
```
Эквивалентно
```csharp
var entities = new AdoHelper<Tuple<int, string, int>>(_connection)
                .Query("SELECT current_id, category_name, next_id FROM categories WHERE category LIKE ‘TMP’")
                .ExecuteReader();
```
Для сопоставления можно использовать коллекции `IEnumerable` и `List`, в таком случае результат будет содержать таблицу из элементов типа, указанного в обобщенном параметре коллекции.
```csharp
IEnumerable<IEnumerable<string>> entities = new AdoHelper<IEnumerable<string>>(_connection)
                .Query("SELECT current_id, category_name, next_id FROM categories WHERE category LIKE ‘TMP’")
                .ExecuteReader();
```

## Лицензия
Проект опубликован под лицензией [MIT](LICENSE.md) и поставляется как есть, без каких либо гарантий.
