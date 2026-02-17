using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace express;

public sealed class UploadedFile
{
    private readonly IFormFile _file;

    internal UploadedFile(IFormFile file)
    {
        _file = file;
        fieldname = file.Name;
        originalname = file.FileName;
        mimetype = file.ContentType;
        size = file.Length;
    }

    public string fieldname { get; }
    public string originalname { get; }
    public string mimetype { get; }
    public long size { get; }

    public Stream openReadStream() => _file.OpenReadStream();

    public Task copyToAsync(Stream target) => _file.CopyToAsync(target);

    public async Task save(string path)
    {
        await using var stream = File.Create(path);
        await _file.CopyToAsync(stream).ConfigureAwait(false);
    }
}

