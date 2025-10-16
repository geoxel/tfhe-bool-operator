using System;
using System.Diagnostics;
using System.Globalization;

namespace Fhe;

public static class Program
{
    public enum BooleanOperation
    {
        And = 8,      // a & b
        Or = 14,      // a | b
        Xor = 6,      // a ^ b
        Xnor = 9,     // !(a ^ b)
        Nand = 7,     // !(a & b)
        Nor = 1,      // !(a | b)
        AAndNotB = 2, // a & !b
        NotAAndB = 4, // !a & b
    }

    private static FheUInt8 bitop8_fast(FheUInt8 a, FheUInt8 b, FheUInt8 op)
    {
        // Each bit of op defines output for (a_bit, b_bit)
        FheUInt8 mask0 = a & b;              // index 3
        FheUInt8 mask1 = !a & b;             // index 1
        FheUInt8 mask2 = a & !b;             // index 2
        FheUInt8 mask3 = !a & !b;            // index 0

        return
            ((-(op & 1)) & mask3) |
            ((-(op >> 1 & 1)) & mask1) |
            ((-(op >> 2 & 1)) & mask2) |
            ((-(op >> 3 & 1)) & mask0);
    }

    public static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        Console.WriteLine("Generate keys...");
        using var fhe = Fhe.Instance;

        using var a = FheUInt8.Encrypt(0b11001001);
        using var b = FheUInt8.Encrypt(0b10101011);

        string ToBinaryString(int x) => Convert.ToString(x, 2);

        Console.WriteLine("a = 0b" + ToBinaryString(a.Decrypt()));
        Console.WriteLine("b = 0b" + ToBinaryString(b.Decrypt()));

        using FheUInt8 c = bitop8_fast(a, b, FheUInt8.Encrypt((int)BooleanOperation.And));
        Console.WriteLine("a & b = 0b" + ToBinaryString(c.Decrypt()));
        Console.WriteLine("a & b = 0b" + ToBinaryString(a.Decrypt() & b.Decrypt()));

        using FheUInt8 d = bitop8_fast(a, b, FheUInt8.Encrypt((int)BooleanOperation.Or));
        Console.WriteLine("a | b = 0b" + ToBinaryString(d.Decrypt()));
        Console.WriteLine("a | b = 0b" + ToBinaryString(a.Decrypt() | b.Decrypt()));

        using FheUInt8 e = bitop8_fast(a, b, FheUInt8.Encrypt((int)BooleanOperation.Xor));
        Console.WriteLine("a ^ b = 0b" + ToBinaryString(e.Decrypt()));
        Console.WriteLine("a ^ b = 0b" + ToBinaryString(a.Decrypt() ^ b.Decrypt()));
    }
}
