// Author:  Kyle Chapman
// Created: October 11, 2024
// Updated: November 6, 2024
// Description:
// Code for a WPF app that adds games for the Nintendo Switch into
// a list. This particular demo was made to demonstrate class
// definitions, and was expanded to include both exception
// handling and data persistence.

using Microsoft.Win32;
using System.Data;
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
        /// <summary>
        /// Constructor for the window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // After the form is initialized, populate the "release year" combobox.
            PopulateYears();
            ResetForm();
        }

        /// <summary>
        /// Event handler to confirm the entry and add the game to the list.
        /// </summary>
        private void AddGameClick(object sender, RoutedEventArgs e)
        {
            double price = 0;
            double space = 0;
            // Type validation for the price TextBox.
            if (double.TryParse(textPrice.Text, out price))
            {
                // Type validation for the space TextBox.
                if (double.TryParse(textSpace.Text, out space))
                {
                    // Prepare to catch exceptions thrown in the class.
                    try
                    {
                        // Everything is valid? Create a SwitchGame object. Note that this may throw exceptions.
                        SwitchGame newGame = new SwitchGame(textName.Text, int.Parse(comboYear.Text), price, space);

                        // Add the game's text to the ListView's Items property.
                        // There is another approach that we may change this to use in the near future.
                        listGames.Items.Add(newGame.ToString());
                        // Display the game's Gold Points.
                        textGoldPoints.Text = newGame.GoldPoints.ToString();

                        // Entry complete; need to click Reset to add another game.
                        DisableEntryControls();
                        buttonReset.Focus();
                    }
                    // Catch ArgumentExceptions.
                    catch (ArgumentException ex)
                    {
                        // If the name is in error, report that.
                        if (ex.ParamName == SwitchGame.NameParameter)
                        {
                            ReportError(ex.Message, textName);
                        }
                        // If the price is in error, report that.
                        else if (ex.ParamName == SwitchGame.PriceParameter)
                        {
                            ReportError(ex.Message, textPrice);
                        }
                        // If the space is in error, report that.
                        else if (ex.ParamName == SwitchGame.SpaceParameter)
                        {
                            ReportError(ex.Message, textSpace);
                        }
                        // Some other ArgumentException was thrown? Not sure what happened.
                        else
                        {
                            MessageBox.Show("An unknown entry error has taken place. Please try again.", "Entry Error");
                            StatusMessage("An unknown error has occurred.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Diplsya all available exception details.
                        MessageBox.Show("An unknown error has occurred. Please contact your IT service provider and provide the following information:" +
                            "\nMesssage: " + ex.Message +
                            "\nSource: " + ex.Source +
                            "\nType: " + ex.GetType() +
                            "\n\nStack Trace: " + ex.StackTrace, "Unknown Error");
                        StatusMessage("An unknown error has occurred.");
                    }
                }
                // Space was non-numeric.
                else
                {
                    ReportError("Space required must be a number", textSpace);
                }
            }
            // Price was non-numeric.
            else
            {
                ReportError("Price must be a number", textPrice);
            }
        }
        
        /// <summary>
        /// Resets the form to its default state to be ready for new entries.
        /// </summary>
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            ResetForm();
            StatusMessage("Ready for new entries.");
        }

        /// <summary>
        /// Semi-finished event handler for the save button to save the current list to either plain text or JSON-formatted text. Work-in-progress.
        /// </summary>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            // Create a SaveFileDialog object.
            SaveFileDialog saveDialog = new SaveFileDialog();
            // Set the SaveFileDialog's filetype filter.
            saveDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            // If the saveDialog has its "Save"/"OK" button clicked...
            if (saveDialog.ShowDialog() == true)
            {
                // Then we create a StreamWriter instance to write to the file.
                FileStream myFile = new FileStream(saveDialog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(myFile);

                // This version of the code (try block) writes the contents of the list to plain text, one line at a time.
                //try
                //{
                //    for (int index = 0; index < SwitchGame.List.Count; index++)
                //    {
                //        writer.Write(SwitchGame.List[index].ToString() + "\n");
                //    }
                //}

                // This version of the code (try block) serializes the entire list into JSON.
                try
                {
                    string jsonString = JsonSerializer.Serialize(SwitchGame.List);
                    writer.WriteLine(jsonString);
                    StatusMessage("List saved to " + myFile.Name);
                }
                // Catch IOExceptions thrown by file permissions, etc.
                catch (IOException ex)
                {
                    MessageBox.Show("An error was encountered while trying to write the list to " + myFile.Name + ".\n" + ex.Message, "File Access Error");
                    StatusMessage("Error writing the list to " + myFile.Name);
                }
                catch (Exception ex)
                {
                    // Diplsya all available exception details.
                    MessageBox.Show("An unknown error has occurred. Please contact your IT service provider and provide the following information:" +
                        "\nMesssage: " + ex.Message +
                        "\nSource: " + ex.Source +
                        "\nType: " + ex.GetType() +
                        "\n\nStack Trace: " + ex.StackTrace, "Unknown Error");
                    StatusMessage("An unknown error has occurred.");
                }
                finally
                {
                    writer.Close();
                }

            }
            else
            {
                StatusMessage("Cancelled Save operation.");
            }
        }

        /// <summary>
        /// Me close form.
        /// </summary>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit? :(","Exit?",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Close();
            }
            StatusMessage("Cancelled Exit operation.");
        }

        /// <summary>
        /// When the user overtypes in the entry TextBoxes, clear any colour formatting caused by prior errors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyText(object sender, TextChangedEventArgs e)
        {
            TextBox modifiedBox = (TextBox)sender;
            ResetColour(modifiedBox);
        }

        /// <summary>
        /// Populate the ComboBox with a list of years. Goes from START_YEAR up to the current year.
        /// </summary>
        private void PopulateYears()
        {
            // Configures what year the list of years starts from.
            const int START_YEAR = 2016;

            // Count from the present year until START_YEAR, and adds the year to the ComboBox's Items property.
            for (int year = DateTime.Now.Year; year >= START_YEAR; year--)
            {
                comboYear.Items.Add(year);
            }
        }

        /// <summary>
        /// Reset the form to be ready for new entries.
        /// </summary>
        private void ResetForm()
        {
            textName.Clear();
            textPrice.Clear();
            textSpace.Clear();
            textGoldPoints.Clear();
            EnableEntryControls();
            comboYear.SelectedIndex = 0;
            textName.Focus();
        }

        /// <summary>
        /// Disables all entry controls, to be used when a new list entry is added.
        /// </summary>
        private void DisableEntryControls()
        {
            textName.IsEnabled = false;
            ResetColour(textName);
            textPrice.IsEnabled = false;
            ResetColour(textPrice);
            textSpace.IsEnabled = false;
            ResetColour(textSpace);
            comboYear.IsEnabled = false;
            checkIsDigital.IsEnabled = false;
            buttonAdd.IsEnabled = false;
        }

        /// <summary>
        /// Re-enabled all entry controls, to be used when the form is reset.
        /// </summary>
        private void EnableEntryControls()
        {
            textName.IsEnabled = true;
            ResetColour(textName);
            textPrice.IsEnabled = true;
            ResetColour(textPrice);
            textSpace.IsEnabled = true;
            ResetColour(textSpace);
            comboYear.IsEnabled = true;
            checkIsDigital.IsEnabled = true;
        }

        /// <summary>
        /// When an error takes place in a specified control, highlight it with colour and put focus on it.
        /// </summary>
        /// <param name="message">The error message to display</param>
        /// <param name="boxInError">The control in an error state</param>
        private void ReportError(string message, TextBox boxInError)
        {
            boxInError.Background = Brushes.DeepPink;
            boxInError.BorderBrush = Brushes.MediumVioletRed;
            boxInError.SelectAll();
            boxInError.Focus();
            StatusMessage(message);
        }

        /// <summary>
        /// Sets a TextBox's colours back to their default values.
        /// </summary>
        /// <param name="boxToClear">The control to set to default colours</param>
        private void ResetColour(TextBox boxToClear)
        {
            boxToClear.Background = new TextBox().Background;
            boxToClear.BorderBrush = Brushes.Black;
        }

        /// <summary>
        /// Sets a message on the StatusBar that includes the current date and time.
        /// </summary>
        /// <param name="message">Message to write to the StatusBar.</param>
        private void StatusMessage(string message)
        {
            statusMessage.Content = String.Format("{0:G}", DateTime.Now) + ": " + message;
        }
    }
}