using SevenZip;
using System.IO;
using System.Text;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class LZMAUtils
{
        public static void TestLZMA()
        {
        byte[] text1 = Encoding.ASCII.GetBytes(new string('X', 10000));
        byte[] compressed_ = LZMAtools.CompressByteArrayToLZMAByteArray(text1);
        byte[] text2 = LZMAtools.DecompressLZMAByteArrayToByteArray(compressed_);


        byte[] text3 = Encoding.ASCII.GetBytes(Samples.longstring);
        byte[] compressed2 = LZMAtools.CompressByteArrayToLZMAByteArray(text3);
        byte[] text4 = LZMAtools.DecompressLZMAByteArrayToByteArray(compressed2);
        
        Debug.Log("text1 size: " + text1.Length);
        Debug.Log("compressed size:" + compressed_.Length);
        Debug.Log("text2 size: " + text2.Length);
        Debug.Log("are equal: " + ByteArraysEqual(text1, text2));


        Debug.Log("text3 size: " + text3.Length);
        Debug.Log("compressed2 size:" + compressed2.Length);
        Debug.Log("text4 size: " + text4.Length);
        Debug.Log("are equal: " + ByteArraysEqual(text3, text4));
        
        WriteValidatedJsonToFile(
            Samples.sample_jsonstring,
            Samples.sample_jsonfilepath
        );

        WriteTextToFileAndCompress(
            Samples.samplestring,
            Samples.samplestring_filepath
        );


        LZMAtools.DecompressLZMAFileToFile(
            Samples.samplestring_filepath_compressed,
            Samples.samplestring_filepath_decompressed);
        

    }

    public static byte[] GetHashSha256(string filename)
    {
        using (SHA256 mySHA256 = SHA256.Create())
        {
            using (FileStream stream = new FileStream(filename,FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.None))
            {
                byte[] hash = mySHA256.ComputeHash(stream);
                stream.Close();
                return hash;
            }
        }
    }

    public static void WriteValidatedJsonToFile(string jsonStr, string jsonFilepath)
    {
        bool written = WriteTextToFile(jsonStr, jsonFilepath);
        if (written)
        {
            string textContent = File.ReadAllText(jsonFilepath);
            if (textContent.Length > 0)
            {
                if (IsValidJson(textContent) == true)
                {
                    Console.WriteLine("Compressing");
                    LZMAtools.CompressFileToLZMAFile(jsonFilepath, jsonFilepath + ".lzma");
                }
            }
        }
    }

    public static void WriteTextToFileAndCompress(string textStr, string filePath)
    {
        bool written = WriteTextToFile(textStr, filePath);
        if (written)
        {
            string textContent = File.ReadAllText(filePath);
            if (textContent.Length > 0)
            {
                LZMAtools.CompressFileToLZMAFile(filePath, filePath + ".lzma");
            }
        }
    }

    public static string BytesToString(byte[] bytes)
    {
        string result = "";
        foreach (byte b in bytes) result += b.ToString("x2");
        return result;
    }

    public static void PrintByteArray(byte[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Console.WriteLine($"{array[i]:X2}");
            if ((i % 4) == 3) Console.WriteLine(" ");
        }
        Console.WriteLine("");
    }

    public static bool IsValidJson(string strInput)
    {
        if (string.IsNullOrWhiteSpace(strInput)) { return false; }
        strInput = strInput.Trim();
        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(strInput);
                return true;
            }
            catch (JsonReaderException jex)
            {
                Console.WriteLine(jex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static bool WriteTextToFile(string text, string filepath)
    {
        string createText = text + Environment.NewLine;
        File.WriteAllText(filepath, createText);
        return true;
    }

    public static bool ByteArraysEqual(byte[] b1, byte[] b2)
    {
        if (b1 == b2)
            return true;
        if (b1 == null || b2 == null)
            return false;
        if (b1.Length != b2.Length)
            return false;
        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i])
                return false;
        }

        return true;
    }

    public static void Decompress(Stream inStream, Stream outStream)
    {
        byte[] properties = new byte[5];
        inStream.Read(properties, 0, 5);
        SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
        decoder.SetDecoderProperties(properties);
        long outSize = 0;
        for (int i = 0; i < 8; i++)
        {
            int v = inStream.ReadByte();
            outSize |= ((long)(byte)v) << (8 * i);
        }
        long compressedSize = inStream.Length - inStream.Position;
        decoder.Code(inStream, outStream, compressedSize, outSize, null);
    }

    public static string DecompressLzma(string inputstring)
    {
        if (!string.IsNullOrEmpty(inputstring))
        {
            byte[] myInts = Array.ConvertAll(inputstring.Split(','), s => (byte)int.Parse(s));
            var stream = new MemoryStream(myInts);
            var outputStream = new MemoryStream();
            Decompress(stream, outputStream);
            using (var reader = new StreamReader(outputStream))
            {
                outputStream.Position = 0;
                string output = reader.ReadToEnd();
                return output;
            }
        }

        return "";
    }


    public byte[] CompressBytesLzma(byte[] inputBytes)
    {
        var stream = new MemoryStream(inputBytes);
        var outputStream = new MemoryStream();
        Compress(stream, outputStream);

        return outputStream.ToArray();
    }

    public static string CompressLzma(string inputstring)
    {
        if (!string.IsNullOrEmpty(inputstring))
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(inputstring ?? ""));
            var outputStream = new MemoryStream();
            Compress(stream, outputStream);


            byte[] bytes = outputStream.ToArray();
            var result = string.Join(",", Array.ConvertAll(bytes, v => signedInt((int)v)));
            return result;
        }

        return "";
    }

    public static void Compress(MemoryStream inStream, MemoryStream outStream)
    {
        CoderPropID[] propIDs;
        object[] properties;
        PrepareEncoder(out propIDs, out properties);
        SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
        encoder.SetCoderProperties(propIDs, properties);
        encoder.WriteCoderProperties(outStream);
        Int64 fileSize = inStream.Length;
        for (int i = 0; i < 8; i++)
        {
            outStream.WriteByte((Byte)(fileSize >> (8 * i)));
        }
        encoder.Code(inStream, outStream, -1, -1, null);
    }

    private static int signedInt(int unsignedInt)
    {
        return unsignedInt >= 128 ? Math.Abs(128 - unsignedInt) - 128 : unsignedInt;
    }

    public static void PrepareEncoder(out CoderPropID[] propIDs, out object[] properties)
    {
        bool eos = true;
        Int32 dictionary = 1 << 16;
        Int32 posStateBits = 2;
        Int32 litContextBits = 3; // for normal files
                                  // UInt32 litContextBits = 0; // for 32-bit data
        Int32 litPosBits = 0;
        // UInt32 litPosBits = 2; // for 32-bit data
        Int32 algorithm = 2;
        Int32 numFastBytes = 64;
        string mf = "bt4";

        propIDs = new CoderPropID[]
        {
       CoderPropID.DictionarySize,
       CoderPropID.PosStateBits,
       CoderPropID.LitContextBits,
       CoderPropID.LitPosBits,
#pragma warning disable CS0436 // Typenkonflikte mit importiertem Typ
       CoderPropID.Algorithm,
#pragma warning restore CS0436 // Typenkonflikte mit importiertem Typ
       CoderPropID.NumFastBytes,
       CoderPropID.MatchFinder,
       CoderPropID.EndMarker
        };
        properties = new object[]
        {
       dictionary,
       posStateBits,
       litContextBits,
       litPosBits,
       algorithm,
       numFastBytes,
       mf,
       eos
        };
    }
}
