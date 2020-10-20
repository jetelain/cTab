using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;

namespace cTabWebApp
{

    public class QrFontCode : AbstractQRCode, IDisposable
    {
        const string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public QrFontCode() { }

        public QrFontCode(QRCodeData data) 
            : base(data) { }

        public string GetString()
        {
            var size = QrCodeData.ModuleMatrix.Count;
            var sb = new StringBuilder();
            for (var y = 0; y < size; y += 3)
            {
                for (var x = 0; x < size; x += 2)
                {
                     var value = (GetValue(x, y)) |
                                 (GetValue(x + 1, y) << 1) |
                                 (GetValue(x, y + 1) << 2) |
                                 (GetValue(x + 1, y + 1) << 3) |
                                 (GetValue(x, y + 2) << 4) |
                                 (GetValue(x + 1, y+2) << 5);
                    sb.Append(alpha[value]);
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        private int GetValue(int x, int y)
        {
            if ( x >= QrCodeData.ModuleMatrix.Count || y >= QrCodeData.ModuleMatrix.Count)
            {
                return 0;
            }
            return QrCodeData.ModuleMatrix[y][x] ? 0 : 1;
        }
    }
}
