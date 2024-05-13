using System.Text.Json;
using System.Xml;

namespace lesson_9_sem_1;

internal abstract class Program
{
    private static void Main()
    {
        const string fileJson = """
                                {"Current":{"Time":"2023:06:18T20:35:06.722127+04:00","Temperature":29,"Weathercode":1, "Windspeed":2.1, "Winddirection":1},
                                "History":[{"Time":"2023:06:17T20:35:06.77707+04:00","Temperature":29,"Weathercode":2, "Windspeed":2.4, "Winddirection":1},
                                {"Time":"2023:06:16T20:35:06.777081+04:00","Temperature":22,"Weathercode":2, "Windspeed":2.4, "Winddirection":1},
                                {"Time":"2023:06:15T20:35:06.777082+04:00","Temperature":21,"Weathercode":4, "Windspeed":2.2, "Winddirection":1}]}
                                """;
        var jsonDoc = JsonDocument.Parse(fileJson);
        var xmlDoc = new XmlDocument();
        const string rootW = "root";// корень xml
            
        var rootEl = xmlDoc.CreateElement(rootW);
        xmlDoc.AppendChild(rootEl);
        ConvertJsonToXml(jsonDoc.RootElement, rootEl, xmlDoc);
        xmlDoc.Save("out.xml");// выходной файл xml
    }

    private static void ConvertJsonToXml(JsonElement jsonEl, XmlElement parentEl, XmlDocument xmlDoc)
    {
        foreach (var prop in jsonEl.EnumerateObject())
        {
            var propEl = xmlDoc.CreateElement(prop.Name);
            parentEl.AppendChild(propEl);
            switch (prop.Value.ValueKind)
            {
                case JsonValueKind.Object:
                    ConvertJsonToXml(prop.Value, propEl, xmlDoc);
                    break;

                case JsonValueKind.Array:
                    ConvertJsonArrayToXml(prop.Value, propEl, xmlDoc);
                    break;

                case JsonValueKind.String:
                    propEl.InnerText = prop.Value.GetString() ?? string.Empty;
                    break;

                case JsonValueKind.Number:
                    propEl.InnerText = prop.Value.GetRawText();
                    break;

                case JsonValueKind.True:
                case JsonValueKind.False:
                    propEl.InnerText = prop.Value.GetBoolean().ToString();
                    break;

                case JsonValueKind.Null:
                    propEl.InnerText = "null";
                    break;
                case JsonValueKind.Undefined:
                    break;
            }
        }
    }

    static void ConvertJsonArrayToXml(JsonElement jsonArr, XmlElement parentEl, XmlDocument xmlDocument)
    {
        foreach (JsonElement element in jsonArr.EnumerateArray())
        {
            XmlElement arrEl = xmlDocument.CreateElement("item");
            parentEl.AppendChild(arrEl);

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    ConvertJsonToXml(element, arrEl, xmlDocument);
                    break;
                case JsonValueKind.Array:
                    ConvertJsonArrayToXml(element, arrEl, xmlDocument);
                    break;
                case JsonValueKind.String:
                    arrEl.InnerText = element.GetString() ?? string.Empty;
                    break;
                case JsonValueKind.Number:
                    arrEl.InnerText = element.GetRawText();
                    break;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    arrEl.InnerText = element.GetBoolean().ToString();
                    break;
                case JsonValueKind.Null:
                    arrEl.InnerText = "null";
                    break;
            }
        }
    }
}