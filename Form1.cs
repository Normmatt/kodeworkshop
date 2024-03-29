﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kodinator
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

            int ifCount = 0;
            int copyCount = 0;

            //Lets clear textbox so we dont get lots of crap
            codeOutput.Clear();

            for (int i = 0; i < this.codeInput.Lines.Length; ++i)
            {
                temp = this.codeInput.Lines[i].ToUpper(); //Convert text to uppercase
                if (temp.Length == 17 && (copyCount == 0)) //Make sure codes are right length and that E/F code copys arent checked
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
                                    if (code == 0)
                                    {
                                        codeOutput.AppendText("Type 0 : Manual Hook\n");
                                        break;
                                    }
                                    codeOutput.AppendText("Type 0 : 32 bits write (str)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("writes word 0x" + sValue + " to [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "+offset])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 1:
                                {
                                    codeOutput.AppendText("Type 1 : 16 bits write (strh)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("writes halfword 0x" + sValue.Remove(0, 4) + " to [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "+offset])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 2:
                                {
                                    codeOutput.AppendText("Type 2 : 8 bits write (strb)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("writes byte 0x" + sValue.Remove(0, 6) + " to [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "+offset])\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 3:
                                {
                                    codeOutput.AppendText("Type 3 : 32 bits If (code value)>(data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("checks if 0x" + sValue + " > (word at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "])\n");
                                    codeOutput.AppendText("\n");
                                    ifCount++;
                                    break;
                                }
                            case 4:
                                {
                                    codeOutput.AppendText("Type 4 : 32 bits If (code value)<(data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("checks if 0x" + sValue + " < (word at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "])\n");
                                    codeOutput.AppendText("\n");
                                    ifCount++;
                                    break;
                                }
                            case 5:
                                {
                                    codeOutput.AppendText("Type 5 : 32 bits If ==\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("checks if 0x" + sValue + " == (word at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "])\n");
                                    codeOutput.AppendText("\n");
                                    ifCount++;
                                    break;
                                }
                            case 6:
                                {
                                    codeOutput.AppendText("Type 6 : 32 bits If !=\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("checks if 0x" + sValue + " != (word at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "])\n");
                                    codeOutput.AppendText("\n");
                                    ifCount++;
                                    break;
                                }
                            case 7:
                                {
                                    codeOutput.AppendText("Type 7 : 16 bits If (code value)>(mask & data at address) (unsigned)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("7XXXXXXX ZZZZYYYY : checks if (YYYY) > (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : checks if (0x" + sValue.Substring(4, 4) + ") > (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & halfword at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "])\n");
                                    codeOutput.AppendText("\n");
                                    ifCount++;
                                    break;
                                }
                            case 8:
                                {
                                    codeOutput.AppendText("Type 8 : 16 bits if (code value)<(mask & data at address) (unsigned)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("8XXXXXXX ZZZZYYYY : checks if (YYYY) < (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : checks if (0x" + sValue.Substring(4, 4) + ") < (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & halfword at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "])\n");
                                    codeOutput.AppendText("\n");
                                    ifCount++;
                                    break;
                                }
                            case 9:
                                {
                                    codeOutput.AppendText("Type 9 : 16 bits if (code value)==(mask & data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("9XXXXXXX ZZZZYYYY : checks if (YYYY) == (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : checks if (0x" + sValue.Substring(4, 4) + ") == (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & halfword at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "])\n");
                                    codeOutput.AppendText("\n");
                                    ifCount++;
                                    break;
                                }
                            case 10:
                                {
                                    codeOutput.AppendText("Type A : 16 bits if (code value)!=(mask & data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("AXXXXXXX ZZZZYYYY : checks if (YYYY) != (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : checks if (0x" + sValue.Substring(4, 4) + ") != (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & halfword at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "])\n");
                                    codeOutput.AppendText("\n");
                                    ifCount++;
                                    break;
                                }
                            case 11:
                                {
                                    codeOutput.AppendText("Type B : loads the 32bits value into the 'offset'\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("BXXXXXXXX 00000000 : offset = word at [0XXXXXXX]\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : offset = word at [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "]\n");
                                    codeOutput.AppendText("\n");
                                    break;
                                }
                            case 12:
                                {
                                    //Todo add code types C4 and C5
                                    switch (dType)
                                    {
                                        case 0:
                                            {
                                                if (((code & 0x0FFFFFFF) != 0))
                                                {
                                                    codeOutput.SelectionColor = Color.Red;
                                                    codeOutput.AppendText("Code Error not C0000000\n");
                                                    break;
                                                }
                                                codeOutput.AppendText("Type C : defines the start of the loop code\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("C0000000 YYYYYYYY : set the 'Dx repeat value' to YYYYYYYY, saves the 'Dx next\ncode to be executed' and the 'Dx execution status'. Repeat will be executed when\na D1/D2 code is encountered. When repeat is executed, the AR reloads the 'next\ncode to be executed' and the 'execution status' from the Dx registers\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : Dx repeat value = 0x" + sValue + "\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 6:
                                            {
                                                if (((code & 0x00FFFFFF) != 0))
                                                {
                                                    codeOutput.SelectionColor = Color.Red;
                                                    codeOutput.AppendText("Code Error not C6000000\n");
                                                    break;
                                                }
                                                codeOutput.AppendText("Type C6 : Stores the offset at...\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("C6000000 YYYYYYYY Will store the offset at [YYYYYYYY].\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : word at [0x" + sValue + "] = offset\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    break;
                                }
                            case 13:
                                {
                                    if (((code & 0x00FFFFFF) != 0))
                                    {
                                        codeOutput.SelectionColor = Color.Red;
                                        codeOutput.AppendText("Code Error not D" + Convert.ToString(dType,16).ToUpper() + "000000\n");
                                        break;
                                    }

                                    switch (dType)
                                    {
                                        case 0:
                                            {
                                                codeOutput.AppendText("Type D0 : 'endif'\n");
                                                if ((value != 0))
                                                {
                                                    codeOutput.SelectionColor = Color.Red;
                                                    codeOutput.AppendText("Code Error value not 00000000\n");
                                                    break;
                                                }
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("D0000000 00000000 : loads the previous execution status (if none exists, the\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("execution status stays at 'execute codes')\n");
                                                codeOutput.AppendText("\n");
                                                --ifCount;
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
                                                ifCount = 0; //Reset if count as this  code type clears everything
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
                                                codeOutput.AppendText("In This Case : writes the 'Dx data' word to [0x" + sValue + "+offset], and increments the offset by 4\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 7:
                                            {
                                                codeOutput.AppendText("ype D7 : 16-bits incremental write of the data register (strh).\nD7000000 XXXXXXXX : writes the 'Dx data' halfword to [XXXXXXXX+offset], and\nincrements the offset by 2.\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : writes the 'Dx data' halfword to [0x" + sValue + "+offset], and increments the offset by 2\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 8:
                                            {
                                                codeOutput.AppendText("Type D8 : 8-bits incremental write of the data register (strb).\nD8000000 XXXXXXXX : writes the 'Dx data' byte to [XXXXXXXX+offset], and\nincrements the offset by 1.\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : writes the 'Dx data' byte to [0x" + sValue + "+offset], and increments the offset by 1\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 9:
                                            {
                                                codeOutput.AppendText("Type D9 : 32-bits read to the data register (ldr).\nD9000000 XXXXXXXX : loads the word at [XXXXXXXX+offset] and stores it in the\n'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : loads the word at [0x" + sValue + "+offset] and stores it in the 'Dx data'\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 10:
                                            {
                                                codeOutput.AppendText("Type DA : 16-bits read to the data register (ldrh).\nDA000000 XXXXXXXX : loads the halfword at [XXXXXXXX+offset] and stores it in\nthe 'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : loads the halfword at [0x" + sValue + "+offset] and stores it in the 'Dx data'\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                        case 11:
                                            {
                                                codeOutput.AppendText("Type DB : 8-bits read to the data register (ldrb).\nDB000000 XXXXXXXX : loads the byte at [XXXXXXXX+offset] and stores it in the\n'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("In This Case : loads the byte at [0x" + sValue + "+offset] and stores it in the 'Dx data'\n");
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
                            case 14:
                                {
                                    codeOutput.AppendText("Type E : 'patch' code. Copies YYYYYYYY bytes from (current code location + 8)\nto  [XXXXXXXX + offset].\nEXXXXXXX YYYYYYYY\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : Copies 0x" + sValue + " bytes from (current code location + 8) to  [0x" + sCode.Remove(0, 1).PadLeft(8, '0') + " + offset]\n");
                                    codeOutput.AppendText("\n");
                                    copyCount = value;
                                    break;
                                }
                            case 15:
                                {
                                    codeOutput.AppendText("Type F : memory copy code. It seems you have to use the code type D3, DC or B\nbefore, to set the offset (which is then an address). Then D2 should be needed\nto clear the offset (else it will affect all the next codes).\nD3000000 XXXXXXXX\nFYYYYYYY ZZZZZZZZ\nshould copy ZZZZZZZZ bytes from offset (=XXXXXXXX in this case) to YYYYYYYY\n(YYYYYYYY if fixed, ie. no offset are added to it). \n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : copy 0x" + sValue + " bytes from offset (XXXXXXXX) to " + sCode.Remove(0, 1).PadLeft(8, '0') + "\n");
                                    codeOutput.AppendText("\n");
                                    copyCount = value;
                                    continue;
                                }
                            default:
                                break;
                        }
                    }
                    else
                    {
                        codeOutput.AppendText("Parse error on line #" + i + " (Contains illegal characters)");
                    }
                }
                else
                {
                    if (temp.Length != 17)
                    {
                        codeOutput.AppendText("Length of line is invalid on line #" + i + " (" + temp + ")");
                    }
                    else
                    {
                        copyCount -= 8; //Remove 8 from copy count
                    }
                }
            }

            //Lets make sure there arent any missing ENDIF codes
            if (ifCount != 0)
            {
                codeOutput.SelectionColor = Color.Red;
                codeOutput.AppendText("You are missing " + ifCount + " ENDIF codes\n");
                codeOutput.AppendText("\n");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((branchSource.Text.Length == 10) && (branchDestination.Text.Length == 10))
            {
                string sSource = branchSource.Text.Remove(0, 2); //Remove initial 0x
                string sDest = branchDestination.Text.Remove(0, 2); //Remove initial 0x
                uint Source = Convert.ToUInt32(sSource, 16);
                uint Dest = Convert.ToUInt32(sDest, 16);
                branchOutput.Clear(); //Best make sure the box is empty before writting

                if (comboBox1.SelectedIndex==1) //Thumb BL
                {
                    //Check Source is 2byte aligned
                    uint Temp = (uint)((Source + 1) & ~1);
                    if (Temp != Source)
                    {
                        Source = Temp; //Lets store new aligned address in source
                        sSource = Convert.ToString(Temp, 16).PadLeft(8, '0');
                        branchOutput.SelectionColor = Color.Red; //Lets make error stand out
                        branchOutput.AppendText("The Source address is not 2byte aligned it has been corrected to 0x" + sSource.ToUpper() + "\n"); //Make sure output is uppercase
                        branchOutput.SelectionColor = Color.Green; //Return color to normal
                    }

                    //Check Destination is 2byte aligned
                    Temp = (uint)((Dest + 1) & ~1);
                    if (Temp != Dest)
                    {
                        Dest = Temp; //Make sure destination is 4 byte aligned
                        sDest = Convert.ToString(Temp, 16).PadLeft(8, '0');
                        branchOutput.SelectionColor = Color.Red; //Lets make error stand out
                        branchOutput.AppendText("The Destination address is not 2byte aligned it has been corrected to 0x" + sDest.ToUpper() + "\n"); //Make sure output is uppercase
                        branchOutput.SelectionColor = Color.Green; //Return color to normal
                    }

                    uint tneg = 0;
                    uint Diff = (Dest - Source - 4) & 0x003FFFFF; //PC is always 8 bytes ahead of source address
                    if ((Dest - Source - 4) >= 0x003FFFFF)
                        tneg = 1;
                    if (Diff <= 0x003FFFFE && (Diff & 1) == 0) //Lets make sure the branch is valid
                    {
                        uint part1= (0xF8000000 | ((Diff & 0xFFE) << 15));
                        uint part2= (0xF000 | ((Diff >> 12) & 0x7FF) | (tneg << 10));
                        uint Opcode = part1 | part2;

                        sSource = Convert.ToString(Source, 16).PadLeft(8, '0'); //Lets reuse these variables
                        sDest = Convert.ToString(Opcode, 16).PadLeft(8, '0'); //Lets reuse these variables

                        branchOutput.AppendText(sSource.ToUpper() + " " + sDest.ToUpper()); //Make sure output is uppercase
                    }
                    else
                    {
                        sSource = Convert.ToString((Diff - 0x003FFFFE), 16).PadLeft(8, '0'); ///Lets reuse these variables
                        branchOutput.SelectionColor = Color.Red; //Lets make error stand out
                        branchOutput.AppendText("The Destination is 0x" + sSource.ToUpper() + " bytes too far away from the Source\n"); //Make sure output is uppercase
                    }
                }
                else //Arm Branch
                {
                    //Check Source is 4byte aligned
                    uint Temp = (uint)((Source + 3) & ~3);
                    if (Temp != Source)
                    {
                        Source = Temp; //Lets store new aligned address in source
                        sSource = Convert.ToString(Temp, 16).PadLeft(8, '0');
                        branchOutput.SelectionColor = Color.Red; //Lets make error stand out
                        branchOutput.AppendText("The Source address is not 4byte aligned it has been corrected to 0x" + sSource.ToUpper() + "\n"); //Make sure output is uppercase
                        branchOutput.SelectionColor = Color.Green; //Return color to normal
                    }

                    //Check Destination is 4byte aligned
                    Temp = (uint)((Dest + 3) & ~3);
                    if (Temp != Dest)
                    {
                        Dest = Temp; //Make sure destination is 4 byte aligned
                        sDest = Convert.ToString(Temp, 16).PadLeft(8, '0');
                        branchOutput.SelectionColor = Color.Red; //Lets make error stand out
                        branchOutput.AppendText("The Destination address is not 4byte aligned it has been corrected to 0x" + sDest.ToUpper() + "\n"); //Make sure output is uppercase
                        branchOutput.SelectionColor = Color.Green; //Return color to normal
                    }

                    uint Diff = ((Dest - Source - 8) >> 2) & 0x00FFFFFF; //PC is always 8 bytes ahead of source address
                    if (Diff <= 0x00FFFFFF) //Lets make sure the branch is valid
                    {
                        uint Opcode = 0xEA000000 | Diff;

                        sSource = Convert.ToString(Source, 16).PadLeft(8, '0'); //Lets reuse these variables
                        sDest = Convert.ToString(Opcode, 16).PadLeft(8, '0'); //Lets reuse these variables

                        branchOutput.AppendText(sSource.ToUpper() + " " + sDest.ToUpper()); //Make sure output is uppercase
                    }
                    else
                    {
                        sSource = Convert.ToString(((Diff - 0x00FFFFFF) * 4), 16).PadLeft(8, '0'); ///Lets reuse these variables
                        branchOutput.SelectionColor = Color.Red; //Lets make error stand out
                        branchOutput.AppendText("The Destination is 0x" + sSource.ToUpper() + " bytes too far away from the Source\n"); //Make sure output is uppercase
                    }
                }
            }
            else
            {
                branchOutput.SelectionColor = Color.Red; //Lets make error stand out
                branchOutput.AppendText("Both source and destination must start with 0x and must be a total of 10 characters long.\n"); //Make sure output is uppercase
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string temp = "";
            string sCode = "";
            string sValue = "";
            int code = 0;
            int value = 0;

            int offset = 0;
            int ifCount = 0;
            int loopCount = 0;
            int copyCount = 0;
            int copyAddress = 0;

            //Lets clear textbox so we dont get lots of crap
            codeOutput.Clear();

            for (int i = 0; i < this.codeInput.Lines.Length; ++i)
            {
                temp = this.codeInput.Lines[i].ToUpper(); //Convert text to uppercase
                sCode = cARCode.GetAddressFromCode(temp); //First 32bits
                sValue = cARCode.GetValueFromCode(temp); //Last 32Bits

                if (temp.Length == 17 && (copyCount==0)) //Make sure codes are right length and that E/F code copys arent checked
                {
                    if (cARCode.isCodeValid(sCode, sValue)) //Lets check if there are any illegal characters
                    {
                        code = cARCode.GetHexAddressFromCode(temp); //First 32bits
                        value = cARCode.GetHexValueFromCode(temp); //Last 32Bits

                        int codeType = (code >> 28) & 0x0F;
                        int dType = (code >> 24) & 0x0F;
                        string indent = "";

                        if (ifCount != 0 || loopCount != 0)
                        {
                            for (int j = 0; j < ifCount + loopCount; j++) //Lets hack this for now
                            {
                                indent += "    "; //Append 4 spaces so keeps indentation even
                            }
                        }

                        if (codeType < 0x0D) //Lets not append indend to ENDIF we'll do that manually for now
                        {
                            codeOutput.AppendText(indent);
                        }
                        else
                        {
                            if ((dType < 4) && codeType == 0x0D)
                            {
                                indent = "    "; //4 spaces so things dont crash yay what a hack!
                            }
                        }

                        codeOutput.SelectionColor = Color.Blue;

                        switch (codeType)
                        {
                            case 0:
                                {
                                    codeOutput.AppendText("*(unsigned int*)(0x" + sCode.Remove(0, 1).PadLeft(8,'0') + " + " + cARCode.GetOffsetFromInt(offset) + ") = (unsigned int)0x" + sValue + ";\n");
                                    break;
                                }
                            case 1:
                                {
                                    codeOutput.AppendText("*(unsigned short*)(0x" + sCode.Remove(0, 1).PadLeft(8, '0') + " + " + cARCode.GetOffsetFromInt(offset) + ") = (unsigned short)0x" + sValue + ";\n");
                                    break;
                                }
                            case 2:
                                {
                                    codeOutput.AppendText("*(unsigned char*)(0x" + sCode.Remove(0, 1).PadLeft(8, '0') + " + " + cARCode.GetOffsetFromInt(offset) + ") = (unsigned char)0x" + sValue + ";\n");
                                    break;
                                }
                            case 3:
                                {
                                    codeOutput.AppendText("if(0x" + sValue + " > (*(unsigned int*)0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "))\n");
                                    codeOutput.SelectionColor = Color.Blue;
                                    codeOutput.AppendText(indent);
                                    codeOutput.AppendText("{\n");
                                    ifCount++;
                                    break;
                                }
                            case 4:
                                {
                                    codeOutput.AppendText("if(0x" + sValue + " < (*(unsigned int*)0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "))\n");
                                    codeOutput.SelectionColor = Color.Blue;
                                    codeOutput.AppendText(indent);
                                    codeOutput.AppendText("{\n");
                                    ifCount++;
                                    break;
                                }
                            case 5:
                                {
                                    codeOutput.AppendText("if(0x" + sValue + " == (*(unsigned int*)0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "))\n");
                                    codeOutput.SelectionColor = Color.Blue;
                                    codeOutput.AppendText(indent);
                                    codeOutput.AppendText("{\n");
                                    ifCount++;
                                    break;
                                }
                            case 6:
                                {
                                    codeOutput.AppendText("if(0x" + sValue + " != (*(unsigned int*)0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "))\n");
                                    codeOutput.SelectionColor = Color.Blue;
                                    codeOutput.AppendText(indent);
                                    codeOutput.AppendText("{\n");
                                    ifCount++;
                                    break;
                                }
                            case 7:
                                {
                                    /*codeOutput.AppendText("Type 7 : 16 bits If (code value)>(mask & data at address) (unsigned)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("7XXXXXXX ZZZZYYYY : checks if (YYYY) > (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;*/
                                    codeOutput.AppendText("if(0x" + sValue.Substring(4, 4) + " > (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & *(unsigned short*)0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "))\n");
                                    codeOutput.SelectionColor = Color.Blue;
                                    codeOutput.AppendText(indent);
                                    codeOutput.AppendText("{\n");
                                    ifCount++;
                                    break;
                                }
                            case 8:
                                {
                                    /*codeOutput.AppendText("Type 8 : 16 bits if (code value)<(mask & data at address) (unsigned)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("8XXXXXXX ZZZZYYYY : checks if (YYYY) < (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;*/
                                    codeOutput.AppendText("if(0x" + sValue.Substring(4, 4) + " < (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & *(unsigned short*)0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "))\n");
                                    codeOutput.SelectionColor = Color.Blue;
                                    codeOutput.AppendText(indent);
                                    codeOutput.AppendText("{\n");
                                    ifCount++;
                                    break;
                                }
                            case 9:
                                {
                                    /*codeOutput.AppendText("Type 9 : 16 bits if (code value)==(mask & data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("9XXXXXXX ZZZZYYYY : checks if (YYYY) == (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;*/
                                    codeOutput.AppendText("if(0x" + sValue.Substring(4, 4) + " == (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & *(unsigned short*)0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "))\n");
                                    codeOutput.SelectionColor = Color.Blue;
                                    codeOutput.AppendText(indent);
                                    codeOutput.AppendText("{\n");
                                    ifCount++;
                                    break;
                                }
                            case 10:
                                {
                                    /*codeOutput.AppendText("Type A : 16 bits if (code value)!=(mask & data at address)\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("AXXXXXXX ZZZZYYYY : checks if (YYYY) != (not (ZZZZ) & halfword at [XXXXXXXX])\n");
                                    codeOutput.SelectionColor = Color.Green;*/
                                    codeOutput.AppendText("if(0x" + sValue.Substring(4, 4) + " != (0x" + Convert.ToString(~(value >> 16) & 0xFFFF, 16) + " & *(unsigned short*)0x" + sCode.Remove(0, 1).PadLeft(8, '0') + "))\n");
                                    codeOutput.SelectionColor = Color.Blue;
                                    codeOutput.AppendText(indent);
                                    codeOutput.AppendText("{\n");
                                    ifCount++;
                                    break;
                                }
                            case 11:
                                {
                                    /*codeOutput.AppendText("Type B : loads the 32bits value into the 'offset'\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("BXXXXXXXX 00000000 : offset = word at [0XXXXXXX]\n");
                                    codeOutput.SelectionColor = Color.Green;*/
                                    codeOutput.AppendText("offset = *(unsigned int*)(0x" + sCode.Remove(0, 1).PadLeft(8, '0') + " + " + cARCode.GetOffsetFromInt(offset) + ");\n");
                                    offset = 0; //Lets make it say offset if offset cant be found
                                    break;
                                }
                            case 12:
                                {
                                    //Todo add code types C4 and C5
                                    switch (dType)
                                    {
                                        case 0:
                                            {
                                                if (((code & 0x0FFFFFFF) != 0))
                                                {
                                                    codeOutput.SelectionColor = Color.Red;
                                                    codeOutput.AppendText("Code Error not C0000000\n");
                                                    break;
                                                }
                                                /*codeOutput.AppendText("Type C : defines the start of the loop code\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("C0000000 YYYYYYYY : set the 'Dx repeat value' to YYYYYYYY, saves the 'Dx next\ncode to be executed' and the 'Dx execution status'. Repeat will be executed when\na D1/D2 code is encountered. When repeat is executed, the AR reloads the 'next\ncode to be executed' and the 'execution status' from the Dx registers\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText("for (int i=0; i<0x" + sValue + "; i++)\n");
                                                codeOutput.SelectionColor = Color.Blue;
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("{\n");
                                                loopCount++;
                                                break;
                                            }
                                        case 6:
                                            {
                                                if (((code & 0x00FFFFFF) != 0))
                                                {
                                                    codeOutput.SelectionColor = Color.Red;
                                                    codeOutput.AppendText("Code Error not C6000000\n");
                                                    break;
                                                }
                                                /*codeOutput.AppendText("Type C6 : Stores the offset at...\n");
                                                codeOutput.SelectionColor = Color.Green;
                                                codeOutput.AppendText("C6000000 YYYYYYYY Will store the offset at [YYYYYYYY].\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText("*(unsigned int*)0x" + sValue + " = offset;\n");
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                    break;
                                }
                            case 13:
                                {
                                    if (((code & 0x00FFFFFF) != 0))
                                    {
                                        codeOutput.SelectionColor = Color.Red;
                                        codeOutput.AppendText("Code Error not D" + Convert.ToString(dType, 16).ToUpper() + "000000\n");
                                        break;
                                    }

                                    switch (dType)
                                    {
                                        case 0:
                                            {
                                                codeOutput.AppendText(indent.Remove(0, 4));
                                                codeOutput.SelectionColor = Color.Blue;
                                                codeOutput.AppendText("}\n");
                                                --ifCount;
                                                break;
                                            }
                                        case 1:
                                            {
                                                //codeOutput.AppendText("Type D1 : Used to execute the loop set by the code type C (executes the code(s)\nafter the type C code n times (n being the 'Dx repeat value'), but does not\nclear the Dx registers upon finishing).\nD1000000 00000000 : if the 'Dx repeat value', set by code type C, is different\nthan 0, it is decremented and then the AR loads the 'Dx next code to be executed'\nand the 'execution status' (=jumps back to the code following the type C code).\nWhen the repeat value is 0, this code will load the saved code status value.\n");
                                                //codeOutput.AppendText("\n");
                                                codeOutput.AppendText(indent.Remove(0, 4));
                                                codeOutput.SelectionColor = Color.Blue;
                                                if (ifCount != 0 || loopCount != 0)
                                                {
                                                    codeOutput.AppendText("}\n");
                                                }
                                                loopCount--;
                                                break;
                                            }
                                        case 2:
                                            {
                                                //codeOutput.AppendText("Type D2 : Used to apply the code type C setting (executes the code(s) after the\ntype C code n times, n being the Dx repeat value).\nAlso acts as a 'Full terminator' (clears all temporary data, ie. execution\nstatus, offsets, code C settings...).\nD2000000 00000000 : if the 'Dx repeat value', set by code type C, is different\nthan 0, it is decremented and then the AR loads the 'Dx next code to be executed'\nand the 'execution status' (=jumps back to the code following the type C code).\nWhen the repeat value is 0, this code will clear the code status, the offset\nvalue, and the Dx data value (which can be set by codes DA, DB and DC).\n");
                                                //codeOutput.AppendText("\n");
                                                codeOutput.AppendText(indent.Remove(0, 4));
                                                codeOutput.SelectionColor = Color.Blue;
                                                int count = ifCount + loopCount;
                                                if (count != 0) count--; //Lets remove one so we are aligned properly
                                                if (ifCount != 0 || loopCount != 0)
                                                {
                                                    for (int j = count; j-- > 0; ) //Lets hack this for now
                                                    {
                                                        indent += "    "; //Append 4 spaces so keeps indentation even
                                                    }

                                                    for (int k = 0; k < ifCount + loopCount; k++) //Lets hack this for now
                                                    {
                                                        indent = indent.Remove(0, 4);
                                                        codeOutput.AppendText(indent);
                                                        codeOutput.SelectionColor = Color.Blue;
                                                        codeOutput.AppendText("}\n");
                                                    }
                                                }
                                                loopCount = 0;
                                                ifCount = 0; //Reset if count as this  code type clears everything
                                                break;
                                            }
                                        case 3:
                                            {
                                                /*codeOutput.AppendText("Type D3 : set the 'offset' to the value of the code.\nD3000000 XXXXXXXX : set the offset value to XXXXXXXX.\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("offset = 0x" + sValue + ";\n");
                                                offset = value;
                                                break;
                                            }
                                        case 4:
                                            {
                                                /*codeOutput.AppendText("Type D4 : adds the value of the code to the data register used by D6~DB.\nD4000000 XXXXXXXX : adds XXXXXXXX to the 'Dx data'.\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("datareg += 0x" + sValue + ";\n");
                                                break;
                                            }
                                        case 5:
                                            {
                                                /*codeOutput.AppendText("Type D5 : sets the data register used by D6~D8 to the value of the code.\nD5000000 XXXXXXXX : sets the 'Dx data' to XXXXXXXX\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("datareg = 0x" + sValue + ";\n");
                                                break;
                                            }
                                        case 6:
                                            {
                                                /*codeOutput.AppendText("Type D6 : 32-bits incremental write of the data register (str).\nD6000000 XXXXXXXX : writes the 'Dx data' word to [XXXXXXXX+offset], and\nincrements the offset by 4\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("*(unsigned int*)(0x" + sValue + " + " + cARCode.GetOffsetFromInt(offset) + ") = datareg;\n");
                                                codeOutput.AppendText(indent);
                                                codeOutput.SelectionColor = Color.Blue;
                                                codeOutput.AppendText("offset += 4;\n");
                                                offset += 4;
                                                break;
                                            }
                                        case 7:
                                            {
                                                /*codeOutput.AppendText("ype D7 : 16-bits incremental write of the data register (strh).\nD7000000 XXXXXXXX : writes the 'Dx data' halfword to [XXXXXXXX+offset], and\nincrements the offset by 2.\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("*(unsigned short*)(0x" + sValue + " + " + cARCode.GetOffsetFromInt(offset) + ") = datareg;\n");
                                                codeOutput.AppendText(indent);
                                                codeOutput.SelectionColor = Color.Blue;
                                                codeOutput.AppendText("offset += 2;\n");
                                                offset += 2;
                                                break;
                                            }
                                        case 8:
                                            {
                                                /*codeOutput.AppendText("Type D8 : 8-bits incremental write of the data register (strb).\nD8000000 XXXXXXXX : writes the 'Dx data' byte to [XXXXXXXX+offset], and\nincrements the offset by 1.\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("*(unsigned byte*)(0x" + sValue + " + " + cARCode.GetOffsetFromInt(offset) + ") = datareg;\n");
                                                codeOutput.AppendText(indent);
                                                codeOutput.SelectionColor = Color.Blue;
                                                codeOutput.AppendText("offset += 1;\n");
                                                offset += 1;
                                                break;
                                            }
                                        case 9:
                                            {
                                                /*codeOutput.AppendText("Type D9 : 32-bits read to the data register (ldr).\nD9000000 XXXXXXXX : loads the word at [XXXXXXXX+offset] and stores it in the\n'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("datareg = *(unsigned int*)(0x" + sValue + " + " + cARCode.GetOffsetFromInt(offset) + ");\n");
                                                break;
                                            }
                                        case 10:
                                            {
                                                /*codeOutput.AppendText("Type DA : 16-bits read to the data register (ldrh).\nDA000000 XXXXXXXX : loads the halfword at [XXXXXXXX+offset] and stores it in\nthe 'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("datareg = *(unsigned short*)(0x" + sValue + " + " + cARCode.GetOffsetFromInt(offset) + ");\n");
                                                break;
                                            }
                                        case 11:
                                            {
                                                /*codeOutput.AppendText("Type DB : 8-bits read to the data register (ldrb).\nDB000000 XXXXXXXX : loads the byte at [XXXXXXXX+offset] and stores it in the\n'Dx data'\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("datareg = *(unsigned byte*)(0x" + sValue + " + " + cARCode.GetOffsetFromInt(offset) + ");\n");
                                                break;
                                            }
                                        case 12:
                                            {
                                                /*codeOutput.AppendText("Type DC : adds the offset 'data' to the current offset.\n(some kind of dual offset)\nDC000000 XXXXXXXX : offset = (offset + XXXXXXXX)\n");
                                                codeOutput.SelectionColor = Color.Green;*/
                                                codeOutput.AppendText(indent);
                                                codeOutput.AppendText("offset = (0x" + sValue + " + " + cARCode.GetOffsetFromInt(offset) + ");\n");
                                                offset += value;
                                                break;
                                            }
                                        default:
                                            {
                                                codeOutput.SelectionColor = Color.Red;
                                                codeOutput.AppendText("Type D" + Convert.ToString(dType, 16).ToUpper() + " : Invalid Code Type\n");
                                                codeOutput.AppendText("\n");
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case 14:
                                {
                                    /*codeOutput.AppendText("Type E : 'patch' code. Copies YYYYYYYY bytes from (current code location + 8)\nto  [XXXXXXXX + offset].\nEXXXXXXX YYYYYYYY\n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : Copies 0x" + sValue + " bytes from (current code location + 8) to  [0x" + sCode.Remove(0, 1) + " + offset]\n");*/
                                    copyAddress = code & 0x0FFFFFFF;
                                    copyCount = value;
                                    break;
                                }
                            case 15:
                                {
                                    /*codeOutput.AppendText("Type F : memory copy code. It seems you have to use the code type D3, DC or B\nbefore, to set the offset (which is then an address). Then D2 should be needed\nto clear the offset (else it will affect all the next codes).\nD3000000 XXXXXXXX\nFYYYYYYY ZZZZZZZZ\nshould copy ZZZZZZZZ bytes from offset (=XXXXXXXX in this case) to YYYYYYYY\n(YYYYYYYY if fixed, ie. no offset are added to it). \n");
                                    codeOutput.SelectionColor = Color.Green;
                                    codeOutput.AppendText("In This Case : copy 0x" + sValue + " bytes from offset (XXXXXXXX) to " + sCode.Remove(0, 1).PadLeft(8, '0') + "\n");*/
                                    copyAddress = code & 0x0FFFFFFF;
                                    copyCount = value;
                                    continue;
                                }
                            default:
                                break;
                        }
                    }
                    else
                    {
                        codeOutput.AppendText("Parse error on line #" + i + " (Contains illegal characters)");
                    }
                }
                else
                {
                    if (temp.Length != 17)
                    {
                        codeOutput.AppendText("Length of line is invalid on line #" + i + " (" + temp + ")");
                    }
                    else
                    {
                        string indent = "";

                        if (ifCount != 0 || loopCount != 0)
                        {
                            for (int j = 0; j < ifCount + loopCount; j++) //Lets hack this for now
                            {
                                indent += "    "; //Append 4 spaces so keeps indentation even
                            }
                        }

                        codeOutput.AppendText(indent);
                        codeOutput.SelectionColor = Color.Blue;
                        codeOutput.AppendText("*(unsigned int*)(0x" + cARCode.GetAddressFromInt(copyAddress) + "+offset)= (unsigned int)0x" + sCode + ";\n");
                        codeOutput.AppendText(indent);
                        codeOutput.SelectionColor = Color.Blue;
                        codeOutput.AppendText("*(unsigned int*)(0x" + cARCode.GetAddressFromInt(copyAddress+4) + "+offset)= (unsigned int)0x" + sValue + ";\n");
                        copyCount -= 8; //Remove 8 from copy count
                        copyAddress += 8; //Add 8 to the copy address
                    }
                }
            }

            //Lets make sure there arent any missing ENDIF codes
            if (ifCount != 0)
            {
                codeOutput.SelectionColor = Color.Red;
                codeOutput.AppendText("You are missing " + ifCount + " ENDIF codes\n");
                codeOutput.AppendText("\n");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string temp = "";
            string sCode = "";
            string sValue = "";
            int code = 0;
            int value = 0;

            //Lets clear textbox so we dont get lots of crap
            cbdsOutput.Clear();

#if (RELEASE)
            cbdsOutput.Text = "This feature is unfinished.";
            return;
#else
            for (int i = 0; i < this.cbdsInput.Lines.Length; ++i)
            {
                temp = this.cbdsInput.Lines[i].ToUpper(); //Convert text to uppercase
                sCode = cARCode.GetAddressFromCode(temp); //First 32bits
                sValue = cARCode.GetValueFromCode(temp); //Last 32Bits

                if (temp.Length == 17) //Make sure codes are right length and that E/F code copys arent checked
                {
                    if (cARCode.isCodeValid(sCode, sValue)) //Lets check if there are any illegal characters
                    {
                        code = cARCode.GetHexAddressFromCode(temp); //First 32bits
                        value = cARCode.GetHexValueFromCode(temp); //Last 32Bits

                        int codeType = (code >> 28) & 0x0F;
                        int dType = (code >> 24) & 0x0F;

                        cbdsOutput.SelectionColor = Color.Blue;

                        switch (codeType)
                        {
                            case 0:
                                {
                                    cbdsOutput.AppendText("2" + sCode.Remove(0, 1) + " " + sValue + "\n");
                                    break;
                                }
                            case 1:
                                {
                                    cbdsOutput.AppendText("1" + sCode.Remove(0, 1) + " " + sValue + "\n");
                                    break;
                                }
                            case 2:
                                {
                                    cbdsOutput.AppendText("0" + sCode.Remove(0, 1) + " " + sValue + "\n");
                                    break;
                                }
                            case 3:
                                {
                                    if ((value & 0xFFF00000) == 0)
                                    {
                                        switch ((code >> 16) & 0x0F)
                                        {
                                            case 0:
                                                cbdsOutput.AppendText("2" + sCode.Remove(0, 1) + " " + cARCode.GetAddressFromInt(value & 0x000000FF) + "\n");
                                                break;
                                            case 1:
                                                cbdsOutput.AppendText("1" + sCode.Remove(0, 1) + " " + cARCode.GetAddressFromInt(value & 0x0000FFFF) + "\n");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        cbdsOutput.AppendText("0" + sCode.Remove(0, 1) + " " + sValue + "\n");
                                    }
                                    break;
                                }
                            case 0x0D:
                                {
                                    switch ((code >> 16) & 0x0F)
                                    {
                                        case 0:
                                            {
                                                switch ((code >> 16) & 0x03)
                                                {
                                                    case 0:
                                                        //cbdsOutput.AppendText("==\n");
                                                        int tValue = (int)((~(value << 16)) & 0xFFFF0000);
                                                        cbdsOutput.AppendText("9" + sCode.Remove(0, 1) + " " + cARCode.GetAddressFromInt(tValue) + "\n");
                                                        break;
                                                    case 1:
                                                        cbdsOutput.AppendText("!=\n");
                                                        break;
                                                    case 2:
                                                        cbdsOutput.AppendText("<\n");
                                                        break;
                                                    case 3:
                                                        cbdsOutput.AppendText(">\n");
                                                        break;
                                                }

                                                //cbdsOutput.AppendText("16bit\n");
                                                break;
                                            }
                                        case 1:
                                            {
                                                //cbdsOutput.AppendText("8bit\n");
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
#endif
        }
    }
}
