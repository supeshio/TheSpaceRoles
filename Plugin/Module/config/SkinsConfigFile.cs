using System.Collections.Generic;
using System.Text.Json.Serialization;
using TheSpaceRoles;

namespace TheSpaceRoles;

public class SkinsConfigFile
{
    [JsonPropertyName("hats")] public List<CustomHat> Hats { get; set; }
}