#include <math.h>
#include <assert.h>
#include <stdio.h>
#include <stdlib.h>

#include <pointer.h>
#include <array.h>
#include <linkedlist.h>
#include <queue.h>
#include <binaryheap.h>


int lg(int n) {
    return log2(n);
}
void assert_lg() {
    assert(lg(15) == 3);    
    assert(lg(16) == 4);
    assert(lg(31) == 4);
    assert(lg(32) == 5);
    assert(lg(63) == 5);
    assert(lg(511) == 8);
    assert(lg(512) == 9);
}
int maximoR1(int v[], int n) {
    if(n == 1)
        return v[0];
    
    int x = maximoR1(v, n - 1);
    return x > v[n - 1] ? x : v[n - 1];
}
int maximoR(int v[], int n) {
    int x;
    if (n == 1) return v[0];
    if (n == 2) {
        if (v[0] < v[1]) return v[1];
        else return v[0];
    }
    x = maximoR(v, n -1);
    if(x < v[n - 1]) return v[n-1];
    else return x;
}
void assert_maximoR() {
    int ar1[1] = {5};
    assert(maximoR(ar1, 1) == 5);
    int ar5m[5] = {5,6,8,2,1};
    assert(maximoR(ar5m, 5) == 8);
    
    int ar5i[5] = {8,6,5,2,1};
    assert(maximoR(ar5i, 5) == 8);
    
    int ar5f[5] = {1,6,5,2,8};
    assert(maximoR(ar5f, 5) == 8);
}
int fib(int n) {
    if(n == 0) return 0;
    if(n == 1) return 1;
    
    return fib(n-1) + fib(n-2);
}
void assert_fib() {   
    assert(fib(3) == 2);
    assert(fib(7) == 13);
}
int euclides(int m, int n) {
    if(n == 0) return m;
    return euclides(n, m % n);
}
void assert_euclides() {
    assert(euclides(13, 2) == 1);
    assert(euclides(13, 8) == 1);
    assert(euclides(16, 8) == 8);
    assert(euclides(48, 30) == 6);
}
int expo(int k, int n) {
    if(n == 0) return 1;
    return k * expo(k, n - 1);
}
void assert_expo() {    
    assert(expo(2, 3) == 8);
    assert(expo(2, 10) == 1024);
}
void vetores() {
    //alocado dinamicamente
    int *v;
    v = malloc(5 * sizeof (int));
    *(v+1) = 12;
    printf("%d ", *(v+1));
    free(v);
    
    v = malloc(2 * sizeof(int));
    *(v+0) = 0;
    *(v+1) = 1;
    printf("%d ", *(v+1));
    v = realloc(v, 4 * sizeof(int));
    *(v+2) = 2;
    printf("%d ", *(v+2));
    v = malloc(2 * sizeof(int));
    printf("%d\n", *(v+2));    
}

void pointer(){
    ex1();
    ex2();
    ex3();
    ex4();
    ex5();
    ex6();
    int h, m;
    hm(150, &h, &m);
    printf("hours: %d and minutes: %d\n", h, m);
}

void array() {
    ar1();
    ar2();
    ar3();
}

void linkedlist(){
    celula *c = list();
    printf("PRINT LINKEDLIST %d ", c->conteudo);
    add(1);
    add(2);
    add(3);    
    add(4);
    add(5);    
    print();
    printf("size linkedlist: %d\n", size());
    printf("value_at 3: %d\n", value_at(3));
    push_front(6);
    print();
    printf("pop_front: 6 %d\n", pop_front());
    print();
}

void queue(){
    enqueue(1);
    enqueue(2);
    enqueue(3);
    printf("dequeue: 1 -> %d\n", dequeue());
    printf("dequeue: 2 -> %d\n", dequeue());
}

void binaryheap(){
    create(10);
    insertHeap('a');
    insertHeap('b');
    insertHeap('d');
    insertHeap('f');
    insertHeap('e');
    printf("remove heap %c", deleteHeap());
    insertHeap('j');
    insertHeap('f');
    insertHeap('x');
    printf("remove heap %c", deleteHeap());
    printHeap();
}

void hashtable_() {
    ht1();
    ht2("abacdb");
}


int main(int argc, char **argv)
{       
	printf("hello world\n");
    pointer();
    array();
    linkedlist();
    queue();
    binaryheap();
    hashtable_();
    binarySearchTree();
    /*assert_lg();
    assert_maximoR();
    assert_fib();
    assert_euclides();
    assert_expo();
    vetores();*/
    
	return 0;
}