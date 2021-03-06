﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Memory
{
    public partial class MemoryBoard : Form
    {
        int firstClickedTag = 0, secondClickedTag = 0, numberOfPairsDiscovered = 0,
            numberOfCardsUp = 0, numberOfMoves = 0;

        bool participatorIsAdded = false;

        //16st blandade dubbletparvärden
        List<int> cardIndexesShuffled = new List<int>();
        //8st blandade kort i samma ordning som cardIndexesShuffled baserat på index
        List<Image> cardImagesShuffled = new List<Image>();
        //Pictureboxarna på brädet
        List<PictureBox> pictureBoxesOnBoard = new List<PictureBox>();
        //Lista med antalet drag för varje omgång
        List<int> finalScores = new List<int>();

        //Deltagarnas drag och namn
        Dictionary<int, string> participators = new Dictionary<int, string>();

        DialogResult result;

        public MemoryBoard()
        {
            InitializeComponent();
        }

        //Saker när programmet öppnar
        private void MemoryBoard_Load(object sender, EventArgs e)
        {
            //Hittar PictureBoxarna på brädet
            PictureBoxLocator();
            //Drar ut 8 kortdubbletter till spelet
            PictureSelector();
            //Sätter de tomma PictureBoxarnas bilder till kortbaksidan
            CardTurner();
        }

        //Reseta värdena för ny omgång
        private void Restart(object sender, EventArgs e)
        {
            //Skuggar knappen
            button1.Enabled = false;
            button2.Enabled = false;

            //Int
            firstClickedTag = 0;
            secondClickedTag = 0;
            numberOfPairsDiscovered = 0;
            numberOfCardsUp = 0;
            numberOfMoves = 0;

            //Bool
            participatorIsAdded = false;

            //List
            cardIndexesShuffled.Clear();
            cardImagesShuffled.Clear();

            //Metoder
            PictureSelector();
            CardTurner();

            //Dialogresultat
            result = DialogResult.Cancel;
        }

        private void UpdateScoreboard(object sender, EventArgs e)
        {
            var finalScoresSorted = new List<int>();

            if (finalScores.Count == 0)
            {
                return;
            }

            //Rensar scoreboarden för ny omgång
            label3.Text = "";

            //Sorterar poängen
            finalScoresSorted = Sorter.QuickSort(finalScores, 0, finalScores.Count - 1);

            foreach (int temp in finalScoresSorted)
            {
                participators.TryGetValue(temp, out string name);
                label3.Text += string.Format("Drag: {0} Namn: {1}\n", temp, name);
            }

            button2.Enabled = false;
        }

        //När en PictureBox på brädet klickas
        private void PictureBoxClick(object sender, EventArgs e)
        {
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
                    //Om ett redan vänt kort klickas
                    else
                    {
                        return;
                    }

                    //Sätter bilden som PictureBox p fick till sendern som är på brädet
                    sender = p.Image;

                    //Om inga kort är upvända sparas första kortvärdet
                    if (numberOfCardsUp == 0)
                    {
                        firstClickedTag = int.Parse(p.Tag.ToString());
                    }
                    else
                    {
                        secondClickedTag = int.Parse(p.Tag.ToString());

                        //Om första kortvärdet är samma som andra kortvärdet 
                        if (firstClickedTag == secondClickedTag)
                        {
                            numberOfPairsDiscovered++;
                        }
                    }
                    numberOfCardsUp++;
                }
                //Om två kort är uppvända
                else
                {
                    foreach (PictureBox temp in pictureBoxesOnBoard)
                    {
                        //Om det första och andra PictureBox tagen INTE är samma -> INTE samma kort
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
                if (result != DialogResult.OK)
                {
                    //Bl.a. kalla på UserInput
                    var userInput = new UserInput();
                    result = userInput.ShowDialog();

                    //Om godtagbart värde returneras
                    if (result == DialogResult.OK)
                    {
                        //Kollar om samma antal drag redan finns med i scoreboarden
                        for(int i = 0; participatorIsAdded == false; i++)
                        {
                            //Om dubblett finns går in i "finally" blocket 
                            try
                            {
                                participators.Add(numberOfMoves + i, userInput.Name);
                                participatorIsAdded = true;
                            }
                            finally
                            {}
                        }
                        finalScores.Add(numberOfMoves);
                        button1.Enabled = true;
                        button2.Enabled = true;
                    }
                }
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
