using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SwitchLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PopulateYears();
            ResetForm();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit? :(","Exit?",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void PopulateYears()
        {
            const int START_YEAR = 2016;

            for (int year = DateTime.Now.Year; year >= START_YEAR; year--)
            {
                comboYear.Items.Add(year);
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            double price = 0;
            double space = 0;
            if (double.TryParse(textPrice.Text, out price))
            {
                if (double.TryParse(textSpace.Text, out space))
                {
                    try
                    {
                        // Everything is valid?
                        SwitchGame newGame = new SwitchGame(textName.Text, int.Parse(comboYear.Text), price, space);

                        listGames.Items.Add(newGame.ToString());
                        textGoldPoints.Text = newGame.GoldPoints.ToString();

                        buttonAdd.IsEnabled = false;
                        buttonReset.Focus();
                    }
                    catch (ArgumentException ex)
                    {
                        if (ex.ParamName == SwitchGame.NameParameter)
                        {
                            ReportError(ex.Message, textName);
                        }
                        else if (ex.ParamName == SwitchGame.PriceParameter)
                        {
                            ReportError(ex.Message, textPrice);
                        }
                        else if (ex.ParamName == SwitchGame.SpaceParameter)
                        {
                            ReportError(ex.Message, textSpace);
                        }
                        else
                        {
                            MessageBox.Show("An unknown entry error has taken place. Please try again.", "Entry Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Diplsya all available exception details.
                        MessageBox.Show("An unknown error has occurred. Please contact your IT service provider and provide the following information:"+
                            "\nMesssage: " + ex.Message +
                            "\nSource: " + ex.Source + 
                            "\nType: " + ex.GetType() + 
                            "\n\nStack Trace: " + ex.StackTrace, "Unknown Error");
                    }
                }
                else
                {
                    ReportError("Space required must be a number", textSpace);
                }
            }
            else
            {
                ReportError("Price must be a number", textPrice);
            }
        }

        private void ResetForm()
        {
            textName.Clear();
            textPrice.Clear();
            textSpace.Clear();
            textGoldPoints.Clear();
            comboYear.SelectedIndex = 0;
            buttonAdd.IsEnabled = true;
            textName.Focus();
        }

        private void ReportError(string message, TextBox boxInError)
        {
            boxInError.Background = Brushes.DeepPink;
            boxInError.BorderBrush = Brushes.MediumVioletRed;
            boxInError.SelectAll();
            boxInError.Focus();
            statusMessage.Content = message;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveDialog.ShowDialog() == true)
            {

                FileStream myFile = new FileStream(saveDialog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(myFile);

                //try
                //{
                //    for (int index = 0; index < SwitchGame.List.Count; index++)
                //    {
                //        writer.Write(SwitchGame.List[index].ToString() + "\n");
                //    }
                //}
                //catch (IOException ex)
                //{
                //    MessageBox.Show("An error was encountered while trying to write the list to " + myFile.Name + ".\n" + ex.Message, "File Access Error");
                //}
                //catch (Exception ex)
                //{
                //    // Diplsya all available exception details.
                //    MessageBox.Show("An unknown error has occurred. Please contact your IT service provider and provide the following information:" +
                //        "\nMesssage: " + ex.Message +
                //        "\nSource: " + ex.Source +
                //        "\nType: " + ex.GetType() +
                //        "\n\nStack Trace: " + ex.StackTrace, "Unknown Error");
                //}
                //finally
                //{
                //    writer.Close();
                //}

                try
                {
                    //for (int index = 0; index < SwitchGame.List.Count; index++)
                    //{
                        string jsonString = JsonSerializer.Serialize(SwitchGame.List);
                        writer.WriteLine(jsonString);
                    //}
                }
                catch (IOException ex)
                {
                    MessageBox.Show("An error was encountered while trying to write the list to " + myFile.Name + ".\n" + ex.Message, "File Access Error");
                }
                catch (Exception ex)
                {
                    // Diplsya all available exception details.
                    MessageBox.Show("An unknown error has occurred. Please contact your IT service provider and provide the following information:" +
                        "\nMesssage: " + ex.Message +
                        "\nSource: " + ex.Source +
                        "\nType: " + ex.GetType() +
                        "\n\nStack Trace: " + ex.StackTrace, "Unknown Error");
                }
                finally
                {
                    writer.Close();
                }

            }
        }
    }
}