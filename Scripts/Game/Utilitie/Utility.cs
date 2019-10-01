using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using System.Collections.Generic;
using static Base.Consts;

namespace Base
{
    public class Utility
    {
        #region CALCULATIONS 
        /// <summary>
        /// (Vector2f)v1, v2, v3, v4를 각 모서리로 가지는 사각형의 넓이를 반환합니다.
        /// </summary>
        public static float GetAreaOf(Vector2f v1, Vector2f v2, Vector2f v3, Vector2f v4)
        {
            return 0.5f *
            (
                (v1.X * v2.Y + v2.X * v3.Y + v3.X * v4.Y + v4.X + v1.Y) -
                (v2.X * v1.Y + v3.X * v2.Y + v4.X * v3.Y + v1.X * v4.Y)
            );
        }

        /// <summary>
        /// (Vector2f)vector를 최소 (float)min에서 최대 (float)max만큼 한정한 값을 반환합니다.
        /// </summary>
        public static Vector2f Limit(Vector2f vector, float min, float max)
        {
            if (GetMagnitude(vector) < min)
                vector = SetMagnitude(vector, min);
            else if (GetMagnitude(vector) > max)
                vector = SetMagnitude(vector, max);
            return vector;
        }

        /// <summary>
        /// (Vector2f)vector를 최대 (float)max만큼 한정한 값을 반환합니다.
        /// </summary>
        public static Vector2f Limit(Vector2f vector, float max)
        {
            if (GetMagnitude(vector) > max)
                vector = SetMagnitude(vector, max);
            return vector;
        }

        /// <summary>
        /// (float)var을 최소 (float)min에서 최대 (float)max만큼 한정한 값을 반환합니다.
        /// </summary>
        public static float Limit(float var, float min, float max)
        {
            if (var < min) var = min;
            else if (var > max) var = max;
            return var;
        }

        /// <summary>
        /// (float)var을 최대 (float)max만큼 한정한 값을 반환합니다.
        /// </summary>
        public static float Limit(float var, float max)
        {
            if (var > max) var = max;
            return var;
        }

