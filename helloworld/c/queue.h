struct celq {
    int conteudo;
    struct celq *seg;
};

typedef struct celq cel_queue;

void enqueue(int value);
int dequeue();