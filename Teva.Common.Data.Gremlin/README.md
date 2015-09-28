Effy Teva - C# Libraries

As Gremlin Server will hopefully soon be released,
I've created a C# client for accessing it, using .NET WebSockets.

Please note this requires .NET Framework (currency compiled for 4.6)

There are three primary classes:
* GremlinClient - Uses both GremlinScript and GremlinServerClient, to provide an easy to use library on Gremlin
* GremlinScript - Gremlin script builder library, can be used to access graph
* GremlinServerClient - WebSockets client

Usage sample:

	var Gremlin = new Teva.Common.Data.Gremlin.GremlinClient("localhost", 8184);

	var Script = new Teva.Common.Data.Gremlin.GremlinScript();
	Script.Append_GetVerticesByIndex("Name", "Effy").Out("Knows")

	var Vertices = Gremlin.GetVertices(Script);
	foreach (var Vertice in Vertices)
	{
		Console.WriteLine(Vertices[0].ID);
	}