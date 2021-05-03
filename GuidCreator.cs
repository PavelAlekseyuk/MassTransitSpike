using System;
using System.Security.Cryptography;
using System.Text;

namespace MassTransitSpike
{
  public static class GuidCreator
  {
    public static Guid CreateGuidForString(string s)
    {
      var sha256hash = ComputeSha256Hash(s);
      var guidBytes = SplitAndXorSha256Hash(sha256hash);
      return new Guid(guidBytes);
    }

    private static byte[] ComputeSha256Hash(string s)
    {
      using var sha256 = SHA256.Create();
      return sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
    }

    /// <summary>
    /// Splits and performs a XOR operation on two arrays
    /// </summary>
    /// <param name="sha256hash">Array containing SHA256 hash value</param>
    /// <returns></returns>
    private static ReadOnlySpan<byte> SplitAndXorSha256Hash(Span<byte> sha256hash)
    {
      var leftSpan = sha256hash.Slice(0, 16);                 
      var rightSpan = sha256hash.Slice(16, 16);
      for (int i = 0; i < rightSpan.Length; i++)
      {
        leftSpan[i] ^= rightSpan[i];
      }

      return leftSpan; 
    }
  }
}
