using System;
using Microsoft.SPOT.Hardware;

namespace AngrySquirrel.Netduino.HidDecoder
{
    /// <summary>
    /// Represents the parameters needed for the <see cref="HidDecoder" /> to begin decoding HID cards/keys
    /// </summary>
    public class HidDecoderParams
    {
        #region Fields

        /// <summary>
        /// The interrupt port from which 0s will be transmitted from the HID RFID reader
        /// </summary>
        public readonly InterruptPort Data0;

        /// <summary>
        /// The interrupt port from which 1s will be transmitted from the HID RFID reader
        /// </summary>
        public readonly InterruptPort Data1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HidDecoderParams"/> class
        /// </summary>
        /// <param name="data0">
        /// The interrupt port from which 0s will be transmitted from the HID RFID reader
        /// </param>
        /// <param name="data1">
        /// The interrupt port from which 1s will be transmitted from the HID RFID reader
        /// </param>
        public HidDecoderParams(InterruptPort data0, InterruptPort data1)
        {
            if (data0 == null)
            {
                throw new ArgumentNullException("data0", "The DATA0 interrupt port must not be null.");
            }

            if (data1 == null)
            {
                throw new ArgumentNullException("data1", "The DATA1 interrupt port must not be null.");
            }

            Data0 = data0;
            Data1 = data1;
        }

        #endregion
    }
}