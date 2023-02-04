using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FFT : MonoBehaviour
{
    // Start is called before the first frame update
        public static Complex[] GetFFT(float[] input)
        {
            int n = input.Length;
            Complex[] output = new Complex[n];

            if (n == 1)
            {
                output[0] = new Complex(input[0], 0);
                return output;
            }

            float[] even = new float[n / 2];
            float[] odd = new float[n / 2];
            for (int i = 0; i < n / 2; i++)
            {
                even[i] = input[2 * i];
                odd[i] = input[2 * i + 1];
            }

            Complex[] evenFFT = GetFFT(even);
            Complex[] oddFFT = GetFFT(odd);

            for (int k = 0; k < n / 2; k++)
            {
                Complex t = Complex.FromPolarCoordinates(1, -2 * Mathf.PI * k / n) * oddFFT[k];
                output[k] = evenFFT[k] + t;
                output[k + n / 2] = evenFFT[k] - t;
            }

            return output;
        }

        public static float[] FFTtoFreq(Complex[] complexWave, int n)
        {
            // Get the sample rate of the waveform in Hz
            float sampleRate = 44100;

            // Get the length of the FFT in seconds
            float fftLengthInSeconds = n / sampleRate;

            // Get the frequency resolution in Hz
            float frequencyResolution = 1 / fftLengthInSeconds;

            // Get the magnitude of each component in the FFT
            float[] magnitudes = new float[n];

            for (int i = 0; i < n; i++)
            {
                magnitudes[i] = complexWave[i].Magnitude;
            }

            // Find the index of the largest magnitude
            int maxIndex = Array.IndexOf(magnitudes, magnitudes.Max());

            // Get the frequency of the dominant tone in Hz
            float dominantFrequency = maxIndex * frequencyResolution;

            return magnitudes;

        }

        public struct Complex
        {
            public float Real;
            public float Imaginary;

            public Complex(float real, float imaginary)
            {
                Real = real;
                Imaginary = imaginary;
            }

            public static Complex operator +(Complex a, Complex b)
            {
                return new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);
            }

            public static Complex operator -(Complex a, Complex b)
            {
                return new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
            }

            public static Complex operator *(Complex a, Complex b)
            {
                return new Complex(a.Real * b.Real - a.Imaginary * b.Imaginary, a.Real * b.Imaginary + a.Imaginary * b.Real);
            }

            public static Complex FromPolarCoordinates(float magnitude, float phase)
            {
                return new Complex(magnitude * (float)Math.Cos(phase), magnitude * (float)Math.Sin(phase));
            }

            public float Magnitude
            {
                get { return (float)Math.Sqrt(Real * Real + Imaginary * Imaginary); }
            }

        


        }
        public static float ConvertToPitch(Complex[] frequencyData)
        {
            float maxMagnitude = 0f;
            int maxIndex = 0;
            int dataLength = frequencyData.Length;

            // Find the frequency bin with the highest magnitude
            for (int i = 0; i < dataLength; i++)
            {
                float magnitude = frequencyData[i].Magnitude;
                if (magnitude > maxMagnitude)
                {
                    maxMagnitude = magnitude;
                    maxIndex = i;
                }
            }

            // Convert the frequency bin index to a frequency in Hz
            int sampleRate = 44100;
            float frequency = maxIndex * sampleRate / dataLength;
            return frequency;
        }

}
