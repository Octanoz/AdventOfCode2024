namespace Day23;

public static class EasterBunnyAdmin
{
    public static int PartOne(string[] input)
    {
        Network network = new();
        foreach (var line in input)
        {
            var nodes = line.Split('-');
            network.AddNodes(nodes[0], nodes[1]);
        }

        var clusters = network.FindTriangles();
        foreach (var cluster in clusters)
        {
            Console.WriteLine(cluster);
        }

        return clusters.Count;
    }
}
