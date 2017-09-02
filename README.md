# InfHelper
Nuget package for C# that will help parsing inf files storing information about system drivers.

## How to use
### Init parsing
```cs
var helper = new InfUtil();
var data = helper.ParseFile(path);
```
### Getting data
```cs
/obtain category by id
var version = data["Version"];

//extract key from the inf file
var providerKey = version["Provider"];
var classGuidKey = version["ClassGuid"];

//obtain key directely
var key = data["Strings"]["KeyId"];


// get list of vaues separated by comma
var values = key.KeyValues;

//get value
var value = values[0].Value;

if(value.IsDynamic){
//the key value refers to another key
}

//you can also ignore all wrappers and just get the primitive value (string)
var value = key.PrimitiveValue;
```
