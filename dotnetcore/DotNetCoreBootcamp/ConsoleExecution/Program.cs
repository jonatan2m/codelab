// See https://aka.ms/new-console-template for more information
using GeneralResources.CSharp.AdvancedCollectionsSpan;
using GeneralResources.Paralelismo;

Console.WriteLine("Hello, World!");

//WorkingWithMarshal.ExecuteBenchmarkDotNet();

Console.WriteLine("TaskCompletionSource processes tasks and executes them as tasks finish");
await TestProcessEachTaskJustItFinishs.Test1_OK();
Console.WriteLine("TaskCompletionSource processing in order, causing unnecessary delays");
await TestProcessEachTaskJustItFinishs.Test1_NOK();