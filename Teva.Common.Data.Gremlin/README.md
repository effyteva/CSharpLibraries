Effy Teva - C# Libraries

As Gremlin Server will hopefully soon be released,
I've created a C# client for accessing it, using .NET WebSockets.

When Gremlin Server gets an official version, I'll transform my Gremlin Script builder to support the new Gremlin Server libraries, and publish those as well.

Please note this requires .NET Framework 4.5 or higher, and Windows 8/Server 2012 or newer.

Usage sample:

	var Gremlin = new Teva.Common.Data.Gremlin.GremlinClient();

    List<long> Scalar = Gremlin.Send<long>("5 + 6", null);
    Console.WriteLine(Scalar[0]);

    List<Teva.Common.Data.Gremlin.GraphItems.Vertex> Vertices = Gremlin.Send<Teva.Common.Data.Gremlin.GraphItems.Vertex>("g.addVertex('Key', x);", new Dictionary<string, object>
    {
        { "x", "value" }
    });
    Console.WriteLine(Vertices[0].ID);