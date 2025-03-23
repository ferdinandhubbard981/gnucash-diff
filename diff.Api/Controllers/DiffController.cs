using Microsoft.AspNetCore.Mvc;

namespace diff.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DiffController : ControllerBase
{
    private static string tempFileSavePath = "temp-gnc-files/";
    private static string SaveIFormFile(IFormFile file)
    {
        // check that tempFileSavePath exists, create it if it does not.
        System.IO.Directory.CreateDirectory(tempFileSavePath);
        string uniqueFileName = Guid.NewGuid().ToString();
        var savePath = Path.Join(tempFileSavePath, uniqueFileName);
        // check for name collisions (unlikely)
        while (System.IO.File.Exists(savePath))
        {
            uniqueFileName = Guid.NewGuid().ToString();
            savePath = Path.Join(tempFileSavePath, uniqueFileName);
        }
        var fileStream = System.IO.File.Create(savePath); 
        file.CopyTo(fileStream);
        fileStream.Close();
        return savePath.ToString();
    }

    [HttpPost()]
    // TODO: make async
    public ActionResult<string> Post(IFormFile file1, IFormFile file2)
    {
        if (file1 == null || file2 == null)
        {
            return "Both files must be provided.";
        }
        string file1Path = SaveIFormFile(file1);
        string file2Path = SaveIFormFile(file2);
        var diff = GNCDiff.Diff.FromBooks(file1Path, file2Path);
        System.IO.File.Delete(file1Path);
        System.IO.File.Delete(file2Path);
        return diff.ToDiffString();
    }
}
