using System;
using System.Linq;
using UnityEngine;


namespace LLAPI
{
    public class NetUtils : MonoBehaviour
    {
        public static byte[] ConcatByteArrays(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static byte[] ConcatByteArrays(byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length, third.Length);
            return ret;
        }

        public static byte[] ConcatByteArrays(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                Buffer.BlockCopy(arrays[i], 0, ret, offset, arrays[i].Length);
                offset += arrays[i].Length;
            }
            return ret;
        } //ConcatByteArrays

    }
}
