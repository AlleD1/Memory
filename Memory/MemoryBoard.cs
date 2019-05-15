﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class MemoryBoard : Form
    {

        int firstClickedTag = 0, secondClickedTag = 0, numberOfPairsDiscovered = 0,
            numberOfCardsUp = 0, numberOfMoves = 0;

        //16st blandade dubbletparvärden
        List<int> cardIndexesShuffled = new List<int>();
        //8st blandade kort i samma ordning som cardIndexesShuffled baserat på index
        List<Image> cardImagesShuffled = new List<Image>();
        //Pictureboxarna på brädet
        List<PictureBox> pictureBoxesOnBoard = new List<PictureBox>();

        Dictionary<int, string> participators = new Dictionary<int, string>();

        public MemoryBoard()
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

        //När en PictureBox på brädet klickas
        private void PictureBoxClick(object sender, EventArgs e)
        {
            var discoveredPairs = new List<int>();

            PictureBox p = (PictureBox)sender;

            //Om alla paren INTE är hittade
            if(numberOfPairsDiscovered < 8)
            {
                //Om mindre än två kort är uppvända
                if (numberOfCardsUp < 2)
                {
                    //Om bilden i boxen är baksida
                    if (p.Image.Height == Image.FromFile("images/cards/baksida.jpg").Height)
                    {
                        //Kollar vilken siffra pictureboxarna har, dvs. ex. picbox 1 eller ex. picbox 7 osv. 
                        string pictureboxIndex = Regex.Match(p.Name, @"\d+").Value;
                        p.Image = cardImagesShuffled[int.Parse(pictureboxIndex) - 1];
                    }

                    //Sätter bilden som PictureBox p har till sendern som är på brädet
                    sender = p.Image;

                    //Om inga kort är upvända sparas första kortvärdet
                    if (numberOfCardsUp == 0)
                    {
                        firstClickedTag = int.Parse(p.Tag.ToString());
                    }
                    else
                    {
                        secondClickedTag = int.Parse(p.Tag.ToString());

                        //Om första kortvärdet är samma som andra Kortvärdet 
                        if (firstClickedTag == secondClickedTag)
                        {
                            discoveredPairs.Add(firstClickedTag);
                            numberOfPairsDiscovered++;
                        }
                    }
                    numberOfCardsUp++;
                }
                //När två kort är uppvända
                else
                {
                    //Hitta bättre jämförelse
                    foreach (PictureBox temp in pictureBoxesOnBoard)
                    {
                        if ((int.Parse(temp.Tag.ToString()) == firstClickedTag || int.Parse(temp.Tag.ToString()) == secondClickedTag) && firstClickedTag != secondClickedTag)
                        {
                            temp.Image = Image.FromFile("images/cards/baksida.jpg");
                        }
                    }
                    numberOfCardsUp = 0;
                    numberOfMoves++;
                }

            }
            //Om alla paren är hittade
            else
            {
                //Bl.a. kalla på UserInput
            }

            label2.Text = numberOfMoves.ToString();
        }

        //Ger en List med 8 blandade dubbletter 
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