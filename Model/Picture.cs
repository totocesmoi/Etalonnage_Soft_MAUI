using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Picture
    {
        public byte[] ImageData { get; set; }

        public Picture()
        {
            ImageData = Array.Empty<byte>();
        }

        public Picture(byte[] imageData)
        {
            ImageData = imageData;
        }

        public string ToBase64()
        {
            return Convert.ToBase64String(ImageData);
        }

        public static Picture FromBase64(string base64)
        {
            return new Picture(Convert.FromBase64String(base64));
        }

        public static Picture FromStream(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return new Picture(memoryStream.ToArray());
            }
        }
    }
}
