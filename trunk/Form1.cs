using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kode_Workshop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp = "";
            string sCode = "";
            string sValue = "";
            int code = 0;
            int value = 0;

            //Lets clear textbox so we dont get lots of crap
            codeOutput.Clear();

            for (int i = 0; i < this.codeInput.Lines.Length; ++i)
            {
                temp = this.codeInput.Lines[i].ToUpper(); //Convert text to uppercase
                if (temp.Length == 17) //Make sure codes are right length
                {
                    sCode = cARCode.GetAddressFromCode(temp); //First 32bits
                    sValue = cARCode.GetValueFromCode(temp); //Last 32Bits
                    if (cARCode.isCodeValid(sCode, sValue)) //Lets check if there are any illegal characters
                    {
                        code = cARCode.GetHexAddressFromCode(temp); //First 32bits
                        value = cARCode.GetHexValueFromCode(temp); //Last 32Bits

                        codeOutput.SelectionColor = Color.Blue;
                        codeOutput.AppendText(temp + " ");
                        codeOutput.AppendText(":\n");

                        int codeType = (code >> 28) & 0x0F;
                        int dType = (code >> 24) & 0x0F;

                        codeOutput.SelectionColor = Color.Green;

                        switch (codeType)
                        {
                            case 0:
                                {
                                    codeOutput.AppendText("Type 0 : 32 bits write (str)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("writes word 0x" + sValue + " to [0x" + sCode + "+offset])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 1:
                                {
                                    codeOutput.AppendText("Type 1 : 16 bits write (strh)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("writes halfword 0x" + sValue.Remove(0, 4) + " to [0x" + sCode + "+offset])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 2:
                                {
                                    codeOutput.AppendText("Type 2 : 8 bits write (strb)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("writes byte 0x" + sValue.Remove(0, 6) + " to [0x" + sCode + "+offset])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 3:
                                {
                                    codeOutput.AppendText("Type 3 : 32 bits If (code value)>(data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("checks if 0x" + sValue + " > (word at [" + sCode + "])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 4:
                                {
                                    codeOutput.AppendText("Type 4 : 32 bits If (code value)<(data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("checks if 0x" + sValue + " > (word at [" + sCode + "])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 5:
                                {
                                    codeOutput.AppendText("Type 5 : 32 bits If ==\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("checks if 0x" + sValue + " > (word at [" + sCode + "])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 6:
                                {
                                    codeOutput.AppendText("Type 6 : 32 bits If !=\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("checks if 0x" + sValue + " > (word at [" + sCode + "])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 7:
                                {
                                    codeOutput.AppendText("Type 7 : 16 bits If (code value)>(mask & data at address) (unsigned)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("7XXXXXXX ZZZZYYYY : checks if (YYYY) > (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : checks if (0x" + sValue.Substring(4, 4) + ") > (0x" + Convert.ToString(~(value >> 16)&0xFFFF, 16) + " & halfword at [" + sCode + "])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 8:
                                {
                                    codeOutput.AppendText("Type 8 : 16 bits if (code value)<(mask & data at address) (unsigned)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("8XXXXXXX ZZZZYYYY : checks if (YYYY) < (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : checks if (0x" + sValue.Substring(4, 4) + ") < (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & halfword at [" + sCode + "])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 9:
                                {
                                    codeOutput.AppendText("Type 9 : 16 bits if (code value)==(mask & data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("9XXXXXXX ZZZZYYYY : checks if (YYYY) == (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : checks if (0x" + sValue.Substring(4, 4) + ") == (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & halfword at [" + sCode + "])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 10:
                                {
                                    codeOutput.AppendText("Type A : 16 bits if (code value)!=(mask & data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("AXXXXXXX ZZZZYYYY : checks if (YYYY) != (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : checks if (0x" + sValue.Substring(4, 4) + ") != (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & halfword at [" + sCode + "])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 11:
                                {
                                    codeOutput.AppendText("Type B : loads the 32bits value into the 'offset'\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("BXXXXXXXX 00000000 : offset = word at [0XXXXXXX]\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : offset = word at [" + sCode + "]\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 12:
                                {
                                    codeOutput.AppendText("Type C : defines the start of the loop code\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("C0000000 YYYYYYYY : set the 'Dx repeat value' to YYYYYYYY, saves the 'Dx next\ncode to be executed' and the 'Dx execution status'. Repeat will be executed when\na D1/D2 code is encountered. When repeat is executed, the AR reloads the 'next\ncode to be executed' and the 'execution status' from the Dx registers\n");
                                    codeOutput.AppendText("In This Case : Dx repeat value = 0x" + sValue + "\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 13:
                                {
                                    switch (dType)
                                    {
                                        case 0:
                                            {
                                                codeOutput.AppendText("Type D0 : 'endif'\n");
                                                if (((code & 0x00FFFFFF) != 0) || (value != 0))
                                                {
                                                    codeOutput.SelectionColor = Color.Red;
                                                    codeOutput.AppendText("Code Error not D0000000 00000000\n");
                                                }
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("D0000000 00000000 : loads the previous execution status (if none exists, the\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("execution status stays at 'execute codes')\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 1:
                                            {
                                                codeOutput.AppendText("Type D1 : Used to execute the loop set by the code type C (executes the code(s)\nafter the type C code n times (n being the 'Dx repeat value'), but does not\nclear the Dx registers upon finishing).\nD1000000 00000000 : if the 'Dx repeat value', set by code type C, is different\nthan 0, it is decremented and then the AR loads the 'Dx next code to be executed'\nand the 'execution status' (=jumps back to the code following the type C code).\nWhen the repeat value is 0, this code will load the saved code status value.\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 2:
                                            {
                                                codeOutput.AppendText("Type D2 : Used to apply the code type C setting (executes the code(s) after the\ntype C code n times, n being the Dx repeat value).\nAlso acts as a 'Full terminator' (clears all temporary data, ie. execution\nstatus, offsets, code C settings...).\nD2000000 00000000 : if the 'Dx repeat value', set by code type C, is different\nthan 0, it is decremented and then the AR loads the 'Dx next code to be executed'\nand the 'execution status' (=jumps back to the code following the type C code).\nWhen the repeat value is 0, this code will clear the code status, the offset\nvalue, and the Dx data value (which can be set by codes DA, DB and DC).\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 3:
                                            {
                                                codeOutput.AppendText("Type D3 : set the 'offset' to the value of the code.\nD3000000 XXXXXXXX : set the offset value to XXXXXXXX.\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : set the offset value to 0x" + sValue + "\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 4:
                                            {
                                                codeOutput.AppendText("Type D4 : adds the value of the code to the data register used by D6~DB.\nD4000000 XXXXXXXX : adds XXXXXXXX to the 'Dx data'.\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : adds 0x" + sValue + " to the 'Dx data'\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 5:
                                            {
                                                codeOutput.AppendText("Type D5 : sets the data register used by D6~D8 to the value of the code.\nD5000000 XXXXXXXX : sets the 'Dx data' to XXXXXXXX\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : sets the 'Dx data' to 0x" + sValue + "\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 6:
                                            {
                                                codeOutput.AppendText("Type D6 : 32-bits incremental write of the data register (str).\nD6000000 XXXXXXXX : writes the 'Dx data' word to [XXXXXXXX+offset], and\nincrements the offset by 4\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : writes the 'Dx data' word to [" + sValue + "+offset], and increments the offset by 4\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 7:
                                            {
                                                codeOutput.AppendText("ype D7 : 16-bits incremental write of the data register (strh).\nD7000000 XXXXXXXX : writes the 'Dx data' halfword to [XXXXXXXX+offset], and\nincrements the offset by 2.\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : writes the 'Dx data' halfword to [" + sValue + "+offset], and increments the offset by 2\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 8:
                                            {
                                                codeOutput.AppendText("Type D8 : 8-bits incremental write of the data register (strb).\nD8000000 XXXXXXXX : writes the 'Dx data' byte to [XXXXXXXX+offset], and\nincrements the offset by 1.\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : writes the 'Dx data' byte to [" + sValue + "+offset], and increments the offset by 1\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 9:
                                            {
                                                codeOutput.AppendText("Type D9 : 32-bits read to the data register (ldr).\nD9000000 XXXXXXXX : loads the word at [XXXXXXXX+offset] and stores it in the\n'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : loads the word at [" + sValue + "+offset] and stores it in the 'Dx data'\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 10:
                                            {
                                                codeOutput.AppendText("Type DA : 16-bits read to the data register (ldrh).\nDA000000 XXXXXXXX : loads the halfword at [XXXXXXXX+offset] and stores it in\nthe 'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : loads the halfword at [" + sValue + "+offset] and stores it in the 'Dx data'\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 11:
                                            {
                                                codeOutput.AppendText("Type DB : 8-bits read to the data register (ldrb).\nDB000000 XXXXXXXX : loads the byte at [XXXXXXXX+offset] and stores it in the\n'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : loads the byte at [" + sValue + "+offset] and stores it in the 'Dx data'\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 12:
                                            {
                                                codeOutput.AppendText("Type DC : adds the offset 'data' to the current offset.\n(some kind of dual offset)\nDC000000 XXXXXXXX : offset = (offset + XXXXXXXX)\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : offset = (offset + 0x" + sValue + ")\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        default:
                                        {
                                            codeOutput.SelectionColor = Color.Red;
                                            codeOutput.AppendText("Type D" + Convert.ToString(dType,16).ToUpper() + " : Invalid Code Type\n");
                                            codeOutput.AppendText("\n");
                                            break;
                                        }
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    else
                    {
                        codeOutput.AppendText("Parse error on line #" + i);
                    }
                }
                else
                {
                    codeOutput.AppendText("Parse error on line #" + i);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
