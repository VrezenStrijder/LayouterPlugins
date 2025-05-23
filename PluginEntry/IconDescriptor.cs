using System.Text.Json;

namespace PluginEntry
{
    public class IconDescriptor
    {
        private Dictionary<string, string> iconPaths = new Dictionary<string, string>();

        public IReadOnlyDictionary<string, string> IconPaths => iconPaths;

        public static IconDescriptor FromJson(string json)
        {
            var descriptor = new IconDescriptor();
            descriptor.iconPaths = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return descriptor;
        }
    }

}
