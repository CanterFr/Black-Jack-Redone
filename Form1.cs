using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assign6withGUI;


namespace Assignment6CSharp
{
    public partial class mainForm : Form
    {
       
        public string[] oAllCardsArr;
        public string[] allCardsArr;
        public string flipped;
        int p1Total = 0;
        int dealerTotal = 0;
        Boolean flip = true;
        public bool arrange = false;
        public string[] p1 = new string[6];
        public string[] dealer = new string[6];
        int p1Cards = 1;
        int dealerCards = 1;
        public mainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void startGame_Click(object sender, EventArgs e)
        {
            p1Total = 0;
            dealerTotal = 0;
            p1Cards = 1;
            dealerCards = 1;
            
            flipped = (System.IO.File.ReadAllText(@"back.txt"));
            lastScreen.Text = "";
            //read in from a file and into an array of strings
            oAllCardsArr = (System.IO.File.ReadAllText(@"allCards.txt")).Split(' ').ToArray();
            allCardsArr = oAllCardsArr;

            //the shuffled card are now distributed one at a time in a round robin fashion to the four players (4 arrays) and display in their perspective p#DeckText area
            Random rnd = new Random();//rnd number seed
            string[] temp = new string[allCardsArr.Length];//make a temp based of off the size of the length of the array we are mixing
            for (int i = 0; i < 52; i++)
            {
                int card = rnd.Next(allCardsArr.Length);// creates a number between 0 and 51
                temp[i] = allCardsArr[card];//grab the value in the random position on the main array and set it to the next location on the temp array or mixed array
                allCardsArr = allCardsArr.Where(w => w != allCardsArr[card]).ToArray();//remove the value that was just used out of our main array
            }
            allCardsArr = temp;//get rid of unmixed array copy info to temp array

            for (int i =0; i<2; i++)
            {
                p1[i] = allCardsArr[i];
                allCardsArr = allCardsArr.Where(w => w != allCardsArr[i]).ToArray();
                dealer[i] = allCardsArr[i];
            }
            p1Total=fixTotal(p1);
            playerScore.Text = p1Total+"";
            dealerTotal=fixTotal(dealer);
            PictureBox[] pB = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12 };
            pB[0].Image = Image.FromFile(Environment.CurrentDirectory + "\\cardImages\\" +p1[0] + ".png"); //make relative path
                pB[1].Image = Image.FromFile(Environment.CurrentDirectory + "\\cardImages\\" + p1[1] + ".png");
            if (flip)
            {
                pB[6].Image = Image.FromFile(Environment.CurrentDirectory + "\\cardImages\\" + dealer[0] + ".png"); //make relative path
                pB[7].Image = Image.FromFile(Environment.CurrentDirectory + "\\cardImages\\" + flipped + ".png"); //make relative path
            }


        }
        private int fixTotal(string[] set)
        {
            int total = 0;
            for(int i =0; i<set.Length; i++)
            {
                string card = set[i];
                if (set[i] == null)
                    break;
                else if (card.Contains("2"))
                    total += 2;
                else if (card.Contains("3"))
                    total += 3;
                else if (card.Contains("4"))
                    total += 4;
                else if (card.Contains("5"))
                    total += 5;
                else if (card.Contains("6"))
                    total += 6;
                else if (card.Contains("7"))
                    total += 7;
                else if (card.Contains("8"))
                    total += 8;
                else if (card.Contains("9"))
                    total += 9;
                else if (card.Contains("10"))
                    total += 10;
                else if (!card.Contains("A"))
                    total += 10;
                else
                {
                    if (total + 11 > 21)
                        total += 1;
                    else
                        total += 11;
                }

            }
            return total;
        }

        private void Hit_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            PictureBox[] pB = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12 };
            int card = rnd.Next(allCardsArr.Length);
            p1[++p1Cards] = allCardsArr[card];
            allCardsArr = allCardsArr.Where(w => w != allCardsArr[card]).ToArray();
            pB[p1Cards].Image = Image.FromFile(Environment.CurrentDirectory + "\\cardImages\\" + p1[p1Cards] + ".png");

            p1Total=fixTotal(p1);
            playerScore.Text = p1Total + "";
            if (p1Total > 21) { 
                lastScreen.AppendText("You Lose!");
             }
        }

        private void StayPut_Click(object sender, EventArgs e)
        {
            PictureBox[] pB = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12 };
            pB[7].Image = Image.FromFile(Environment.CurrentDirectory + "\\cardImages\\" + dealer[1] + ".png");
            while(dealerTotal < 17)
            {
                Random rnd = new Random();
                int card = rnd.Next(allCardsArr.Length);
                dealer[++dealerCards] = allCardsArr[card];
                dealerTotal = fixTotal(dealer);
                dealerScore.Text = dealerTotal + "";
                allCardsArr = allCardsArr.Where(w => w != allCardsArr[card]).ToArray();
                pB[dealerCards+6].Image = Image.FromFile(Environment.CurrentDirectory + "\\cardImages\\" + dealer[dealerCards] + ".png");
            }

            if (dealerTotal > 21)
            {
                lastScreen.AppendText("You won!");
            }
            else
                if (dealerTotal > p1Total)
            {
                lastScreen.AppendText("You lost!");
            }
            else if (dealerTotal < p1Total)
            {
                lastScreen.AppendText("You won!");
            }
            else
            {
                lastScreen.AppendText("TIE!");
            }
        }
        private void restart_Click(object sender, EventArgs e)
        {
            PictureBox[] pB = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12 };
            for(int x=0; x<pB.Length; x++)
            {
                pB[x].Image = Image.FromFile(Environment.CurrentDirectory + "\\cardImages\\" + flipped + ".png");
            }
            p1Cards = 1;
            dealerCards = 1;
            dealerTotal = 0;
            p1Total = 0;
            string[] temp = new string[6];
            p1 = temp;
            dealer = temp;
           
        }

        private void p1Hand_TextChanged(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
