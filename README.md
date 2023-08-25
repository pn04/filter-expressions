# Filter.Expressions
This project contains the source code for the Filter.Expressions library.

## What is Filter.Expressions?
Filter.Expressions is a library that allows you to create and evaluate expressions to filter data. It creates `System.Linq.Expressions` from a string and uses them to filter data.

## How to use Filter.Expressions?
Here is an example to get you started:
```C#
using Filter.Expressions;
var person = new Person { Name = "John" AND Age = 20 }.AsQueryable()
var result = person.Where("Name = \"John\" AND Age = \"20\"");
```

## How to build Filter.Expressions?
The project is written in C# and can be built using Visual Studio 2017 or newer.

## How to report an issue?
If you want to report an issue, please use the [Issues](https://github.com/pn04/filter-expressions/issues)


