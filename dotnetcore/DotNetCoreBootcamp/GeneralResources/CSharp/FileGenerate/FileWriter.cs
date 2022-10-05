using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using BenchmarkDotNet.Attributes;
using CSharp.FileGenerate.Model;

namespace CSharp.FileGenerate
{

    /// <summary>
    /// O framework posicional não permite fazer a geração do arquivo de forma separada.
    /// Gerar uma linha de cada vez
    /// </summary>
    [RPlotExporter, RankColumn]
    public class FileWriter
    {
        private string path = @"C:\teste-processa-arquivos";

        List<HeaderFile> headers = new List<HeaderFile>();
        
        public FileWriter()
        {
            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);

            for (int i = 0; i < 5; i++)
            {
                headers.Add(new HeaderFile { CreatedAt = DateTime.Now, Code = Guid.NewGuid().ToString("N"), Status = 1 });
            }
        }

        public static void Play()
        {
            var fileWriter = new FileWriter();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            GC.CollectionCount(0);
            fileWriter.GenerateFileAllLines();
            fileWriter.PrintGCInfo();
            stopwatch.Stop();
            Console.WriteLine($"GenerateFileAllLines {stopwatch.ElapsedMilliseconds} mls");

            stopwatch.Reset();

            stopwatch.Start();
            fileWriter.GenerateFilePerLine();

            fileWriter.PrintGCInfo();

            stopwatch.Stop();
            Console.WriteLine($"GenerateFilePerLine {stopwatch.ElapsedMilliseconds} mls");
        }

        private void PrintGCInfo()
        {
            Console.WriteLine($"GC.CollectionCount(0,1) {GC.CollectionCount(0)} {GC.CollectionCount(1)} MaxGeneration: {GC.MaxGeneration}");
        }


        [Benchmark]
        public void GenerateFilePerLine()
        {
            using (StreamWriter sw = File.AppendText($"{path}/GenerateFilePerLine"))
            {
                foreach (var headerFile in headers)
                {
                    sw.WriteLine(headerFile.ToString());
                }
            }
        }

        [Benchmark]
        public void GenerateFileAllLines()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var headerFile in headers)
            {
                sb.AppendLine(headerFile.ToString());
            }

            using (StreamWriter sw = File.AppendText($"{path}/GenerateFileAllLines"))
            {
                sw.WriteLine(sb.ToString());
            }
        }
    }
}
