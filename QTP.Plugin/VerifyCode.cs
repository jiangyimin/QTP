using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace QTP.Plugin
{
    public class Node
    {
        public Node(int x1, int x2)
        {
            this.x1 = x1;
            this.x2 = x2;
        }

        public int x1 { get; set; }
        public int x2 { get; set; }
        public int y1 { get; set; }
        public int y2 { get; set; }

    }

    public abstract class VerifyCode
    {
        protected Bitmap image;
        // number of chars in verify_code.
        private int count;

        public VerifyCode(Bitmap image, int count)
        {
            this.image = image;
            this.count = count;
        }

        public String GetCodeString()
        {
            // get nodes of verify_code
            List<Node> nodes = GetNodes();

            // Get codeString
            string codeString = string.Empty;
            foreach (Node node in nodes)
            {
                codeString += Recognize(GetCharFeature(node));
            }

            return codeString;
        }

        public int PixelThreshold(int pixel)
        {
            int result = 0;
            int r = pixel >> 16 & 0xff;
            int g = pixel >> 8 & 0xff;
            int b = pixel & 0xff;
            int tmp = r * r + g * g + b * b;
            if (tmp > 49152)
                result = 1;
            return result;
        }


        // get feature of node
        private String GetCharFeature(Node node)
        {
            String featur = string.Empty;
            for (int y = node.y1; y <= node.y2; y++)
            {
                for (int x = node.x1; x <= node.x2; x++)
                {
                    if (PixelThreshold(image.GetPixel(x, y).ToArgb()) == 0)
                    {
                        featur += string.Format("[{0},{1}]", x - node.x1, y - node.y1);
                    }
                }
            }

            return featur;
        }

        protected abstract List<Node> GetNodes();
        protected abstract string Recognize(string feature);
    }
}
