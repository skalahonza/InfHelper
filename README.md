# InfHelper
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![NuGet](https://img.shields.io/nuget/v/InfHelper)](https://www.nuget.org/packages/InfHelper/)


* Nuget package for C# that makes it easy to parse .inf files, to store and work with information about system drivers.
* This NuGet Package allows you to parse .inf files and browse them using object model wrappers (using the InfData class). You can browse categories and their keys by handling them as collections or simply using [id] operators. Currently, this package does not support installing drivers from inf files, only parsing and browsing through the file.  The feature for creating an INF file will be added in later versions.

## How to use
### Init parsing
```cs
var helper = new InfUtil();
var data = helper.ParseFile(path);
```
### Getting data
```cs
//obtain category by id
var version = data["Version"];

//get keys from the category
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

### Custom driver info serialization
```cs
public class DriverInfo
    {
        [InfKeyValue("Version","Class")]
        public string Class { get; set; }
        [InfKeyValue("Version", "Provider")]
        public string Provider { get; set; }
    }

    // ....

    var helper = new InfUtil();
    var serilized = helper.SerializeFileInto<DriverInfo>(Path.Combine(testFolder, 
    "oem100.inf" out InfData data);
```
