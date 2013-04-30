using Microsoft.SPOT;

namespace AngrySquirrel.Netduino.HidDecoder
{
    /// <summary>
    /// Represents the event arguments containing decoded card data information
    /// </summary>
    public class CardDecodedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CardDecodedEventArgs"/> class
        /// </summary>
        /// <param name="cardData">
        /// The card data of the HID card/key
        /// </param>
        internal CardDecodedEventArgs(H10301CardData cardData)
        {
            CardData = cardData;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the card data of the HID card/key
        /// </summary>
        public H10301CardData CardData { get; private set; }

        #endregion
    }
}