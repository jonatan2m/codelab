using System;
using System.Collections.Generic;
using System.Text;

namespace CodingInterview
{
    internal class LeftRotation
    {
        private int[] arrExample;

        public LeftRotation(int[] arrExample)
        {
            this.arrExample = arrExample;
        }

        internal void Rotate(int times)
        {
            int nextIndex = times;
            
            while(nextIndex < arrExample.Length)
            {
                for (int i = nextIndex; i > nextIndex - times; i--)
                {
                    var element = arrExample[i];
                    arrExample[i] = arrExample[i - 1];
                    arrExample[i - 1] = element;
                }
                nextIndex++;
            }
        }        

        internal string RotateTrick(int times)
        {
            List<string> results = new List<string>();

            for (int i = times; i < arrExample.Length; i++)
            {
                results.Add(arrExample[i].ToString());
            }

            for (int i = 0; i < times; i++)
            {
                results.Add(arrExample[i].ToString());
            }

            return String.Join(" ", results);
        }

        internal void RotateDotNet(int times)
        {
            int[] rotated = new int[arrExample.Length];

            System.Array.Copy(arrExample, times, rotated, 0, arrExample.Length - times);
            System.Array.Copy(arrExample, 0, rotated, arrExample.Length - times, times);

            arrExample = rotated;
        }

        internal int[] Array()
        {
            return arrExample;
        }

        internal string Print()
        {
            return String.Join(" ", arrExample);
        }
    }
}