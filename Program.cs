using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AsciiArt
{
    public class Library
    {
        static public int Max => maxCharacters;
        static public List<string> infillChars => charInfills;
        static List<string> charInfills = new List<string>([
            "░░",
            "▒▒",
            "██"
            ]);
        static int maxCharacters = 26;
    }

    class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string finalOutput = "";
            string copyOutput = "";

            // Calculate Thresholds for Pixel to ASCII
            int multiplier = (int)Math.Floor(255.0f / Library.infillChars.Count);
            List<int> thresholds = new List<int>();
            for (int i = 0; i < Library.infillChars.Count; i++)
            {
                thresholds.Add(i * multiplier);
            }
            thresholds.Add(255);

            // Create Efficient Pixel Lookup System for SPEEEEEEEEEEEEEEEEEEEEEEEEED
            int onlyFansTierSub = 0;
            List<string> pixelLookup = new List<string>();
            for (int i = 0; i < 256; i++)
            {
                if (i >= thresholds[onlyFansTierSub + 1] && i != 255)
                    onlyFansTierSub++;
                pixelLookup.Add(Library.infillChars[onlyFansTierSub]);
            }

            // Find all images
            string[] files = Directory.GetFiles("C:\\Users\\huner\\source\\repos\\ImageToAscii\\ImageToAscii\\bin\\Debug\\net8.0");
            List<string> images = new List<string>();

            foreach (string file in files)
            {
                List<string> fragments = new List<string>(file.Split("\\"));
                if (fragments.Last().Contains("png"))
                    images.Add(fragments.Last());
            }

            foreach (string picture in images)
            {
                using Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(picture);
                image.Mutate(x => x.Resize(Library.Max, Library.Max));

                image.ProcessPixelRows(accessor =>
                {
                    Rgba32 transparent = Color.White;

                    for (int y = 0; y < accessor.Height; y++)
                    {
                        Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                        for (int x = 0; x < pixelRow.Length; x++)
                        {
                            ref Rgba32 pixel = ref pixelRow[x];

                            // Injection Mold Pixel to ASCII type shit
                            byte val = RGBToGrayScale(ref pixel);
                            finalOutput += pixelLookup[val];
                            if (copyOutput.Length / 2 % Library.Max == 0)
                                copyOutput += "\n";
                            copyOutput += pixelLookup[val];
                        }
                        finalOutput += "\n";
                    }
                });
                image.Save("Finished" + picture);

                Console.WriteLine("Image: ");
                Console.WriteLine(finalOutput);

                Console.WriteLine("Copy Output: ");
                Console.WriteLine(copyOutput);

                // Clear Buffer to Suffer
                finalOutput = "";
                copyOutput = "";
            }
        }
        public static byte RGBToGrayScale(ref Rgba32 pixel)
        {
            float newPixel = (pixel.R + pixel.G + pixel.B) / 3;
            pixel.R = (byte)newPixel;
            pixel.G = (byte)newPixel;
            pixel.B = (byte)newPixel;
            return (byte)newPixel;
        }
    }
}