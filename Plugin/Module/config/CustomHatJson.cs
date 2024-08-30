using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TheSpaceRoles;

public class repoConfig
{
    [JsonPropertyName("hats")] public List<repoUrlConfig> Hats { get; set; }
}
public class repoUrlConfig
{
    [JsonPropertyName("repository")] public string Repository { get; set; }
    [JsonPropertyName("jsonUrl")] public string jsonUrl { get; set; }
}