using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Memory
{
    public partial class Form1 : Form
    {
        //8st blandade dubbletpar
        List<int> cardIndexesShuffled = new List<int>();
        //8st blandade kort i samma ordning som cardIndexesShuffled baserat på index
        List<Image> cardImagesShuffled = new List<Image>();
        //De pictureboxarna på brädet
        List<PictureBox> pictureBoxesOnBoard = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
        }

        //Saker när programmet öppnar
        private void Form1_Load(object sender, EventArgs e)
        {
            //Hittar PictureBoxarna på brädet
            PictureBoxLocator();
            //Drar ut 8 kortdubbletter till spelet
            PictureSelector();
            //Sätter de tomma PictureBoxarnas bilder till kortbaksidan
            CardTurner();
        }

        private void PictureBoxClick(object sender, EventArgs e)
        {

        }

        private void PictureSelector()
        {
            var random = new Random();

            //Kort som redan är dragna
            var blacklistedCardIndexes = new List<int>();
            var cardIndexes = new List<int>();

            //Väljer 8 st unika värden mellan 1 och 52
            while(cardIndexes.Count < 8)
            {
                int randomInt = random.Next(1, 52);

                if(!blacklistedCardIndexes.Contains(randomInt))
                {
                    cardIndexes.Add(randomInt);
                    blacklistedCardIndexes.Add(randomInt);
                }
            }

            //Dubblar antalet kort indexar
            cardIndexes.AddRange(cardIndexes);
            //Blandar och matar in värdena i lista till en annan lista
            cardIndexesShuffled = cardIndexes.OrderBy(a => Guid.NewGuid()).ToList();

            //Skaffar 16 kort till spelet & ger varje picturebox på brädet en tag med värdet på resp. kort
            for(int i = 0; i < 16; i++)
            {
                cardImagesShuffled.Add(Image.FromFile(string.Format("images/cards/{0}.png", cardIndexesShuffled[i].ToString())));
                pictureBoxesOnBoard[i].Tag = cardIndexesShuffled[i].ToString();
            }
        }

        //Läser in alla PictureBoxar på brädet till en lista
        private void PictureBoxLocator()
        {
            for (int i = 0; i < 16; i++)
            {
                pictureBoxesOnBoard.Add((PictureBox)Controls.Find("pictureBox" + (i + 1).ToString(), true)[0]);
            }
        }

        //Vänder alla korten
        private void CardTurner()
        {
            foreach(PictureBox temp in pictureBoxesOnBoard)
            {
                temp.Image = Image.FromFile("images/cards/baksida.jpg");
            }
        }
    }
}
