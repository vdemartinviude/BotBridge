// See https://aka.ms/new-console-template for more information
using ConsoleApp2;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;

var assembly = Assembly.GetExecutingAssembly();

var Guards = assembly.GetExportedTypes().Where(x => x.IsAssignableTo(typeof(ITeste)) && x.IsInterface == false);
List<ITeste> testes = new();
foreach (var teste in Guards)
{
    testes.Add((ITeste)Activator.CreateInstance(teste));
}

JsonObject _doc = new JsonObject();
_doc.Add("Messages", new JsonArray());

JsonObject JsonMsg1 = new JsonObject();
JsonMsg1.Add("topic", "Erro no calculo");
JsonMsg1.Add("message", "Descrição completa do erro!");
JsonArray Messages = _doc["Messages"].AsArray();
Messages.Add(JsonMsg1);

var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement),
    WriteIndented = true
};
Console.WriteLine(JsonSerializer.Serialize(_doc, options));

Console.WriteLine();
JsonArray msgs2 = _doc["Messages"].AsArray();

JsonObject JsonMsg2 = new JsonObject();
JsonMsg2.Add("topic", "Mais um erro no calculo");
JsonMsg2.Add("message", "Mais uma descrição!");
msgs2.Add(JsonMsg2);

Console.WriteLine(JsonSerializer.Serialize(_doc, options));

Console.WriteLine("Hello, World!");