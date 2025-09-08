/*
Aplicando esse caso em produção, com sistema distribuído
1) Redis (simples e muito usado)
    Use um Sorted Set por cartão com score = timestamp (epoch millis).

    Operações atômicas (pode ser um script Lua):
    ZADD key timestamp txId
    ZREMRANGEBYSCORE key -inf (now-30s)
    ZCARD key → se ≥ 10, sinaliza fraude.

    Vantagens: simples, rápido, atômico; expiração de chave mantém memória sob controle.
    Dica: adicione também EXPIRE key 300 para limpar cartões inativos.

    2) Streams (Kafka + Flink/Spark/Kafka Streams)

    KeyBy(cardId) → todas transações do cartão vão para a mesma partição.

    Time-based sliding window (30s) com event-time + watermarks.

    Estado local (RocksDB) mantém janelas por chave.

    Emite um “alerta de fraude” quando o count na janela ≥ 10.
*/
public sealed class FraudSlidingWindow
{
    private readonly TimeSpan _window = TimeSpan.FromSeconds(30);
    private readonly int _threshold; // ex.: 10
    private readonly object _lock = new();
    // Para produção, prefira ConcurrentDictionary + lock por chave
    private readonly Dictionary<string, Deque<DateTimeOffset>> _perCard = new();

    public FraudSlidingWindow(int threshold = 10)
    {
        _threshold = threshold;
    }

    // Retorna true se excedeu o limite na janela (suspeita de fraude)
    public bool Register(string cardId, DateTimeOffset eventTime)
    {
        lock (_lock)
        {
            if (!_perCard.TryGetValue(cardId, out var dq))
            {
                dq = new Deque<DateTimeOffset>();
                _perCard[cardId] = dq;
            }

            // 1) Enfileira o novo evento
            dq.AddToBack(eventTime);

            // 2) Desliza a janela: remove eventos < (eventTime - 30s)
            var minTime = eventTime - _window;
            while (dq.Count > 0 && dq.Front < minTime)
                dq.RemoveFromFront();

            // 3) Checa a contagem
            return dq.Count >= _threshold;
        }
    }
}

// Deque simples; pode usar uma lib, ou List<T> com dois índices.
/*
Deque = Double-Ended Queue 
*/
public class Deque<T>
{
    private readonly LinkedList<T> _ll = new();
    public int Count => _ll.Count;
    public T Front => _ll.First!.Value;
    public void AddToBack(T v) => _ll.AddLast(v);
    public void RemoveFromFront() { if (_ll.First is not null) _ll.RemoveFirst(); }
}
