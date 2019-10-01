using static Base.Utility;
using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;

namespace Base
{
    public class NoiseFactors
    {
        /// <summary>
        /// (float[])Noise() 메소드에서 사용될 파라미터들을 설정합니다.
        /// </summary>
        public NoiseFactors(
            int size = 800,
            int octave = 10,
            float softness = 8,
            float interval = 5,
            int randomSeed = 999)
        {
            Size = size;
            Octave = octave;
            Softness = softness;
            Interval = interval;
            RandomSeed = randomSeed;
        }
        public int Size { get; set; }
        public int Octave { get; set; }
        public float Softness { get; set; }
        public float Interval { get; set; }
        public int RandomSeed { get; set; }
    }

    public class PerlinNoise
    {
        /// <summary>
        /// (NoiseFactors)noiseFactors에 따른 펄린 노이즈 배열을 반환합니다.
        /// </summary>
        public static float[] Noise(NoiseFactors noiseFactors)
        {
            float[] output = new float[noiseFactors.Size];
            float[] seed = new float[noiseFactors.Size];

            Random rand = new Random(noiseFactors.RandomSeed);
            for (int i = 0; i < noiseFactors.Size; i++)
                seed[i] = (float)rand.NextDouble();

            for (int x = 0; x < noiseFactors.Size; x++)
            {
                float noise = 0f;
                float scale = 1f;
                float scaleAcc = 0f;

                for (int o = 0; o < noiseFactors.Octave; o++)
                {
                    int pitch = noiseFactors.Size >> o;
                    int sample1 = (x / pitch) * pitch;
                    int sample2 = (sample1 + pitch) % noiseFactors.Size;
                    float blend = (float)(x - sample1) / (float)pitch;
                    float sample = (1f - blend) * seed[sample1] + blend * seed[sample2];

                    noise += sample * scale;
                    scaleAcc += scale;
                    scale = scale / noiseFactors.Softness;
                }
                output[x] = noise / scaleAcc;
            }
            return output;
        }
    }
}