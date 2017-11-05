# FastConfig
FastConfig - C# init file parser.

## Hot to use

You should add **ConstDescribe** and **FastConfig** file in your programm. ConstDescribe file needs to define your init file variables. 

ConstDescribe file can be like this:
``` c#
namespace FastConfigLibrary
{
	public partial class FastConfig
	{
		public string stringVariable { get; set; }
		public bool boolVariable{ get; set; }
		public double doubleVariable { get; set; }
		public long londVariable { get; set; }
	}
}
```
## Init file example
```
[User section x]
stringVariable="Rule 34"
boolVariable=false
[User section x2]
doubleVariable=-6.345
londVariable=-55443
```

## Use example
```c#
try
{
  FastConfig config = new FastConfig("test.init");

  Console.WriteLine(config.stringVariable);
  Console.WriteLine(config.doubleVariable);
  Console.WriteLine(config.londVariable);
  Console.WriteLine(config.boolVariable);
}
catch(Exception e)
{
  Console.WriteLine(e.ToString());
}
 ```
# Supported values

| Suported value  | Define example |
| ------------- | ------------- |
| boolean  | true OR false  |
| string  | "string value"  |
| double  | 2.543  OR 2. OR 2 OR 2,0 |
| int  | 34  |

