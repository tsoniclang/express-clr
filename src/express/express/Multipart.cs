using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace express;

public sealed class Multipart
{
    private readonly MultipartOptions _options;

    public Multipart(MultipartOptions? options = null)
    {
        _options = options ?? new MultipartOptions();
    }

    public RequestHandler any() => async (req, _, next) =>
    {
        await parse(req, MultipartMode.any, null, next).ConfigureAwait(false);
    };

    public RequestHandler none() => async (req, _, next) =>
    {
        await parse(req, MultipartMode.none, null, next).ConfigureAwait(false);
    };

    public RequestHandler single(string name) => async (req, _, next) =>
    {
        await parse(req, MultipartMode.single, new[] { new MultipartField { name = name, maxCount = 1 } }, next)
            .ConfigureAwait(false);
    };

    public RequestHandler array(string name, int? maxCount = null) => async (req, _, next) =>
    {
        await parse(req, MultipartMode.fields, new[] { new MultipartField { name = name, maxCount = maxCount } }, next)
            .ConfigureAwait(false);
    };

    public RequestHandler fields(MultipartField[] fields) => async (req, _, next) =>
    {
        await parse(req, MultipartMode.fields, fields, next).ConfigureAwait(false);
    };

    private async Task parse(Request req, MultipartMode mode, MultipartField[]? allowList, NextFunction next)
    {
        if (req.context is null)
        {
            await next(null).ConfigureAwait(false);
            return;
        }

        var contentType = req.context.Request.ContentType ?? string.Empty;
        if (string.IsNullOrWhiteSpace(contentType) || !contentType.Contains(_options.type, StringComparison.OrdinalIgnoreCase))
        {
            await next(null).ConfigureAwait(false);
            return;
        }

        var form = await req.context.Request.ReadFormAsync().ConfigureAwait(false);

        req.file = null;
        req.files.Clear();

        var formFields = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var key in form.Keys)
        {
            var values = form[key];
            if (values.Count <= 1)
                formFields[key] = values.ToString();
            else
                formFields[key] = values.ToArray();
        }

        req.body = formFields.Count == 0 ? null : formFields;

        if (form.Files.Count == 0)
        {
            await next(null).ConfigureAwait(false);
            return;
        }

        if (_options.maxFileCount is { } maxFiles && form.Files.Count > maxFiles)
            throw new InvalidOperationException($"Too many files (max: {maxFiles}).");

        var perFieldCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in form.Files)
        {
            if (_options.maxFileSizeBytes is { } maxSize && item.Length > maxSize)
                throw new InvalidOperationException($"File '{item.FileName}' exceeds max size ({maxSize} bytes).");

            var file = new UploadedFile(item);

            if (mode == MultipartMode.none)
                throw new InvalidOperationException("Expected no files for multipart request.");

            if (allowList is not null)
            {
                var allowed = false;
                foreach (var field in allowList)
                {
                    if (!string.Equals(field.name, file.fieldname, StringComparison.OrdinalIgnoreCase))
                        continue;
                    allowed = true;
                    if (field.maxCount is { } maxCount)
                    {
                        perFieldCounts.TryGetValue(field.name, out var current);
                        current++;
                        perFieldCounts[field.name] = current;
                        if (current > maxCount)
                            throw new InvalidOperationException($"Too many files for field '{field.name}' (max: {maxCount}).");
                    }

                    break;
                }

                if (!allowed)
                    throw new InvalidOperationException($"Unexpected multipart field '{file.fieldname}'.");
            }

            req.files.Add(file);
        }

        if (mode == MultipartMode.single && allowList is { Length: 1 })
        {
            var name = allowList[0].name;
            var files = req.files[name];
            if (files is { Length: 1 })
                req.file = files[0];
            else if (files is { Length: > 1 })
                throw new InvalidOperationException($"Too many files for field '{name}' (expected 1).");
        }

        await next(null).ConfigureAwait(false);
    }

    private enum MultipartMode
    {
        any,
        none,
        single,
        fields
    }
}

