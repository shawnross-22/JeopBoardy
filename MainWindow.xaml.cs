using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace JeopBoardy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<System.Windows.Controls.Button> btnQs = new List<System.Windows.Controls.Button>();
        public List<System.Windows.Controls.Button> btnPs = new List<System.Windows.Controls.Button>();
        public List<TextBlock> tbCats = new List<TextBlock>();
        public List<System.Windows.Controls.TextBox> txtPlayers = new List<System.Windows.Controls.TextBox>();
        public List<string> questions = new List<string>();
        public Dictionary<int, List<string>> clues = new Dictionary<int, List<string>>();
        public System.Windows.Controls.Button clickedQuestion;
        public List<int> NAs = new List<int>();
        public bool DD = false;
        public bool boolDouble = false;
        public int count = 0;
        public bool gameSelected = false;
        public int gameIndex = 0;
        public bool final = false;
        public bool wagers = false;
        public bool scoreChange = false;
        public bool correctScore = false;
        public System.Windows.Controls.Button control;
        public System.Windows.Controls.Button min;
        public System.Windows.Controls.Button mid;
        public System.Windows.Controls.Button max;
        public int turn = 0;
        System.Windows.Controls.Button clickedScore;


        public MainWindow()
        {
            InitializeComponent();
            tbCats.Add(tbCategory1); tbCats.Add(tbCategory2); tbCats.Add(tbCategory3); tbCats.Add(tbCategory4); tbCats.Add(tbCategory5); tbCats.Add(tbCategory6);
            btnQs.Add(btn11); btnQs.Add(btn21); btnQs.Add(btn31); btnQs.Add(btn41); btnQs.Add(btn51); btnQs.Add(btn61);
            btnQs.Add(btn12); btnQs.Add(btn22); btnQs.Add(btn32); btnQs.Add(btn42); btnQs.Add(btn52); btnQs.Add(btn62);
            btnQs.Add(btn13); btnQs.Add(btn23); btnQs.Add(btn33); btnQs.Add(btn43); btnQs.Add(btn53); btnQs.Add(btn63);
            btnQs.Add(btn14); btnQs.Add(btn24); btnQs.Add(btn34); btnQs.Add(btn44); btnQs.Add(btn54); btnQs.Add(btn64);
            btnQs.Add(btn15); btnQs.Add(btn25); btnQs.Add(btn35); btnQs.Add(btn45); btnQs.Add(btn55); btnQs.Add(btn65);
            btnPs.Add(btnScore1); btnPs.Add(btnScore2); btnPs.Add(btnScore3); btnPs.Add(btnNoAnswer);
            txtPlayers.Add(txtPlayer1); txtPlayers.Add(txtPlayer2); txtPlayers.Add(txtPlayer3);
            control = btnScore1;
            tbChargePlayer.Text = txtPlayers[btnPs.IndexOf(control)].Text;

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start("explorer.exe");
            string fileSelected = $"C:/Users/smr04/source/repos/JeopBoardy/data/jData.txt";
            //using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            //{
            //    openFileDialog1.InitialDirectory = "c:\\";
            //    openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //    openFileDialog1.FilterIndex = 2;
            //    openFileDialog1.RestoreDirectory = true;
            //    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        fileSelected = openFileDialog1.FileName;
            //    }
            //}
            StreamReader sr = null;
            if (File.Exists(fileSelected))
            {
                sr = new StreamReader(File.OpenRead(fileSelected), Encoding.GetEncoding("iso-8859-1"));
                Dictionary<string, string[]> Questions = new Dictionary<string, string[]>();
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    questions.AddRange(line.Split('|'));
                }
                txtGame.Visibility = Visibility.Visible;
                btnGame.Visibility = Visibility.Visible;
                btnGame.IsEnabled = true;
            }
            else
            {
                System.Windows.MessageBox.Show("File doesn't exist");
            }


        }

        private void btnQ_Click(object sender, RoutedEventArgs e)
        {
            clickedQuestion = sender as System.Windows.Controls.Button;
            btnFixScore.IsEnabled = false;
            tbQuestion.Visibility = Visibility.Visible;
            tbQuestion.FontSize = 40;
            if (!boolDouble)
            {
                if (clues[btnQs.IndexOf(clickedQuestion)][2].Substring(0, 1) == "1")
                {
                    DD = true;
                    tbDD.Visibility = Visibility.Visible;
                    txtDD.Visibility = Visibility.Visible;
                    btnNoAnswer.Content = "Make Wager";
                    tbQuestion.Text = "";
                    System.Windows.MessageBox.Show("Answer!");
                    wagers = true;
                    btnNoAnswer.IsEnabled = true;
                    txtDD.Text = "";
                    txtDD.IsEnabled = true;
                    btnFixScore.IsEnabled = false;
                }
                
            }
            else
            {              
                if (clues[30 + btnQs.IndexOf(clickedQuestion)][2].Substring(0, 1) == "1")
                {
                    tbQuestion.Text = "";
                    DD = true;
                    txtDD.Text = "";
                    tbDD.Visibility = Visibility.Visible;
                    txtDD.Visibility = Visibility.Visible;                  
                    System.Windows.MessageBox.Show("Answer!");
                    wagers = true;
                    btnNoAnswer.IsEnabled = true;
                    btnNoAnswer.Content = "Make Wager";
                    txtDD.IsEnabled = true;
                }

            }

            if (!wagers)
            {
                if (boolDouble)
                {
                    tbQuestion.Text = clues[30 + btnQs.IndexOf(clickedQuestion)][0];
                    tbAnswer.Text = clues[30 + btnQs.IndexOf(clickedQuestion)][1];
                }
                else
                {
                    tbQuestion.Text = clues[btnQs.IndexOf(clickedQuestion)][0];
                    tbAnswer.Text = clues[btnQs.IndexOf(clickedQuestion)][1];
                }
                if (tbQuestion.Text.Length > 250)
                {
                    tbQuestion.FontSize = 30;
                }
                foreach (var button in btnPs)
                {
                    button.IsEnabled = true;
                }
            }
            
 
        }          


        private void btnScore_Click(object sender, RoutedEventArgs e)
        {
            clickedScore = sender as System.Windows.Controls.Button;
            btnFixScore.IsEnabled = true;
            if (!scoreChange)
            {
                if (DD)
                {
                    if (!wagers)
                    {
                        DD = false;
                        txtDD.Visibility = Visibility.Hidden;
                        tbDD.Visibility = Visibility.Hidden;
                        tbQuestion.Visibility = Visibility.Hidden;
                        tbAnswer.Text = "";
                        clickedScore.Content = Convert.ToString(Convert.ToInt32(clickedScore.Content) + Convert.ToInt32(txtDD.Text));
                        control.IsEnabled = false;
                        clickedQuestion.IsEnabled = false;
                        clickedQuestion.Visibility = Visibility.Hidden;
                        count += 1;
                        control = clickedScore;
                        tbChargePlayer.Text = txtPlayers[btnPs.IndexOf(control)].Text;
                        if (count == 30)
                        {
                            doubleJeopardy();
                            min = btnScore1;
                            for (int i = 0; i < 3; i++)
                            {
                                if (Convert.ToInt32(btnPs[i].Content) < Convert.ToInt32(min.Content))
                                {
                                    min = btnPs[i];
                                }
                            }
                            control = min;
                            tbChargePlayer.Text = txtPlayers[btnPs.IndexOf(control)].Text;
                        }
                        if (count == 60)
                        {
                            finalJeopardy();
                        }
                    }
                    else
                    {
                        if (clickedScore == btnNoAnswer && txtDD.Text != "")
                        {
                            if (Convert.ToInt32(txtDD.Text) >= 0)
                            {
                                if ((Convert.ToInt32(control.Content) <= 1000 && Convert.ToInt32(txtDD.Text) <= 1000) || (boolDouble && Convert.ToInt32(control.Content) <= 2000 && Convert.ToInt32(txtDD.Text) <=2000))
                                {
                                    if (boolDouble)
                                    {
                                        tbQuestion.Text = tbQuestion.Text = clues[30 + btnQs.IndexOf(clickedQuestion)][0];
                                        tbAnswer.Text = clues[30 + btnQs.IndexOf(clickedQuestion)][1];
                                    }
                                    else
                                    {
                                        tbQuestion.Text = tbQuestion.Text = clues[btnQs.IndexOf(clickedQuestion)][0];
                                        tbAnswer.Text = clues[btnQs.IndexOf(clickedQuestion)][1];
                                    }
                                    if (tbQuestion.Text.Length > 250)
                                    {
                                        tbQuestion.FontSize = 30;
                                    }
                                    wagers = false;
                                    control.IsEnabled = true;
                                    btnNoAnswer.IsEnabled = false;
                                    btnFixScore.IsEnabled = false;
                                    btnNoAnswer.Content = "No Answer";
                                    txtDD.IsEnabled = false;
                                }
                                else if (Convert.ToInt32(control.Content) > 1000 && Convert.ToInt32(txtDD.Text) <= Convert.ToInt32(control.Content))
                                {
                                    if (boolDouble)
                                    {
                                        tbQuestion.Text = tbQuestion.Text = clues[30 + btnQs.IndexOf(clickedQuestion)][0];
                                        tbAnswer.Text = clues[30 + btnQs.IndexOf(clickedQuestion)][1];
                                    }
                                    else
                                    {
                                        tbQuestion.Text = tbQuestion.Text = clues[btnQs.IndexOf(clickedQuestion)][0];
                                        tbAnswer.Text = clues[btnQs.IndexOf(clickedQuestion)][1];
                                    }
                                    if (tbQuestion.Text.Length > 250)
                                    {
                                        tbQuestion.FontSize = 30;
                                    }
                                    wagers = false;
                                    control.IsEnabled = true;
                                    btnNoAnswer.IsEnabled = false;
                                    btnFixScore.IsEnabled = false;
                                    btnNoAnswer.Content = "No Answer";
                                    txtDD.IsEnabled = false;
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Wager is invalid.");
                                }
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Wager is invalid.");
                            }
                        }
                    }
                }
                else if (final)
                {
                    if (turn == 1)
                    {
                        if (Convert.ToInt32(txtDD.Text) < 0 || Convert.ToInt32(txtDD.Text) > Convert.ToInt32(min.Content))
                        {
                            System.Windows.MessageBox.Show("Wager is invalid.");
                        }
                        else
                        {
                            clickedScore.Content = Convert.ToString(Convert.ToInt32(clickedScore.Content) + Convert.ToInt32(txtDD.Text));
                            if (btnPs.Count() > 2)
                            {
                                turn += 1;
                                min.IsEnabled = false;
                                mid.IsEnabled = true;
                                tbDD.Text = txtPlayers[btnPs.IndexOf(mid)].Text + "'s Wager";
                            }
                            else
                            {
                                turn += 2;
                                min.IsEnabled = false;
                                max.IsEnabled = true;
                                tbDD.Text = txtPlayers[btnPs.IndexOf(max)].Text + "'s Wager";
                            }
                        }
                    }
                    else if (turn == 2)
                    {
                        if (Convert.ToInt32(txtDD.Text) < 0 || Convert.ToInt32(txtDD.Text) > Convert.ToInt32(mid.Content))
                        {
                            System.Windows.MessageBox.Show("Wager is invalid.");
                        }
                        else
                        {
                            mid.Content = Convert.ToString(Convert.ToInt32(mid.Content) + Convert.ToInt32(txtDD.Text));
                            turn += 1;
                            mid.IsEnabled = false;
                            max.IsEnabled = true;
                            tbDD.Text = txtPlayers[btnPs.IndexOf(max)].Text + "'s Wager";
                        }
                    }
                    else if (turn == 3)
                    {
                        if (Convert.ToInt32(txtDD.Text) < 0 || Convert.ToInt32(txtDD.Text) > Convert.ToInt32(max.Content))
                        {
                            System.Windows.MessageBox.Show("Wager is invalid.");
                        }
                        else
                        {
                            max.Content = Convert.ToString(Convert.ToInt32(max.Content) + Convert.ToInt32(txtDD.Text));
                            System.Windows.Controls.TextBox winner = txtPlayers[0];
                            foreach (var player in txtPlayers)
                            {
                                if (Convert.ToInt32(btnPs[txtPlayers.IndexOf(player)].Content) > Convert.ToInt32(btnPs[txtPlayers.IndexOf(winner)].Content))
                                {
                                    winner = player;
                                }
                            }
                            System.Windows.MessageBox.Show(winner.Text + " is the winner!");
                            foreach (var btn in btnPs)
                            {
                                btn.IsEnabled = false;
                            }
                        }
                    }

                    if (wagers)
                    {
                        btnFixScore.IsEnabled = false;
                        tbQuestion.Visibility = Visibility.Visible;
                        tbQuestion.Text = questions[gameIndex + 484];
                        if (tbQuestion.Text.Length > 250)
                        {
                            tbQuestion.FontSize = 30;
                        }
                        tbAnswer.Text = questions[gameIndex + 485];
                        txtDD.Text = "0";
                        txtDD.Visibility = Visibility.Visible;
                        txtDD.IsEnabled = true;
                        tbDD.Visibility = Visibility.Visible;
                        tbDD.Text = txtPlayers[btnPs.IndexOf(min)].Text + "'s Wager";
                        btnNoAnswer.IsEnabled = false;
                        wagers = false;
                        btnNoAnswer.Visibility = Visibility.Hidden;
                        min.IsEnabled = true;
                        turn = 1;
                    }

                }
                else
                {
                    tbQuestion.Visibility = Visibility.Hidden;
                    tbAnswer.Text = "";
                    if (Convert.ToString(clickedScore.Content) != "No Answer")
                    {
                        clickedScore.Content = Convert.ToString(Convert.ToInt32(clickedScore.Content) + Convert.ToInt32(clickedQuestion.Content));
                        control = clickedScore;
                        tbChargePlayer.Text = txtPlayers[btnPs.IndexOf(control)].Text;
                    }
                    foreach (var button in btnPs)
                    {
                        button.IsEnabled = false;
                    }
                    clickedQuestion.IsEnabled = false;
                    clickedQuestion.Visibility = Visibility.Hidden;
                    count += 1;
                    if (count == 30)
                    {
                        doubleJeopardy();
                        min = btnScore1;
                        for (int i = 0; i < 3; i++)
                        {
                            if (Convert.ToInt32(btnPs[i].Content) < Convert.ToInt32(min.Content))
                            {
                                min = btnPs[i];
                            }
                        }
                        control = min;
                        tbChargePlayer.Text = txtPlayers[btnPs.IndexOf(control)].Text;

                    }
                    if (count == 60)
                    {
                        finalJeopardy();
                    }
                }
            }
            else
            {
                tbDD.Text = "Enter Correct Score";
                txtDD.Visibility = Visibility.Visible;
                tbDD.Visibility = Visibility.Visible;
                foreach (var button in btnPs)
                {
                    button.IsEnabled = false;
                }
                correctScore = true;
            }          
        }

        private void btnScoreDown_Click(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Button clickedScore = sender as System.Windows.Controls.Button;
            if (DD)
            {
                clickedScore.Content = Convert.ToString(Convert.ToInt32(clickedScore.Content) - Convert.ToInt32(txtDD.Text));
                btnFixScore.IsEnabled = true;
                DD = false;
                txtDD.Visibility = Visibility.Hidden;
                tbDD.Visibility = Visibility.Hidden;
                tbQuestion.Visibility = Visibility.Hidden;
                tbAnswer.Text = "";
                foreach (var button in btnPs)
                {
                    button.IsEnabled = false;
                }
                clickedQuestion.IsEnabled = false;
                clickedQuestion.Visibility = Visibility.Hidden;
                count += 1;
                if (count == 30)
                {
                    doubleJeopardy();
                    min = btnScore1;
                    for (int i = 0; i < 3; i++)
                    {
                        if (Convert.ToInt32(btnPs[i].Content) < Convert.ToInt32(min.Content))
                        {
                            min = btnPs[i];
                        }
                    }
                    control = min;
                    tbChargePlayer.Text = txtPlayers[btnPs.IndexOf(clickedScore)].Text;
                }
                if (count == 60)
                {
                    finalJeopardy();
                }
            }
            else if (final)
            {
                if (turn == 1)
                {
                    if (Convert.ToInt32(txtDD.Text) < 0 || Convert.ToInt32(txtDD.Text) > Convert.ToInt32(min.Content))
                    {
                        System.Windows.MessageBox.Show("Wager is invalid.");
                    }
                    else
                    {
                        min.Content = Convert.ToString(Convert.ToInt32(min.Content) - Convert.ToInt32(txtDD.Text));
                        if (btnPs.Count() > 2)
                        {
                            turn += 1;
                            min.IsEnabled = false;
                            mid.IsEnabled = true;
                            tbDD.Text = txtPlayers[btnPs.IndexOf(mid)].Text + "'s Wager";
                        }
                        else
                        {
                            turn += 2;
                            min.IsEnabled = false;
                            max.IsEnabled = true;
                            tbDD.Text = txtPlayers[btnPs.IndexOf(max)].Text + "'s Wager";
                        }
                    }
                }
                else if (turn == 2)
                {
                    if (Convert.ToInt32(txtDD.Text) < 0 || Convert.ToInt32(txtDD.Text) > Convert.ToInt32(mid.Content))
                    {
                        System.Windows.MessageBox.Show("Wager is invalid.");
                    }
                    else
                    {
                        mid.Content = Convert.ToString(Convert.ToInt32(mid.Content) - Convert.ToInt32(txtDD.Text));
                        turn += 1;
                        mid.IsEnabled = false;
                        max.IsEnabled = true;
                        tbDD.Text = txtPlayers[btnPs.IndexOf(max)].Text + "'s Wager";
                    }
                }
                else
                {
                    if (Convert.ToInt32(txtDD.Text) < 0 || Convert.ToInt32(txtDD.Text) > Convert.ToInt32(max.Content))
                    {
                        System.Windows.MessageBox.Show("Wager is invalid.");
                    }
                    else
                    {
                        max.Content = Convert.ToString(Convert.ToInt32(max.Content) - Convert.ToInt32(txtDD.Text));
                        System.Windows.Controls.TextBox winner = txtPlayer1;
                        foreach (var player in txtPlayers)
                        {
                            if (Convert.ToInt32(btnPs[txtPlayers.IndexOf(player)].Content) > Convert.ToInt32(btnPs[txtPlayers.IndexOf(player)].Content))
                            {
                                winner = player;
                            }
                        }
                        System.Windows.MessageBox.Show(winner.Text + " is the winner!");
                        foreach (var btn in btnPs)
                        {
                            btn.IsEnabled = false;
                        }
                    }
                }
            }
            else
            {
                clickedScore.Content = Convert.ToString(Convert.ToInt32(clickedScore.Content) - Convert.ToInt32(clickedQuestion.Content));
                clickedScore.IsEnabled = false;
            }
        }

        private void doubleJeopardy()
        {
            boolDouble = true;
            count += NAs.Count();
            for (int i = 0; i < 6; i++)
            {
                tbCats[i].Text = questions[(8 * i) + gameIndex + 242];
            }
            for(int i = 0; i < 30; i++)
            {
                btnQs[i].Content = Convert.ToString(Convert.ToInt32(btnQs[i].Content) * 2);
                if (!NAs.Contains(i+30))
                {
                    btnQs[i].IsEnabled = true;
                    btnQs[i].Visibility = Visibility.Visible;
                }
                
            }
        }

        private void finalJeopardy()
        {
            btnPs.Remove(btnPs[3]);
            for (int i = 2; i > -1; i--)
            {
                if (Convert.ToInt32(btnPs[i].Content) <= 0)
                {
                    btnPs.Remove(btnPs[i]);
                    txtPlayers.Remove(txtPlayers[i]);
                }
            }
            final = true;



            if (btnPs.Count()==0)
            {
                System.Windows.MessageBox.Show("Everyone loses!");
            }
            else
            {
                min = btnPs[0];
                max = btnPs[0];
                if (btnPs.Count() > 1)
                {
                    for (int i = 1; i < btnPs.Count(); i++)
                    {
                        if (Convert.ToInt32(btnPs[i].Content) < Convert.ToInt32(min.Content))
                        {
                            min = btnPs[i];
                        }
                        if (Convert.ToInt32(btnPs[i].Content) >= Convert.ToInt32(max.Content))
                        {
                            max = btnPs[i];
                        }
                    }
                    wagers = true;
                    tbQuestion.Text = questions[gameIndex + 482];
                    tbQuestion.Visibility = Visibility.Visible;
                    btnNoAnswer.Content = "Wagers in?";
                    btnNoAnswer.IsEnabled = true;
                    btnFixScore.IsEnabled = true;
                }
                else
                {
                    System.Windows.MessageBox.Show(txtPlayers[0].Text + " is the winner!");
                    turn = 0;
                }

                if (btnPs.Count() > 2)
                {
                    for (int i = 0; i < btnPs.Count(); i++)
                    {
                        if (btnPs[i] != min && btnPs[i] != max)
                        {
                            mid = btnPs[i];
                            break;
                        }
                    }
                }


            }

        }

        private void btnGameClick(object sender, RoutedEventArgs e)
        {

            foreach (var line in questions)
            {
                if (line==txtGame.Text)
                {
                    gameSelected = true;
                    gameIndex = questions.IndexOf(line);

                    for (int i = gameIndex; i < gameIndex + 488; i += 8)
                    {
                        List<string> values = new List<string>();
                        values.Add(questions[gameIndex + (i-gameIndex) + 4]);
                        if(questions[gameIndex + (i - gameIndex) + 4]=="NA")
                        {
                            NAs.Add((i - gameIndex) / 8);
                        }                        
                        values.Add(questions[gameIndex + (i-gameIndex) + 5]);
                        values.Add(questions[gameIndex + (i-gameIndex) + 6]);
                        clues.Add((i - gameIndex)/8, values);
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        tbCats[i].Text = questions[(8 * i) + gameIndex + 2];
                    }

                    for (int i = 0; i < 30; i++)
                    {
                        if (!NAs.Contains(i))
                        {
                            btnQs[i].IsEnabled = true;                          
                        }
                        else
                        {
                            btnQs[i].Visibility = Visibility.Hidden;
                        }
                    }

                    txtGame.Visibility = Visibility.Hidden;
                    btnGame.Visibility = Visibility.Hidden;
                    btnGame.IsEnabled = false;
                    tbChargePlayer.Visibility = Visibility.Visible;
                    tbCharge.Visibility = Visibility.Visible;

                    break;
                }
            }
        }

        private void btnFixScore_Click(object sender, RoutedEventArgs e)
        {
            if (correctScore)
            {
                clickedScore.Content = txtDD.Text;
                clickedScore.IsEnabled = false;
                scoreChange = false;
                tbDD.Text = "Wager?";
                txtDD.Visibility = Visibility.Hidden;
                tbDD.Visibility = Visibility.Hidden;
                correctScore = false;
            }
            else
            {
                btnPs[0].IsEnabled = true;
                btnPs[1].IsEnabled = true;
                btnPs[2].IsEnabled = true;
                scoreChange = true;
            }
            
        }
    }

  
}
