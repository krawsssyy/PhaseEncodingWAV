using System;
using System.IO;

namespace SteganoWave
{
    //hides and extract data in/from a wave stream
    public class WaveUtility
    {
        //carrier wave used for extracting data
        private WaveStream sourceStream;
        //stream to receive the edited carrier wave
        private Stream destinationStream;
        private int bytesPerSample;

        public WaveUtility(WaveStream source, Stream destination)
        {
            this.sourceStream = source;
            destinationStream = destination;
            bytesPerSample = sourceStream.Format.wBitsPerSample / 8;
        }
        public WaveUtility(WaveStream source)
        {
            this.sourceStream = source;
            bytesPerSample = sourceStream.Format.wBitsPerSample / 8;
        }

        private (double, double)[] dft(int[] block)
        {
            int N = block.Length;
            double[] real = new double[N];
            double[] img = new double[N];
            (double, double)[] res = new (double, double)[N];
            for (int k = 0; k < N; k++)
            {
                real[k] = img[k] = 0;
                for (int n = 0; n < N; n++)
                {
                    real[k] += (double)block[n] * Math.Cos(-2 * Math.PI * k * n / N);
                    img[k] += (double)block[n] * Math.Sin(-2 * Math.PI * k * n / N);
                }
                res[k] = (Math.Sqrt(real[k] * real[k] + img[k] * img[k]), Math.Atan2(img[k], real[k]));
            }
            return res;
        }

        private int[] StringToBin(byte[] data, int len)
        {
            int[] res = new int[len];
            int pos = 0;
            foreach (byte value in data)
            {
                string binarybyte = Convert.ToString(value, 2);
                // add missing 0s
                while (binarybyte.Length < 8)
                {
                    binarybyte = "0" + binarybyte;
                }
                for (int i = 0; i < 8; i++)
                {
                    res[pos] = Int32.Parse(binarybyte[i].ToString());
                    pos++;
                }
            }
            return res;
        }

        private byte[] idft_real((double, double)[] data, bool is8bit)
        {
            int N = data.Length;
            byte[] real;
            if (is8bit)
            {
                real = new byte[N];
            }
            else
            {
                real = new byte[2 * N];
            }
            double[] res = new double[N];
            for (int n = 0; n < N; n++)
            {
                res[n] = 0;
                for (int k = 0; k < N; k++)
                {
                    res[n] += (data[k].Item1 * Math.Cos(2 * Math.PI * k * n / N)) - (data[k].Item2 * Math.Sin(2 * Math.PI * k * n / N));
                }
                res[n] = res[n] / (double)N;
                if (is8bit)
                {
                    real[n] = (byte)res[n];
                }
                else
                {
                    byte[] buf = BitConverter.GetBytes((short)res[n]);
                    Buffer.BlockCopy(buf, 0, real, 2 * n, buf.Length);
                }
            }
            return real;
        }

