# RexReader [![Build status](https://ci.appveyor.com/api/projects/status/m2wv5c3v5bgr47vb?svg=true)](https://ci.appveyor.com/project/BaconSoap/rexreader) [![NuGet Version](https://img.shields.io/nuget/v/RexReader.svg)](https://www.nuget.org/packages/RexReader/)

Non-pretty, generated docs [are available](http://baconsoap.github.io/RexReader).

A lightweight .NET Library for reading REXPaint files into an easy to use format.

Install from the NuGet Manager in Visual Studio or from the package manager console using `Install-Package RexReader`.

You can build the RexReader project without NUnit, but the tests require it to run.

See [CHANGELOG.md](https://github.com/BaconSoap/RexReader/blob/master/CHANGELOG.md) for updates.

## Usage

The test suite is comprehensive if you want to get a feel of using this. Otherwise, the basic usage is:

```csharp
var reader = new RexReader("path/to/exported/file.xp");
var map = RexReader.GetMap();
```

The TileMap structure is:

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

## License

This format reader is licensed under the MIT license.

>The MIT License (MIT)

>Copyright (c) 2015 Andrew Varnerin

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
