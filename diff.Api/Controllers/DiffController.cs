using Microsoft.AspNetCore.Mvc;

namespace diff.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DiffController : ControllerBase
{

    [HttpPost()]
    public string Post(IFormFile file1, IFormFile file2)
    {
        if (file1 == null || file2 == null)
        {
            return "Both files must be provided.";
        }

        return $"Received {file1.FileName} and {file2.FileName}";
    }
}
