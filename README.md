RexReader
=========

A C# Library for reading REXPaint files into an easy to use format.

Feeling lazy and don't want to build from source? The latest DLL is [here](http://downloads.varnerin.info/github/RexTools/0.5/RexReader.dll).

Follow [this guide](http://docs.nuget.org/docs/workflows/using-nuget-without-committing-packages) to automatically install required nuget packages on build, or manually add the NUnit package/dll yourself.
You can build the RexReader project without NUnit, but the tests require it to run.

Usage
=====

The test suite is pretty comprehensive if you want to get a feel of using this. Otherwise, the basic usage is:

    var reader = new RexReader("path/to/exported/file.xp");
    var map = RexReader.GetMap();

The TileMap structure is:

    | TileMap
    | | Layers[]
    | | | Tiles[,]
    | | | | CharacterCode
    | | | | 
    | | | | 
