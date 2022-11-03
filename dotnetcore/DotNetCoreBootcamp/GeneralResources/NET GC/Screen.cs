// //-----------------------------------------------------------------------------
// // <copyright file="Screen.cs" company="DCOM Engineering, LLC">
// //     Copyright (c) DCOM Engineering, LLC.  All rights reserved.
// // </copyright>
// //-----------------------------------------------------------------------------
namespace NET_GC
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static class Screen
    {
        public static void Print(IEnumerable<DebugAllocationData> data)
        {
            int cursorLeft = Console.CursorLeft;
            int cursorTop  = Console.CursorTop;

            string col1 = "Before Allocation (Bytes)",
                col2    = "After Allocation (Bytes)",
                col3    = "After Dispose (Bytes)";

            Console.WriteLine($"{col1}    {col2}    {col3}{Environment.NewLine}");

            DebugAllocationData previousData = null;

            foreach (var row in data)
            {
                string s = "";

                s += $"{row.BeforeAllocBytes.ToString().PadRight(col1.Length, ' ')}";
                s += "    ";
                s += $"{row.AfterAllocBytes.ToString().PadRight(col2.Length, ' ')}";
                s += "    ";
                s += $"{row.AfterDisposeBytes.ToString().PadRight(col3.Length, ' ')}";

                Console.Write(s);

                if (previousData != null)
                {
                    var isGreater = row.AfterDisposeBytes >= previousData.AfterDisposeBytes;
                    var change = (long)0;

                    if (isGreater)
                    {
                        change = previousData.AfterDisposeBytes - row.AfterDisposeBytes;
                    }
                    else
                    {
                        change = row.AfterDisposeBytes - previousData.AfterDisposeBytes;
                    }

                    change = Math.Abs(change) / 1024;

                    Console.ForegroundColor = isGreater ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.Write($"{(isGreater ? "-" : "+")} {change} kb");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                previousData = row;
                
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}