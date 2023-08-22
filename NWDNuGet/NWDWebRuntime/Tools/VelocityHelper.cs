using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace NWDWebRuntime.Tools;

public class VelocityHelper {
    
    public static VelocityEngine getVelocityEngine() {
        ExtendedProperties properties = new ExtendedProperties();
        properties.AddProperty("resource.loader", "assembly");
        properties.AddProperty("assembly.resource.loader.class", "NVelocity.Runtime.Resource.Loader.AssemblyResourceLoader, NVelocity");
        properties.AddProperty("assembly.resource.loader.assembly", "NWD3_WhiteApp");
        return new VelocityEngine(properties);
    }

    public static string getStringFromTemplate(string templateName, string dataName, object data) {
        VelocityEngine engine = getVelocityEngine();
        Template template = engine.GetTemplate(templateName);
        VelocityContext context = new VelocityContext();
        context.Put(dataName, data);
        StringWriter strWriter = new StringWriter();
        template.Merge(context, strWriter);
        return strWriter.ToString();
    }

    public static string getStringFromTemplate(string templateName, Dictionary<string, object> dico) {
        VelocityEngine engine = getVelocityEngine();
        Template template = engine.GetTemplate(templateName);
        VelocityContext context = new VelocityContext();
        foreach (string key in dico.Keys) {
            context.Put(key, dico!.GetValueOrDefault(key, null));
        }
        StringWriter strWriter = new StringWriter();
        template.Merge(context, strWriter);
        return strWriter.ToString();
    }
}