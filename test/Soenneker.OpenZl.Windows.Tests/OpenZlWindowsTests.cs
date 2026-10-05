using System;
using System.IO;
using System.Security.Cryptography;
namespace Soenneker.OpenZl.Windows.Tests;

public sealed class OpenZlWindowsTests
{
    [Test]
    public void NativeAssetMatchesProvenanceAndIncludesLicenses()
    {
        string directory = Path.Combine(AppContext.BaseDirectory, "Resources", "win-x64", "native");
        byte[] bytes = File.ReadAllBytes(Path.Combine(directory, "openzl.dll"));
        if (!bytes.AsSpan().StartsWith(new byte[] { 0x4d, 0x5a })) throw new InvalidDataException("Incorrect native binary format.");
        string hash = Convert.ToHexStringLower(SHA256.HashData(bytes));
        string provenance = File.ReadAllText(Path.Combine(directory, "SOURCE.txt"));
        if (!provenance.Contains($"SHA256 (openzl.dll): {hash}", StringComparison.Ordinal)) throw new InvalidDataException("Native binary does not match its provenance.");
        foreach (string license in new[] { "OpenZL.txt", "Zstandard.txt", "LZ4.txt" })
            if (new FileInfo(Path.Combine(directory, "licenses", license)).Length == 0) throw new InvalidDataException("Missing license content.");
    }
}
