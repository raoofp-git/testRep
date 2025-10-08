using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GeneralTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string XmlPath = @"\\192.168.1.214\Shared\temp\Else\ContrastTemp\";
            string XmlFileName = "main.xml";
            string ErrString = "";
            string Journal = "conzzz";
            FileStream ObjFile = File.Open(Path.Combine(XmlPath, XmlFileName), FileMode.Open, FileAccess.Read, FileShare.Read);
            using (StreamReader ObjFs = new StreamReader(ObjFile))
            {
                if (ObjFile.CanRead)
                {
                    string Inputdata = ObjFs.ReadLine();

                    // Check for XML DTD version
                    if (Inputdata != null && Inputdata.IndexOf("DTD VERSION", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        int StPos = Inputdata.IndexOf("DTD VERSION", StringComparison.OrdinalIgnoreCase);
                        int EndPos = Inputdata.IndexOf("//", StPos, StringComparison.OrdinalIgnoreCase);
                        string artver = Inputdata.Substring(StPos + 11, EndPos - StPos - 11).Trim();

                        if (string.IsNullOrEmpty(artver))
                        {
                            ErrString += "Missing Version Tag.";
                        }
                    }
                    if (Inputdata != null && Inputdata.IndexOf("<Jid>", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        string Temp_Jid = GetStrInTags(Inputdata, "<jid>", "</jid>");
                        
                        if (Temp_Jid.ToUpper().Contains("BS:"))
                        {
                            Temp_Jid = Temp_Jid.Replace("BS:", "");
                        }
                              
                        if (Journal.ToUpper().Trim()=="CONZZZ")
                        {
                            if (Temp_Jid.ToUpper().Trim() == "CON")
                            {
                                Temp_Jid = "CONZZZ";
                            }
                        }

                        
                            if (!Temp_Jid.Trim().Equals(Journal.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                ErrString += "Journal Name not matching In Xml.";
                                // test commet for git    
                            }
                        
                    }
                    else
                    {
                        ErrString += "Missing <Jid> Tag.";
                    }

                }
            }
        }
        public string GetStrInTags(string fullstr, string StTag, string EndTag)
        {
            try
            {
                int pos1st_quo = 0;
                int pos2nd_quo = 0;
                string ret_var = string.Empty;

                // Initialize return value
                string getStrInTags = string.Empty;

                // Find the position of the start tag
                if (fullstr.IndexOf(StTag, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    pos1st_quo = fullstr.IndexOf(StTag, StringComparison.OrdinalIgnoreCase);
                }

                // Find the position of the end tag
                if (pos1st_quo > 0)
                {
                    if (fullstr.IndexOf(EndTag, pos1st_quo, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        pos2nd_quo = fullstr.IndexOf(EndTag, pos1st_quo, StringComparison.OrdinalIgnoreCase);
                    }
                }

                // Extract the value between the tags
                if (pos1st_quo > 0 && pos2nd_quo > 0)
                {
                    if (StTag.IndexOf(">", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ret_var = fullstr.Substring(pos1st_quo + StTag.Length, pos2nd_quo - pos1st_quo - StTag.Length).Trim();
                    }
                    else
                    {
                        int innerStart = fullstr.IndexOf(">", pos1st_quo, StringComparison.OrdinalIgnoreCase) + 1;
                        ret_var = fullstr.Substring(innerStart, pos2nd_quo - innerStart).Trim();
                    }

                    getStrInTags = ret_var;
                }

                return getStrInTags;
            }
            catch (Exception ex)
            {
                //UpdateError($"Error In GetStrInTags Module - {ex.Message}", "", false, "", "", "", "", "", "", 0, false);
                return string.Empty;
            }
        }

    }
}
