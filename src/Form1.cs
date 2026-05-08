using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe_Finale
{
    public partial class Form1 : Form
    {
        public int spieler = 2;
        public int zug = 0;
        public int sp_x = 0;
        public int sp_o = 0;
        public int sd = 0;
        public bool pveModus = false;

        public int schwierigkeit = 1; // 1 = Leicht, 2 = Schwer
        public int startSpieler = 2;  // 2 = X, 1 = O

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            labelX.Text = "Spieler X: " + sp_x;
            labelO.Text = "Spieler O: " + sp_o;
            labelG.Text = "Gleichstand: " + sd;
        }

        bool IsWinner()
        {
            // horizontal
            if ((button1.Text == button2.Text) && (button2.Text == button3.Text) && button1.Text != "")
                return true;
            if ((button4.Text == button5.Text) && (button5.Text == button6.Text) && button4.Text != "")
                return true;
            if ((button7.Text == button8.Text) && (button8.Text == button9.Text) && button7.Text != "")
                return true;

            // vertikal
            if ((button1.Text == button4.Text) && (button4.Text == button7.Text) && button1.Text != "")
                return true;
            if ((button2.Text == button5.Text) && (button5.Text == button8.Text) && button2.Text != "")
                return true;
            if ((button3.Text == button6.Text) && (button6.Text == button9.Text) && button3.Text != "")
                return true;

            // diagonal
            if ((button1.Text == button5.Text) && (button5.Text == button9.Text) && button1.Text != "")
                return true;
            if ((button3.Text == button5.Text) && (button5.Text == button7.Text) && button3.Text != "")
                return true;

            return false;

        }
        bool IsDraw()
        {
            return (zug == 9 && !IsWinner());
        }

        private void ButtonsClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Text == "")
            {
                if (spieler % 2 == 0)
                {
                    button.Text = "X";
                    button.ForeColor = Color.Red;
                }
                else
                {
                    button.Text = "O";
                    button.ForeColor = Color.Blue;
                }

                zug++;

                // Gewinner prüfen
                if (IsWinner())
                {
                    if (button.Text == "X")
                    {
                        MessageBox.Show("Spieler X hat gewonnen");
                        sp_x++;
                    }
                    else
                    {
                        MessageBox.Show("Spieler O hat gewonnen");
                        sp_o++;
                    }

                    NeuesSpiel();
                    return;
                }
                // Unentschieden prüfen
                else if (IsDraw())
                {
                    MessageBox.Show("Unentschieden");
                    sd++;
                    NeuesSpiel();
                }
                else    //Für Bot
                {
                    spieler++;
                    // Wenn Computer-Modus an ist und O dran ist
                    if (pveModus && spieler % 2 != 0)
                    {
                        ComputerZieht();
                    }
                }
            }
        }
        private void NeuesSpiel_Click(object sender, EventArgs e)
        {
            NeuesSpiel();
        }

        public void NeuesSpiel()
        {
            spieler = startSpieler;
            zug = 0;

            button1.Text = "";
            button2.Text = "";
            button3.Text = "";
            button4.Text = "";
            button5.Text = "";
            button6.Text = "";
            button7.Text = "";
            button8.Text = "";
            button9.Text = "";

            labelX.Text = "Spieler X:   " + sp_x;
            labelO.Text = "Spieler O:    " + sp_o;
            labelG.Text = "Gleichstand:     " + sd;

            if (pveModus && spieler % 2 != 0) 
                ComputerZieht(); // Für Bot
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Alles zurücksetzen?", "Reset", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                sp_x = sp_o = sd = 0;
                NeuesSpiel();
            }
        }
        private void werBeginnt(object sender, EventArgs e)
        {
            if (startSpieler == 2)
            {
                startSpieler = 1;
                MessageBox.Show("Spieler O beginnt jetzt.");
            }
            else
            {
                startSpieler = 2;
                MessageBox.Show("Spieler X beginnt jetzt.");
            }

            NeuesSpiel();

        }

        private void btnBeenden_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Willst du das Spiel wirklich beenden?", "Beenden", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void Modus_Click(object sender, EventArgs e)
        {
            if (pveModus)
            {
                pveModus = false;
            }
            else
            {
                pveModus = true;
            }

            if (pveModus)
            {
                MessageBox.Show("Modus: Spieler vs Computer");
            }
            else
            {
                MessageBox.Show("Modus: Spieler vs Spieler");
            }

            NeuesSpiel();
        }

        private void ComputerZieht()
        {
            // 1. Taktik: Kann ich gewinnen oder muss ich blocken? (Nur bei Schwierigkeit 2)
            if (schwierigkeit >= 2)
            {
                Button test = FindeFeld("O"); // Erst schauen ob O gewinnen kann
                if (test == null) test = FindeFeld("X"); // Dann schauen ob X geblockt werden muss

                if (test != null) { ButtonsClick(test, EventArgs.Empty); return; }
            }

            // 2. Zufall: Einfach das nächste freie Feld nehmen
            if (button1.Text == "") { ButtonsClick(button1, EventArgs.Empty); }
            else if (button2.Text == "") { ButtonsClick(button2, EventArgs.Empty); }
            else if (button3.Text == "") { ButtonsClick(button3, EventArgs.Empty); }
            else if (button4.Text == "") { ButtonsClick(button4, EventArgs.Empty); }
            else if (button5.Text == "") { ButtonsClick(button5, EventArgs.Empty); }
            else if (button6.Text == "") { ButtonsClick(button6, EventArgs.Empty); }
            else if (button7.Text == "") { ButtonsClick(button7, EventArgs.Empty); }
            else if (button8.Text == "") { ButtonsClick(button8, EventArgs.Empty); }
            else if (button9.Text == "") { ButtonsClick(button9, EventArgs.Empty); }
        }

        // Hilfsfunktion für die Taktik (prüft Reihen auf 2 gleiche Steine)
        private Button FindeFeld(string s)
        {
            // Horizontal
            if (button1.Text == s && button2.Text == s && button3.Text == "") return button3;
            if (button1.Text == s && button3.Text == s && button2.Text == "") return button2;
            if (button2.Text == s && button3.Text == s && button1.Text == "") return button1;

            if (button4.Text == s && button5.Text == s && button6.Text == "") return button6;
            if (button4.Text == s && button6.Text == s && button5.Text == "") return button5;
            if (button5.Text == s && button6.Text == s && button4.Text == "") return button4;

            if (button7.Text == s && button8.Text == s && button9.Text == "") return button9;
            if (button7.Text == s && button9.Text == s && button8.Text == "") return button8;
            if (button8.Text == s && button9.Text == s && button7.Text == "") return button7;

            // Vertikal
            if (button1.Text == s && button4.Text == s && button7.Text == "") return button7;
            if (button1.Text == s && button7.Text == s && button4.Text == "") return button4;
            if (button4.Text == s && button7.Text == s && button1.Text == "") return button1;

            if (button2.Text == s && button5.Text == s && button8.Text == "") return button8;
            if (button2.Text == s && button8.Text == s && button5.Text == "") return button5;
            if (button5.Text == s && button8.Text == s && button2.Text == "") return button2;

            if (button3.Text == s && button6.Text == s && button9.Text == "") return button9;
            if (button3.Text == s && button9.Text == s && button6.Text == "") return button6;
            if (button6.Text == s && button9.Text == s && button3.Text == "") return button3;

            // Diagonal
            if (button1.Text == s && button5.Text == s && button9.Text == "") return button9;
            if (button1.Text == s && button9.Text == s && button5.Text == "") return button5;
            if (button5.Text == s && button9.Text == s && button1.Text == "") return button1;

            if (button3.Text == s && button5.Text == s && button7.Text == "") return button7;
            if (button3.Text == s && button7.Text == s && button5.Text == "") return button5;
            if (button5.Text == s && button7.Text == s && button3.Text == "") return button3;

            return null;
        }

        private void Schwierigkeits_Click(object sender, EventArgs e)
        {
            if (schwierigkeit == 1)
            {
                schwierigkeit = 2;
                MessageBox.Show("Schwierigkeit: Mittel (Computer blockt jetzt)");
            }
            else
            {
                schwierigkeit = 1;
                MessageBox.Show("Schwierigkeit: Leicht (Computer zieht zufällig)");
            }

            NeuesSpiel();
        }
    }
}
