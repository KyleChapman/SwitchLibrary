using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            Close();
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

                        listGames.Items.Add(newGame.GetGameInfo());
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
    }
}