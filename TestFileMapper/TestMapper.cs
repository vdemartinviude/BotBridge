using TheFileMapper;

namespace TestFileMapper;

public class TestMapper
{
    [Fact]
    public void EnsureCanReadSetup()
    {
        IFileMapper fileMapper = new WooCommerceFileMapper(@"D:\Wc");
        fileMapper.GenerateJsonFile("setup", "");
    }
}