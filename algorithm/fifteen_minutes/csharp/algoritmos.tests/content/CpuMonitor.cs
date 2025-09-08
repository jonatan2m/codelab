/// <summary>
/// A cada segundo você chama AddMeasurement(cpu).
/// Ele guarda no máximo os últimos N segundos.
/// Mantém a soma parcial em _currentSum.
/// Retorna a média móvel instantânea em O(1).
/// </summary>
public class CpuMonitor
{
    private readonly Queue<int> _window = new();
    private readonly int _windowSize;
    private long _currentSum = 0;

    public CpuMonitor(int windowSizeInSeconds)
    {
        _windowSize = windowSizeInSeconds;
    }

    public double AddMeasurement(int cpuUsagePercent)
    {
        // adiciona nova medição
        _window.Enqueue(cpuUsagePercent);
        _currentSum += cpuUsagePercent;

        // remove valor antigo se estourou a janela
        if (_window.Count > _windowSize)
        {
            _currentSum -= _window.Dequeue();
        }

        // retorna a média atual
        return (double)_currentSum / _window.Count;
    }
}
