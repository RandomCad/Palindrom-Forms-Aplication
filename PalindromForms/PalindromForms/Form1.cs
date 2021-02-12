using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalindromForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        static List<char> SchortChars = new List<char>() { 'l', 'i', 'I',',', '.', ' ', ';', ':', '-', '(', ')', '[', ']', '!', 'j', '|', '°', '²', '³' };
        List<string> BekanteStrings = new List<string>();
        private async void ChekPalindrom(object sender, EventArgs e)
        {
            string Text = RemouveTheHarmfoulParts();
            //Get Palindrome:
            List<string> Palindrome = await GetPlaindroms(Text);
            string folPalindrom = Palindrome[0];
            for (int x = 1; x < Palindrome.Count; x++)
                Palindrome[x - 1] = Palindrome[x];
            Palindrome[Palindrome.Count - 1] = folPalindrom;
            bool TherWasAChange = false;
            do
            {
                TherWasAChange = false;
                for (int x = 0; x < Palindrome.Count; x++)
                    for (int y = x + 1; y < Palindrome.Count; y++)
                        if (Palindrome[x] == Palindrome[y])
                        {
                            TherWasAChange = true;
                            Palindrome.RemoveAt(y);
                        }
            } while (TherWasAChange);
            //Now diesplay it:
            richTextBox1.Clear();
            richTextBox1.SelectionColor = Color.Black;
            richTextBox1.AppendText(Text+"\n");
            //schneide das Ende Ab in welchem es kein Palindrom mehr gibt.
            string UrsprünglicherTextCharZwi = Text;
            for (int x = Text.Length - 1; x >= 0; x--)
            {
                TherWasAChange = false;
                char kkjfdhal = Text[x];
                foreach (var item in Palindrome)
                {
                    for (int y = item.Length - 1; y >= 0; y--)
                        if (item[y] != Text[x - y])
                        {
                            TherWasAChange = true;
                            break;
                        }
                    if (!TherWasAChange)
                        break;
                }
                if (!TherWasAChange)
                    break;
                string zwi = UrsprünglicherTextCharZwi;
                UrsprünglicherTextCharZwi = "";
                for (int y = 0; y < zwi.Length - 1; y++)
                    UrsprünglicherTextCharZwi += zwi[y];
            }
            Text = "";
            foreach (var item in UrsprünglicherTextCharZwi)
                Text += item;
            for (int x = 0; x < Palindrome.Count; x++)
            {
                GetPositionOfThePalindrom(Text, Palindrome[x]);
                richTextBox1.AppendText("\n");
            }
        }
        private async Task<List<string>> GetPlaindroms(string Text)
        {
            //Beschleunigungen:
            if (Text.Length == 2)
            {
                if (Text[0] == Text[1])
                    return new List<string>() { Text };
                else return null;
            }
            //ist dieser Teil bereits bekant?
            if (BekanteStrings.Contains(Text))
                return null;
            //Soll rekursiv im Text suche:
            Task<List<string>>[] t = new Task<List<string>>[2];
            if (Text.Length > 2)
            {//recursion:
                t[0] = GetPlaindroms(Text.Remove(Text.Length - 1));
                t[1] = GetPlaindroms(Text.Remove(0, 1));
            }
            //Leichte beschleunigung
            if (Text.Length == 3)
            {
                if (Text[0] == Text[2])
                    return new List<string>() { Text };
                else return null;
            }
            //eigene berechnung:
            string[] parts = new string[2];
            for (int x = 0; x < Text.Length / 2; x++)
                parts[0] += Text[x];
            if (Text.Length % 2 == 1)
                for (int x = Text.Length - 1; x >= Text.Length / 2 + 1; x--)
                    parts[1] += Text[x];
            else
                for (int x = Text.Length - 1; x >= Text.Length / 2; x--)
                    parts[1] += Text[x];
            List<string> Palindrome = new List<string>();
            if (parts[0] == parts[1])
                Palindrome.Add(Text);
            //Recursive rückgabe:
            if (t[0] != null)
            {
                await t[0];
                if (t[0].Result != null)
                    Palindrome.AddRange(t[0].Result.ToArray());
                BekanteStrings.Add(Text.Remove(Text.Length - 1));
            }
            if (t[1] != null)
            {
                await t[1];
                if (t[1].Result != null)
                    Palindrome.AddRange(t[1].Result.ToArray());
                BekanteStrings.Add(Text.Remove(0, 1));
            }
            return Palindrome;
        }
        private void GetPositionOfThePalindrom(string Text, string Palindrom)
        {
            //if The hole is a Palindrom:
            if (Palindrom == Text)
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText(Palindrom);
            }
            for (int x = 0; x < Text.Length-Palindrom.Length; x++)
            {
                bool isWrong = false;
                for (int y = 0; y < Palindrom.Length; y++)
                    if (Palindrom[y] != Text[x + y]) 
                    {
                        isWrong = true;
                        break;
                    }
                if (isWrong)
                {
                    richTextBox1.SelectionColor = Color.White;
                    richTextBox1.AppendText(Text[x] + "");
                    continue;
                }
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText(Palindrom);
                x += Palindrom.Length - 1;
            }
        }
        private string RemouveTheHarmfoulParts()
        {
            string[] HarmfoulParts = new string[] { " ", "-", ".", ",", ";", ":", "_", "!", "'", '"' + "", "&", "%", "(", ")", "=", "?","\n","\t","\r" };
            string Text = textBox1.Text.ToLower();
            foreach (var item in HarmfoulParts)
                Text.Replace(item, "");
            return Text;


        }
    }
}
