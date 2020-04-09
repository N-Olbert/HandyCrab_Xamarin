using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HandyCrab.Business
{
    public class AssemblyConfig<TAssemblyType>
    {
        private static readonly XElement Config = GetConfigurationElement();

        public static string GetValue(string settingsName)
        {
            return Config?.Element(settingsName)?.Value;
        }

        private static XElement GetConfigurationElement()
        {
            var x = typeof(TAssemblyType).Assembly.GetName().Name;
            using (var stream = typeof(TAssemblyType).Assembly.GetManifestResourceStream(x + ".App.config"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    var doc = XDocument.Parse(content);

                    return doc.Element("config");
                }
            }
        }
    }
}