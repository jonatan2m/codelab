#include <stdlib.h>
#include <stdio.h>
#include <utils.h>
#include <linkedlist.h>

int _size = 0;
celula *head = NULL;
celula *tail = NULL;

void add(int value) {
    if(head == NULL) {
        head = malloc(sizeof(celula));
        head->conteudo = value;        
        head->seg = NULL;        
        
        tail = malloc(sizeof(celula));
        tail->conteudo = value;        
        tail->seg = NULL;                        
    }else {
        celula *current = NULL;
        current = head;
        while (current->seg != NULL){
            current = current->seg;
        }        
        
        tail->conteudo = value;
        tail->seg = NULL;
        current->seg = malloc(sizeof(celula));
        current->seg->conteudo = value;
        current->seg->seg = NULL;
    }
    _size += 1;
}

void print() {
    line();
    celula *current = NULL;
    current = head;
    while(current != NULL){
        printf("-> %d\n", current->conteudo);
        current = current->seg;
    }
}

int value_at(int position) {
    if(_size < (position - 1)) return EXIT_FAILURE;
    celula* item = head;
    for(int i = 1; i < position; i++) {
        item = item->seg;
    }
    if(item != NULL) return item->conteudo;
    else return EXIT_FAILURE;
}

void push_front(int value) {
    celula *newItem = malloc(sizeof(celula));
    newItem->conteudo = value;
    newItem->seg = head;
    head = newItem;
}

int pop_front(){
    int value = head->conteudo;
    celula* secondItem = head->seg;
    head = secondItem;
    return value;
}

int push_back(int value) {
    return EXIT_SUCCESS;
}

int size() {
    line();
    return _size;
}

celula * list(){
    celula *c;
    c = malloc(sizeof(celula));
    c->conteudo = 23; 
    line();
    return c;
}