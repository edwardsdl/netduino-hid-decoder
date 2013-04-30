namespace AngrySquirrel.Netduino.HidDecoder
{
    /// <summary>
    /// Represents 26-bit H10301 formatted card data
    /// </summary>
    /// <remarks>
    /// For more information about common card data formats, visit http://www.hidglobal.com/page.php?page_id=10.
    /// </remarks>
    public class H10301CardData
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="H10301CardData"/> class
        /// </summary>
        /// <param name="rawData">
        /// The raw card data
        /// </param>
        public H10301CardData(int rawData)
        {
            RawData = rawData;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the card number
        /// </summary>
        public int CardNumber
        {
            get { return RawData >> 1 & 0xFFFF; }
        }

        /// <summary>
        /// Gets the facility code
        /// </summary>
        public int FacilityCode
        {
            get { return RawData >> 17 & 0xFF; }
        }

        /// <summary>
        /// Gets the leading parity bit
        /// </summary>
        public int LeadingParityBit
        {
            get { return RawData >> 25 & 0x1; }
        }

        /// <summary>
        /// Gets the raw card data
        /// </summary>
        public int RawData { get; private set; }

        /// <summary>
        /// Gets the trailing parity bit
        /// </summary>
        public int TrailingParityBit
        {
            get { return RawData & 0x1; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Checks that the given card data is valid
        /// </summary>
        /// <returns>
        /// A value indicating whether the given card data is valid
        /// </returns>
        public bool IsValid()
        {
            return CalculateHammingWeight(RawData >> 13 & 0x1FFF) % 2 == 0 &&
                   CalculateHammingWeight(RawData & 0x1FFF) % 2 == 1;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the current <see cref="object" />
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents the current <see cref="object" />
        /// </returns>
        public override string ToString()
        {
            return "Leading Parity Bit: " + LeadingParityBit + "\n" + "Facility Code: " + FacilityCode + "\n" +
                   "Card Number: " + CardNumber + "\n"
                   + "Trailing Parity Bit: " + TrailingParityBit + "\n" + "Is Valid: " + IsValid();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Hamming weight of the given bit array
        /// </summary>
        /// <param name="bitArray">
        /// The bit array to use when calculating the Hamming weight
        /// </param>
        /// <returns>
        /// The Hamming weight of the given bit array
        /// </returns>
        private int CalculateHammingWeight(int bitArray)
        {
            var hammingWeight = 0;
            for (var i = 0; i < 32; i++)
            {
                hammingWeight += bitArray >> i & 0x1;
            }

            return hammingWeight;
        }

        #endregion
    }
}