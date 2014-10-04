RexReader 0.9
=============

Non-pretty, generated docs [are available](http://baconsoap.github.io/RexReader).

A C# Library for reading REXPaint files into an easy to use format.

Feeling lazy and don't want to build from source? The latest DLL is [here](http://downloads.varnerin.info/github/RexTools/0.6/RexReader.dll). This was compiled on Win7 using .NET 3.0. If you are using Linux I'm gonna assume you can build from source.

Follow [this guide](http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages) to automatically install required nuget packages on build, or manually add the NUnit package/dll yourself.
You can build the RexReader project without NUnit, but the tests require it to run.

See [CHANGELOG.md](https://github.com/BaconSoap/RexReader/blob/master/CHANGELOG.md) for updates.

Usage
=====

The test suite is pretty comprehensive if you want to get a feel of using this. Otherwise, the basic usage is:

```csharp
var reader = new RexReader("path/to/exported/file.xp");
var map = RexReader.GetMap();
```
The TileMap structure is (roughly):

    | TileMap
    | | TileLayer[] Layers
    | | | Tile[,] Tiles
    | | | | byte CharacterCode
    | | | | byte ForegroundRed
    | | | | byte ForegroundGreen
    | | | | byte ForegroundBlue
    | | | | byte BackgroundRed
    | | | | byte BackgroundGreen
    | | | | byte BackgroundBlue

License
=======

This format reader is licensed under the MIT license.

>The MIT License (MIT)

>Copyright (c) 2013 Andrew Varnerin

>Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

>The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

>THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
