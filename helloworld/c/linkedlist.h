struct cel {
    int conteudo;
    struct cel *seg;
};

typedef struct cel celula;

void add(int value);
void print();
int size();
int value_at(int position);
void push_front(int value);
int pop_front();
int push_back(int value);