        public void HidePhaseCoding(Stream messageStream)
        {
            // get message length
            int msgLen = messageStream.ReadByte();
            // length considered to be byte => max len = 255 bytes, quick modification for bigger length of messages
            messageStream.ReadByte();
            messageStream.ReadByte();
            messageStream.ReadByte();
            // get message
            byte[] msg = new byte[msgLen];
            for (int i = 0; i < msgLen; i++)
            {
                msg[i] = (byte)messageStream.ReadByte();
            }
            // message length in bits
            msgLen *= 8;
            // get amount of necessary blocks
            int blockLength = (int)(2 * Math.Pow(2, Math.Ceiling(Math.Log(2 * msgLen, 2))));
            int noBlocks = (int)(sourceStream.Length / sourceStream.Format.nChannels);
            // check if 8bits per sample or 16
            bool is8bit = bytesPerSample == 1 ? true : false;
            // check if file is single channel or dual-channel
            bool isDual = sourceStream.Format.nChannels == 2 ? true : false;
            noBlocks = (int)Math.Ceiling((double)noBlocks / (double)blockLength);
            // initialize byte matrix for chunks, result matrix for the discrete fourier transform and also array for the other channel in case of dual channels
            int[][] chunks = new int[noBlocks][];
            int[][] dualChn = new int[noBlocks][];
            (double, double)[][] dft = new (double, double)[noBlocks][];
            for (int i = 0; i < noBlocks; i++)
            {
                chunks[i] = new int[blockLength];
                if (isDual)
                {
                    dualChn[i] = new int[blockLength];
                }
                dft[i] = new (double, double)[blockLength];
            }
            // populate the chunks (and dual channel if applicable) and calculate their dft
            // dft item1 - amplitude, item2 - phase
            for (int i = 0; i < noBlocks; i++)
            {
                for (int j = 0; j < blockLength; j++)
                {
                    byte[] buf = new byte[2];
                    if (is8bit)
                    {
                        sourceStream.Read(buf, 0, 1);
                        chunks[i][j] = buf[0];
                        if (isDual)
                        {
                            sourceStream.Read(buf, 0, 1);
                            dualChn[i][j] = buf[0];
                        }
                    }
                    else
                    {
                        sourceStream.Read(buf, 0, 2);
                        chunks[i][j] = BitConverter.ToInt16(buf, 0);
                        if (isDual)
                        {
                            sourceStream.Read(buf, 0, 2);
                            dualChn[i][j] = BitConverter.ToInt16(buf, 0);
                        }
                    }

                }
                dft[i] = this.dft(chunks[i]);
            }
            // calculate the phase differences for each block
            double[][] phaseDiffs = new double[noBlocks][];
            phaseDiffs[0] = new double[blockLength];
            for (int i = 1; i < noBlocks; i++)
            {
                phaseDiffs[i] = new double[blockLength];
                for (int j = 0; j < blockLength; j++)
                {
                    phaseDiffs[i][j] = dft[i][j].Item2 - dft[i - 1][j].Item2;
                }
            }
            // convert message to binary - result stored in an int array
            int[] msgBin = this.StringToBin(msg, msgLen);
            int blockMid = blockLength / 2;
            int ctr = 0;
            // update phases accordingly
            for (int i = blockMid - msgLen; i < blockMid; i++)
            {
                int sgn = 1;
                if (msgBin[ctr] == 1)
                {
                    sgn = -1;
                }
                dft[0][i].Item2 = sgn * Math.PI / 2;
                ctr++;
            }
            ctr = msgLen - 1;
            for(int i = blockMid + 1; i < blockMid + msgLen + 1; i++)
            {
                int sgn = 1;
                if (msgBin[ctr] == 0)
                {
                    sgn = -1;
                }
                dft[0][i].Item2 = sgn * Math.PI / 2;
                ctr--;
            }
            // recompute phases
            for (int i = 1; i < noBlocks; i++)
            {
                for (int j = 0; j < blockLength; j++)
                {
                    dft[i][j].Item2 = dft[i - 1][j].Item2 + phaseDiffs[i][j];
                }
            }
            // initialize matrices for inverse dft
            (double, double)[][] newData = new (double, double)[noBlocks][];
            byte[][] finalData = new byte[noBlocks][];
            // prepare the values that will go into the idft - amplitude * exp(i * phase)
            for (int i = 0; i < noBlocks; i++)
            {
                newData[i] = new (double, double)[blockLength];
                if (is8bit)
                {
                    finalData[i] = new byte[blockLength];
                }
                else
                {
                    finalData[i] = new byte[2 * blockLength];
                }
                for (int j = 0; j < blockLength; j++)
                {
                    newData[i][j].Item1 = dft[i][j].Item1 * Math.Cos(dft[i][j].Item2);
                    newData[i][j].Item2 = dft[i][j].Item1 * Math.Sin(dft[i][j].Item2);
                }
                // get the real part from the idft
                finalData[i] = idft_real(newData[i], is8bit);
                // write values into our destination
                if (!isDual)
                {
                    destinationStream.Write(finalData[i], 0, finalData[i].Length);
                }
                else
                {
                    if (is8bit)
                    {
                        for (int j = 0; j < blockLength; j++)
                        {
                            byte[] left = new byte[1];
                            byte[] right = new byte[1];
                            left[0] = finalData[i][j];
                            right[0] = (byte)dualChn[i][j];
                            destinationStream.Write(left, 0, 1);
                            destinationStream.Write(right, 0, 1);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < blockLength; j++)
                        {
                            byte[] right = BitConverter.GetBytes((short)dualChn[i][j]);
                            byte[] left = new byte[2];
                            left[0] = finalData[i][2 * j];
                            left[1] = finalData[i][2 * j + 1];
                            destinationStream.Write(left, 0, 2);
                            destinationStream.Write(right, 0, 2);
                        }
                    }
                }

            }
            // get the rest of the data from the original file and copy it to the destionation
            byte[] reminder = new byte[4];
            int c = sourceStream.Read(reminder, 0, 4);
            while (c > 0)
            {
                destinationStream.Write(reminder, 0, c);
                c = sourceStream.Read(reminder, 0, 4);
            }
        }

        public void ExtractPhaseCoding(Stream messageStream)
        {
            // requirement - have to know message length
            int msgLen = "test msg muie adi".Length * 8; // for message="ABBA" - modify for different length of message
            int blockLength = (int)(2 * Math.Pow(2, Math.Ceiling(Math.Log(2 * msgLen, 2))));
            int blockMid = blockLength / 2;
            // check if 8bits per sample or 16
            bool is8bit = bytesPerSample == 1 ? true : false;
            bool isDual = sourceStream.Format.nChannels == 2 ? true : false;
            // initialize byte matrix for chunks and result matrix for the discrete fourier transform
            int[] chunk = new int[blockLength];
            (double, double)[] dft = new (double, double)[blockLength];
            // populate the chunks and calculate their dft
            // dft item1 - amplitude, item2 - phase
            for (int i = 0; i < blockLength; i++)
            {
                byte[] buf = new byte[2];
                if (is8bit)
                {
                    sourceStream.Read(buf, 0, 1);
                    chunk[i] = buf[0];
                    if (isDual)
                    {
                        sourceStream.Read(buf, 0, 1);
                    }
                }
                else
                {
                    sourceStream.Read(buf, 0, 2);
                    chunk[i] = BitConverter.ToInt16(buf, 0);
                    if (isDual)
                    {
                        sourceStream.Read(buf, 0, 2);
                    }
                }
            }
            dft = this.dft(chunk);
            string binStr = ""; // string container for the message in binary
            // traverse the modified phases and do the reverse, if the phase is negative then add a '1', else add a '0'
            for (int i = blockMid - msgLen; i < blockMid; i++)
            {
                if (dft[i].Item2 < 0)
                {
                    binStr += "1";
                }
                else
                {
                    binStr += "0";
                }
            }
            // rebuild message
            for (int i = 0; i < binStr.Length; i += 8)
            {
                byte aux = (byte)Convert.ToSByte(binStr.Substring(i, 8), 2);
                byte[] msgByte = new byte[1]; msgByte[0] = aux;
                messageStream.Write(msgByte, 0, 1);
            }
        }

    }
}
