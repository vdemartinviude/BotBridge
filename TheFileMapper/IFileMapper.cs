using System.Text.Json;

namespace TheFileMapper;

public interface IFileMapper
{
    public Task GenerateJsonFile(string Phase, string OutputJsonFilePath);
}