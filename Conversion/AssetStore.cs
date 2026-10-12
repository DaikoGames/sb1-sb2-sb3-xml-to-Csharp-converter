using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using SkiaSharp;

namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>
/// Schreibt Kostüme/Sounds einmalig ins Assets-Verzeichnis. Identische Daten (SHA-256) werden
/// nicht ein zweites Mal geschrieben, es wird der vorhandene Dateiname zurückgegeben.
/// </summary>
public sealed class AssetStore
{
    readonly string dir;
    readonly Dictionary<string, string> byHash = new();
    readonly HashSet<string> usedNames = new(StringComparer.OrdinalIgnoreCase);

    public int Written { get; private set; }
    public int Reused { get; private set; }

    public AssetStore(string assetsDir)
    {
        dir = assetsDir;
        Directory.CreateDirectory(dir);
    }

    /// <summary>Gibt den Dateinamen (ohne Pfad) zurück oder null, wenn die Daten ungültig sind.</summary>
    public string? Save(string dataUri, string baseName)
    {
        if (!TryParse(dataUri, out string mime, out byte[] bytes)) return null;

        string ext = mime switch
        {
            "image/png" => ".png",
            "image/jpeg" or "image/jpg" => ".jpg",
            "image/gif" => ".gif",
            "image/svg+xml" => ".png",               // wird unten gerastert
            "audio/mpeg" or "audio/mp3" => ".mp3",
            "audio/ogg" => ".ogg",
            _ when mime.StartsWith("audio/") => ".wav",
            _ => ".bin"
        };

        if (mime == "image/svg+xml")
        {
            var png = RasterizeSvg(bytes);
            if (png == null) return null;
            bytes = png;
        }

        string hash = Convert.ToHexString(SHA256.HashData(bytes)) + ext;
        if (byHash.TryGetValue(hash, out var existing)) { Reused++; return existing; }

        string stem = NameRegistry.Sanitize(baseName);
        string name = stem + ext;
        for (int i = 2; !usedNames.Add(name); i++) name = stem + "_" + i + ext;

        File.WriteAllBytes(Path.Combine(dir, name), bytes);
        byHash[hash] = name;
        Written++;
        return name;
    }

    static bool TryParse(string dataUri, out string mime, out byte[] bytes)
    {
        mime = ""; bytes = Array.Empty<byte>();
        if (string.IsNullOrEmpty(dataUri) || !dataUri.StartsWith("data:")) return false;
        int comma = dataUri.IndexOf(',');
        if (comma < 0) return false;
        string header = dataUri.Substring(5, comma - 5);          // z.B. image/png;base64
        mime = header.Split(';')[0].ToLowerInvariant();
        try { bytes = Convert.FromBase64String(dataUri.Substring(comma + 1).Trim()); return bytes.Length > 0; }
        catch (FormatException) { return false; }
    }

    // Synchron (kein Task.Run + Löschen während gelesen wird, wie im alten Code).
    static byte[]? RasterizeSvg(byte[] svgBytes)
    {
        string tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".svg");
        File.WriteAllBytes(tmp, svgBytes);
        try
        {
            using var svg = new Svg.Skia.SKSvg();
            var picture = svg.Load(tmp);
            if (picture == null) return null;

            int w = (int)Math.Ceiling(picture.CullRect.Width);
            int h = (int)Math.Ceiling(picture.CullRect.Height);
            if (w <= 0) w = 480;
            if (h <= 0) h = 360;

            using var bitmap = new SKBitmap(w, h);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(picture);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine("SVG konnte nicht gerastert werden: " + ex.Message);
            return null;
        }
        finally
        {
            try { File.Delete(tmp); } catch { /* egal */ }
        }
    }
}