        /// <summary>
        /// (float)start1에서 (float)stop1 사이의 값인 (float)value를 (float)start2 에서 (float)stop2 사이의 비율로 치환한 값을 반환합니다.
        /// </summary>
        public static float Map(float value, float start1, float stop1, float start2, float stop2)
        {
            return ((value - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }

        /// <summary>
        /// 라디안각 (float)radian 의 디그리각을 반환합니다.
        /// </summary>
        public static float ToDegree(float radian)
        {
            return radian * 180 / MathF.PI;
        }

        /// <summary>
        /// 디그리각 (float)degree 의 라디안각을 반환합니다.
        /// </summary>
        public static float ToRadian(float degree)
        {
            return degree * MathF.PI / 180;
        }

        /// <summary>
        /// (float[])array의 엘리먼츠중 최솟값을 반환합니다.
        /// </summary>
        public static float GetMin(float[] array)
        {
            float min = array[0];
            foreach (var element in array)
            {
                if (element <= min) min = element;
            }
            return min;
        }
        /// <summary>
        /// (float[])array의 엘리먼츠중 최댓값을 반환합니다.
        /// </summary>
        public static float GetMax(float[] array)
        {
            float max = array[0];
            foreach (var element in array)
            {
                if (element >= max) max = element;
            }
            return max;
        }
        /// <summary>
        /// (int[])array의 엘리먼츠중 최솟값을 반환합니다.
        /// </summary>
        public static int GetMin(int[] array)
        {
            int min = array[0];
            foreach (var element in array)
            {
                if (element <= min) min = element;
            }
            return min;
        }
        /// <summary>
        /// (int[])array의 엘리먼츠중 최댓값을 반환합니다.
        /// </summary>
        public static int GetMax(int[] array)
        {
            int max = array[0];
            foreach (var element in array)
            {
                if (element >= max) max = element;
            }
            return max;
        }

        /// <summary>
        /// (List<float>)list의 엘리먼츠 평균 값을 반환합니다.
        /// </summary>
        public static float GetAverageOf(List<float> list)
        {
            var sum = 0f;
            foreach (var element in list)
                sum += element;
            return sum / list.Count;
        }
        /// <summary>
        /// (List<int>)list의 엘리먼츠 평균 값을 반환합니다.
        /// </summary>
        public static int GetAverageOf(List<int> list)
        {
            var sum = 0;
            foreach (var element in list)
                sum += element;
            return sum / list.Count;
        }
        /// <summary>
        /// (float)x의 시그모이드 함수 반환값을 반환합니다
        /// </summary>
        public static float Sigmoid(float x)
        {
            return 1 / (1 + MathF.Exp(-x));
        }
        /// <summary>
        /// (float)x의 디시그모이드 함수 반환값을 반환합니다
        /// </summary>
        public static float DeSigmoid(float x)
        {
            return x * (1 - x);
        }
        #endregion

        #region VECTORS
        /// <summary>
        /// (Vector2f)vector의 크기를 반환합니다.
        /// </summary>
        public static float GetMagnitude(Vector2f vector)
        {
            return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        /// <summary>
        /// (Vector2f)vector의 크기를 반환합니다.
        /// </summary>
        public static float GetMagnitude(Vector3f vector)
        {
            return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        }

        /// <summary>
        /// (Vector2f)vector의 크기를 (float)mag로 한정한 값을 반환합니다.
        /// </summary>
        public static Vector2f SetMagnitude(Vector2f vector, float mag)
        {
            vector = Normalize(vector);
            vector *= mag;
            return vector;
        }

        /// <summary>
        /// (Vector2f)vector의 크기를 (float)mag로 한정한 값을 반환합니다.
        /// </summary>
        public static Vector3f SetMagnitude(Vector3f vector, float mag)
        {
            vector = Normalize(vector);
            vector *= mag;
            return vector;
        }

        /// <summary>
        /// 정규화된 (Vector2f)vector를 반환합니다.
        /// </summary>
        public static Vector2f Normalize(Vector2f vector)
        {
            var magnitude = GetMagnitude(vector);
            return vector / magnitude;
        }

        /// <summary>
        /// 정규화된 (Vector3f)vector를 반환합니다.
        /// </summary>
        public static Vector3f Normalize(Vector3f vector)
        {
            var magnitude = GetMagnitude(vector);
            return vector / magnitude;
        }

        /// <summary>
        /// (Vector2f)pos1과 (Vector2f)pos2 사이의 거리를 반환합니다.
        /// </summary>
        public static float Distnace(Vector2f pos1, Vector2f pos2)
        {
            return MathF.Sqrt
            (
                MathF.Pow(pos1.X - pos2.X, 2) +
                MathF.Pow(pos1.Y - pos2.Y, 2)
            );
        }

        /// <summary>
        /// (Vector3f)pos1과 (Vector3f)pos2 사이의 거리를 반환합니다.
        /// </summary>
        public static float Distnace(Vector3f pos1, Vector3f pos2)
        {
            return MathF.Sqrt
            (
                MathF.Pow(pos1.X - pos2.X, 2) +
                MathF.Pow(pos1.Y - pos2.Y, 2) +
                MathF.Pow(pos1.Z - pos2.Z, 2)
            );
        }

        /// <summary>
        /// (Vector2)vector의 원점을 기준으로 한 라디안 각도를 반환합니다.
        /// </summary>
        public static float GetAngle(Vector2f vector)
        {
            return MathF.Atan2(vector.Y, vector.X);
        }

        /// <summary>
        /// 라디안각 (float)angle만큼 회전시킨 (Vector2f)vector를 반환합니다.
        /// </summary>
        public static Vector2f RotateVector(Vector2f vector, float angle)
        {
            return new Vector2f
            (
                MathF.Cos(angle) * vector.X -
                MathF.Sin(angle) * vector.Y,
                MathF.Sin(angle) * vector.X +
                MathF.Cos(angle) * vector.Y
            );
        }

        /// <summary>
        /// 각 축을 라디안각 (Vector3f)rotation의 축만큼 회전시킨 (Vector3f)vector를 반환합니다.
        /// </summary>
        public static Vector3f RotateVector(Vector3f vector, Vector3f rotation)
        {
            float thetaZ = rotation.Z;
            vector = new Vector3f(
                MathF.Cos(thetaZ) * vector.X - MathF.Sin(thetaZ) * vector.Y,
                MathF.Sin(thetaZ) * vector.X + MathF.Cos(thetaZ) * vector.Y,
                vector.Z
            );
            float thetaY = rotation.X;
            vector = new Vector3f(
                MathF.Cos(thetaY) * vector.X - MathF.Sin(thetaY) * vector.Z,
                vector.Y,
                MathF.Sin(thetaY) * vector.X + MathF.Cos(thetaY) * vector.Z
            );
            float thetaX = rotation.Y;
            return new Vector3f(
                vector.X,
                MathF.Cos(thetaX) * vector.Y - MathF.Sin(thetaX) * vector.Z,
                MathF.Sin(thetaX) * vector.Y + MathF.Cos(thetaX) * vector.Z
            );
        }

        /// <summary>
        /// (Vector3f)vector1의 각 축마다 (Vector3f)vector2의 축을 곱한 값을 반환합니다.
        /// </summary>
        public static Vector3f Multiply(Vector3f vector1, Vector3f vector2)
        {
            return new Vector3f(vector1.X * vector2.X, vector1.Y * vector2.Y, vector1.Z * vector2.Z);
        }
        #endregion

        #region RANDOM
        public static float Random(float min, float max)
        {
            Random rnd = new Random();
            return Map((float)rnd.NextDouble(), 0, 1, min, max);
        }
        public static float Random(float max)
        {
            Random rnd = new Random();
            return Map((float)rnd.NextDouble(), 0, 1, 0, max);
        }
        public static float Random(float min, float max, int seed)
        {
            Random rnd = new Random(seed);
            return Map((float)rnd.NextDouble(), 0, 1, min, max);
        }
        public static float Random(float max, int seed)
        {
            Random rnd = new Random(seed);
            return Map((float)rnd.NextDouble(), 0, 1, 0, max);
        }
        public static T Random<T>(List<T> list)
        {
            return list[(int)Random(list.Count - 0.01f)];
        }
        public static T Random<T>(T[] array)
        {
            return array[(int)Random(array.Length - 0.01f)];
        }
        #endregion

        #region DATA STRUCTURES 
        public static float[][] ColVec(params float[] cols)
        {
            var data = new float[1][];
            data[0] = cols;
            return data;
        }
        #endregion
    }

}