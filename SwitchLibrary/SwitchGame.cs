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
            gameName = nameValue;
            gameReleaseYear = yearValue;
            gamePrice = priceValue;
            gameSpace = spaceValue;

            gameGoldPoints = CalculateGoldPoints(gamePrice);
        }
        #endregion

        #region "Properties"
        /// <summary>
        /// The name of the game.
        /// </summary>
        public string Name
        {
            get
            {
                return gameName;
            }
            set
            {
                if (value != String.Empty)
                {
                    MessageBox.Show("Game name must not be blank.");
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
        public double Price
        {
            get
            {
                return gamePrice;
            }
            set
            {
                gamePrice = value;
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
                gameSpace = value;
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
