// Author:  Kyle Chapman
// Created: October 11, 2024
// Updated: November 6, 2024
// Description:
// For individual Nintendo Switch games as objects, intended for use
// in a program where you keep track of owned Switch games in a list.

using System.Windows;

namespace SwitchLibrary
{
    /// <summary>
    /// A class representing individual games for the Nintendo Switch.
    /// </summary>
    class SwitchGame
    {
        #region "Variables"
        // Instance variables.
        private string gameName = String.Empty;
        private int gameReleaseYear = DateTime.Today.Year;
        private double gamePrice = 0.0;
        private double gameGoldPoints = 0.0;
        private double gameSpace = 0.0;
        private bool isDigital = false;

        // A static List that contains all games created to date.
        private static List<SwitchGame> gameList = new List<SwitchGame>();

        // Constant static strings for use with exceptions. These may be referenced in other project files.
        public const string NameParameter = "Name";
        public const string PriceParameter = "Price";
        public const string SpaceParameter = "Space";
        #endregion

        #region "Constructors"
        /// <summary>
        /// Default constructor for Switch Games. Doesn't do anything.
        /// </summary>
        public SwitchGame()
        { 
        }

        /// <summary>
        /// Parametrized constructor for Switch Games. Creates a new SwitchGame instance.
        /// </summary>
        /// <param name="nameValue">The title of the game</param>
        /// <param name="yearValue">The year that the game was released</param>
        /// <param name="priceValue">The price paid for the game, or the MSRP (some interpretation can be used)</param>
        /// <param name="spaceValue">The amount of storage space the game requires</param>
        public SwitchGame(string nameValue, int yearValue, double priceValue, double spaceValue)
        {
            // Set property values using properties so the validation is run.
            Name = nameValue;
            ReleaseYear = yearValue;
            Price = priceValue;
            Space = spaceValue;

            // Calculate the gold points.
            gameGoldPoints = CalculateGoldPoints(gamePrice);

            // Add this game instance to the list.
            gameList.Add(this);
        }
        #endregion

        #region "Properties"
        /// <summary>
        /// The name of the game.
        /// </summary>
        public string Name { get => gameName; 
            set
            {
                if (value.Trim() == String.Empty)
                {
                    throw new ArgumentNullException(NameParameter,"The game name cannot be blank.");
                }
                else
                {
                    gameName = value;
                }
            }
        }

        /// <summary>
        /// The year that the game was released.
        /// </summary>
        public int ReleaseYear
        {
            get
            {
                return gameReleaseYear;
            }
            set
            {
                gameReleaseYear = value;
            }
        }

        /// <summary>
        /// The price paid for the game, or the MSRP (some interpretation can be used).
        /// </summary>
        public double Price { get => gamePrice; 
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentOutOfRangeException(PriceParameter, "Game price must not be negative.");
                }
                else
                {
                    gamePrice = value;
                }
            }
        }

        /// <summary>
        /// A calculated value representing the gold points earned for purchasing the game.
        /// </summary>
        public double GoldPoints
        {
            get
            {
                return gameGoldPoints;
            }
        }

        /// <summary>
        /// The space the game consumes in MB (for now..?).
        /// </summary>
        public double Space
        {
            get
            {
                return gameSpace;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentOutOfRangeException(SpaceParameter, "Game's space must not be negative.");
                }
                else
                {
                    gameSpace = value;
                }
            }
        }

        /// <summary>
        /// A list of all instantiated Switch games.
        /// </summary>
        public static List<SwitchGame> List { get => gameList; }

        #endregion

        #region "Functions/Methods"
        /// <summary>
        /// Calculates the gold points earned for purchasing the game. Does not account for things like used game prices or games that do not qualify for gold points.
        /// </summary>
        /// <param name="price">The price paid for the game.</param>
        /// <returns>Gold points.</returns>
        private int CalculateGoldPoints(double price)
        {
            const double goldRate = 0.10;
            return (int)Math.Round(goldRate * price, 0);
        }

        /// <summary>
        /// Converts a SwitchGame to a short string displaying some of its properties.
        /// </summary>
        /// <returns>SwitchGame as a string</returns>
        public override string ToString()
        {
            return gameName + " (" + gameReleaseYear + ") - $" + gamePrice;
        }
        #endregion
    }
}
