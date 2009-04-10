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

                        switch (code >> 28)
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
