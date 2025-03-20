using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace diff.Api.Tests;

public class Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public Tests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    [Fact]
    public async Task Test1()
    {
        string basePath = "../../../data/Test1/";
        string testFile1Path = Path.Join(basePath, "old.gnucash");
        string testFile2Path = Path.Join(basePath, "new.gnucash");
        HttpClient client = _factory.CreateClient();
        using var form = new MultipartFormDataContent();
        var file1Content = new ByteArrayContent(File.ReadAllBytes(testFile1Path));
        file1Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
        form.Add(file1Content, "file1", testFile1Path);

        var file2Content = new ByteArrayContent(File.ReadAllBytes(testFile2Path));
        file2Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
        form.Add(file2Content, "file2", testFile2Path);

        var response = await client.PostAsync("/diff", form);
        response.EnsureSuccessStatusCode();

        // var responseString = await response.Content.ReadAsStringAsync();
    }
}
