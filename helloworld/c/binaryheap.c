//HeapMax implementation

#include <stdlib.h>
#include <utils.h>

char *harr;
int N = 0;

void create(int capacity) {
    harr = malloc(capacity * sizeof( char ));
}

void insertHeap(char item) {
    *(harr + N) = item;
    swim(N);
    N++;    
}

void swim(int k) {
    //verificar se o pai é menor que o filho
    
    while(k > 0 && less((k-1)/2, k)) {        
        //se for, troca de lugar    
        exch(k, (k-1)/2);
        //define o pai como novo k
        k = (k-1)/2;
    }
}

int less(int parent, int child) {
    char x = *(harr + parent);
    char y = *(harr + child);
    return x < y;
}
void exch(int from, int to) {
    char temp = *(harr + from);
    *(harr + from) = *(harr + to);
    *(harr + to) = temp;
}

char deleteHeap() {
    char item = *(harr);
    exch(0, --N);
    *(harr + N ) = NULL;
    sink(0);
    return item;
}

void sink(k) {
    int left = (2*k + 1);
    if(left < N && less(k, left)){
        exch(k, left);
        sink(left);
    }
    int right = (2*k + 2);
    if(right < N && less(k, right)){
        exch(k, right);
        sink(right);
    }
}

void printHeap(){
    printf("%d", N);
}