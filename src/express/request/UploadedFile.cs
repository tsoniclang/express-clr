using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Tsonic.JSRuntime;

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

    internal Stream openReadStream() => _file.OpenReadStream();

    internal Task copyToAsync(Stream target) => _file.CopyToAsync(target);

    public async Task<Uint8Array> bytes()
    {
        await using var stream = new MemoryStream();
        await _file.CopyToAsync(stream).ConfigureAwait(false);
        return new Uint8Array(stream.ToArray());
    }

    public async Task<string> text()
    {
        var data = await bytes().ConfigureAwait(false);
        return Encoding.UTF8.GetString(data.ToArray());
    }

    public async Task save(string path)
    {
        await using var stream = File.Create(path);
        await _file.CopyToAsync(stream).ConfigureAwait(false);
    }
}
