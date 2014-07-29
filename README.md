# DataSift .NET Client Library

## -- Status: BETA --

This is the official .NET library for accessing Datasift.

## Requirements

This lib

## Installation

Nuget

## Usage - REST API Calls

See the **DataSiftExamples** project for some simple example usage.

```c#
    var client = new DataSiftClient(username, apikey);
    var compiled = client.Compile("interaction.content contains \"music\"");
    Console.WriteLine("Compiled to {0}, DPU = {1}", compiled.Data.hash, compiled.Data.dpu);
```

## Usage - Streaming

See the **DataSiftExamples** project for some simple example usage.

```c#

```

## License

All code contained in this repository is Copyright MediaSift Ltd.

This code is released under the BSD license. Please see the LICENSE file for more details.


## Change Log


