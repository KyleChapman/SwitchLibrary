using System.Windows;

namespace SwitchLibrary
{
    class SwitchGame
    {
        #region "Variables"
        private string gameName = String.Empty;
        private int gameReleaseYear = DateTime.Today.Year;
        private double gamePrice = 0.0;
        private double gameGoldPoints = 0.0;
        private double gameSpace = 0.0;
        private bool isDigital = false;

        public static string NameParameter = "Name";
        public static string PriceParameter = "Price";
        public static string SpaceParameter = "Space";
        #endregion

        #region "Constructors"
        /// <summary>
        /// Default constructor for Switch Games. Doesn't do anything.
        /// </summary>
        public SwitchGame()
        { 
        }

        public SwitchGame(string nameValue, int yearValue, double priceValue, double spaceValue)
        {
            Name = nameValue;
            ReleaseYear = yearValue;
            Price = priceValue;
            Space = spaceValue;

            gameGoldPoints = CalculateGoldPoints(gamePrice);
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

        // Price
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

        // Gold points
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

        #endregion

        #region "Functions/Methods"
        private double CalculateGoldPoints(double price)
        {
            const double goldRate = 0.10;
            return goldRate * price;
        }

        public string GetGameInfo()
        {
            return gameName + " (" + gameReleaseYear + ") - $" + gamePrice;
        }
        #endregion
    }
}
