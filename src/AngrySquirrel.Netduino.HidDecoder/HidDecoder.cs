using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace AngrySquirrel.Netduino.HidDecoder
{
    /// <summary>
    /// Represents the HID MiniProx RFID decoder
    /// </summary>
    public class HidDecoder
    {
        #region Fields

        private readonly int data0Port;

        private readonly int data1Port;

        private readonly TimeSpan transmissionTimeout = new TimeSpan(0, 0, 0, 0, 500);

        private DateTime lastTransmissionTime;

        private int rawData;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HidDecoder"/> class
        /// </summary>
        /// <param name="hidDecoderParams">
        /// The HID decoder parameters
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the given <see cref="HidDecoderParams"/> is null
        /// </exception>
        public HidDecoder(HidDecoderParams hidDecoderParams)
        {
            if (hidDecoderParams == null)
            {
                throw new ArgumentNullException("hidReaderParams");
            }

            data0Port = (int)hidDecoderParams.Data0.Id;
            data1Port = (int)hidDecoderParams.Data1.Id;
            hidDecoderParams.Data0.OnInterrupt += OnInterrupt;
            hidDecoderParams.Data1.OnInterrupt += OnInterrupt;

            // This thread waits until no data has been transmitted from the HID decoder for a period of time
            // an then fires the CardDecodedEvent. This is so that cards longer or shorter than the expected
            // 26 bits don't cause the software to choke.
            new Thread(() =>
                {
                    while (true)
                    {
                        if (DateTime.Now.Subtract(lastTransmissionTime) > transmissionTimeout)
                        {
                            if (rawData != 0)
                            {
                                if (CardDecoded != null)
                                {
                                    CardDecoded(this, new CardDecodedEventArgs(new H10301CardData(rawData)));
                                }

                                rawData = 0;
                            }
                        }

                        Thread.Sleep(5);
                    }
                }).Start();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Represents a delegate for handling the event in which card data has been decoded from the HID RFID reader
        /// </summary>
        /// <param name="sender">
        /// The sender of the <see cref="CardDecoded" /> event
        /// </param>
        /// <param name="cardDecodedEventArgs">
        /// The event arguments containing decoded card data information
        /// </param>
        public delegate void CardDecodedEventHandler(object sender, CardDecodedEventArgs cardDecodedEventArgs);

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when card data has been decoded from the HID RFID reader
        /// </summary>
        public event CardDecodedEventHandler CardDecoded;

        #endregion

        #region Methods

        /// <summary>
        /// Clears the card number's next bit
        /// </summary>
        private void ClearNextCardNumberBit()
        {
            rawData = rawData << 0x1;
        }

        /// <summary>
        /// Handles the <see cref="InterruptPort.OnInterrupt"/> event for the DATA0 and DATA1 <see cref="InterruptPort"/>s
        /// </summary>
        /// <param name="port">
        /// The port on which the interrupt is occurring
        /// </param>
        /// <param name="data">
        /// The data describing the state of the port
        /// </param>
        /// <param name="time">
        /// The time the event occurred
        /// </param>
        private void OnInterrupt(uint port, uint data, DateTime time)
        {
            if (port == data0Port)
            {
                ClearNextCardNumberBit();
            }
            else if (port == data1Port)
            {
                SetNextCardNumberBit();
            }
            else
            {
                var message = "The value of \"port\" must be either " + data0Port + " for DATA0, or " + data1Port + "for DATA1";
                throw new ArgumentOutOfRangeException("port", message);
            }

            lastTransmissionTime = time;
        }

        /// <summary>
        /// Sets the card number's next bit
        /// </summary>
        private void SetNextCardNumberBit()
        {
            rawData = rawData << 0x1 | 0x1;
        }

        #endregion
    }
}