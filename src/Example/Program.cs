using System.Threading;
using AngrySquirrel.Netduino.HidDecoder;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace Example
{
    /// <summary>
    /// Represents an example project showing how to use the <see cref="HidDecoder" /> library
    /// </summary>
    public class Program
    {
        #region Public Methods and Operators

        /// <summary>
        /// Program entry point
        /// </summary>
        public static void Main()
        {
            var data0 = new InterruptPort(Pins.GPIO_PIN_D0, 
                                          false, 
                                          ResistorModes.Disabled, 
                                          InterruptModes.InterruptEdgeLow);

            var data1 = new InterruptPort(Pins.GPIO_PIN_D1, 
                                          false, 
                                          ResistorModes.Disabled, 
                                          InterruptModes.InterruptEdgeLow);

            var hidDecoder = new HidDecoder(new HidDecoderParams(data0, data1));
            hidDecoder.CardDecoded += OnCardDecoded;

            Thread.Sleep(Timeout.Infinite);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the <see cref="HidDecoder.CardDecoded"/> event
        /// </summary>
        /// <param name="sender">
        /// The sender of the <see cref="HidDecoder.CardDecoded"/> event
        /// </param>
        /// <param name="cardDecodedEventArgs">
        /// The event arguments containing decoded card data information
        /// </param>
        private static void OnCardDecoded(object sender, CardDecodedEventArgs cardDecodedEventArgs)
        {
            Debug.Print(cardDecodedEventArgs.CardData.ToString());
        }

        #endregion
    }
}