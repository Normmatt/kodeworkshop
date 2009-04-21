using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kodinator
{
    class cARCode
    {
        public static string GetValueFromCode(string code)
        {
            return code.Substring(9, 8).ToUpper().PadLeft(8, '0');
        }

        public static int GetHexValueFromCode(string code)
        {
            return Convert.ToInt32(code.Substring(9, 8).ToUpper().PadLeft(8, '0'), 16);
        }

        public static string GetAddressFromCode(string code)
        {
            return code.Substring(0, 8).ToUpper().PadLeft(8, '0');
        }

        public static string GetAddressFromInt(int code)
        {
            return Convert.ToString(code,16).ToUpper().PadLeft(8, '0');
        }

        public static string GetOffsetFromInt(int code)
        {
            if (code != 0)
            {
                return "0x" + Convert.ToString(code, 16).ToUpper().PadLeft(8, '0');
            }
            else
            {
                return "offset";
            }
        }

        public static int GetHexAddressFromCode(string code)
        {
            return Convert.ToInt32(code.Substring(0, 8).ToUpper().PadLeft(8, '0'), 16);
        }

        public static bool isCodeValid(string code, string value)
        {
            if ((code.Length != 8) || (value.Length != 8))
            {
                return false;
            }

            code = code.ToUpper();
            value = value.ToUpper();
            for (int i = 0; i < 8; i++)
            {
                if (i < 8)
                {
                    switch (code.Substring(i, 1))
                    {
                        case "0":
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                        case "A":
                        case "B":
                        case "C":
                        case "D":
                        case "E":
                        case "F":
                            break;

                        default:
                            return false;
                    }
                }
                else
                {
                    switch (value.Substring(i, 1))
                    {
                        case "0":
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                        case "A":
                        case "B":
                        case "C":
                        case "D":
                        case "E":
                        case "F":
                            break;

                        default:
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
