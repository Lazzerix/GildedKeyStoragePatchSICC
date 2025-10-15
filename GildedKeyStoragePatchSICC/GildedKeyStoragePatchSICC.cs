using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using Range = SemanticVersioning.Range;

namespace GildedKeyStoragePatchSICC;


public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "lazzerix.GildedKeyStoragePatchSICC";
    public override string Name { get; init; } = "Allow Gilded Key Storage In SICC";
    public override string Author { get; init; } = "Lazzerix";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; } =
        new Dictionary<string, Range>() { { "xyz.drakia.gildedkeystorage", new Range("~2.0.0") } };
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; }
    public override string? License { get; init; } = "GNU APGLv3";
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 100)]
public class ItemPatcher(
    DatabaseServer databaseServer) // We inject a logger for use inside our class, it must have the class inside the diamond <> brackets
    : IOnLoad // Implement the IOnLoad interface so that this mod can do something on server load
{
    public Task OnLoad()
    {
        HashSet<MongoId> initialFilter = databaseServer.GetTables().Templates.Items["5d235bb686f77443f4331278"].Properties.Grids.ToArray()[0].Properties//gets SICC pouch
            .Filters.ToArray()[0].Filter;
        //adds items to filter allowing them to be stored in the SICC pouch
        initialFilter.Add("661cb372e5eb56290da76c3e");//Golden Keychain Mk. I
        initialFilter.Add("661cb3743bf00d3d145518b3");//Golden Keychain Mk. II
        initialFilter.Add("661cb376b16226f648eb0cdc");//Golden Keychain Mk. III
        initialFilter.Add("661cb36f5441dc730e28bcb0");//Golden Keycard Holder
        initialFilter.Add("661cb36922c9e10dc2d9514b");//Golden Key Box
        
        // Inform the server our mod has finished doing work
        return Task.CompletedTask;
    }
}
