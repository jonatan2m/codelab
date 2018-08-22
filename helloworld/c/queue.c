#include <stdlib.h>
#include <stdio.h>
#include <utils.h>
#include <queue.h>

cel_queue* qhead = NULL;
cel_queue* qtail = NULL;

int _qsize = 0;

void enqueue(int value) {
    if(qhead == NULL){
        qhead = malloc(sizeof(cel_queue));
        qhead->conteudo = value;
        qhead->seg = NULL;
        
        qtail = qhead;
    }else{
        cel_queue* newItem = malloc(sizeof(cel_queue));
        newItem->conteudo = value;
        newItem->seg = NULL;
        qtail->seg = newItem;
        qtail = newItem;
    }
    
    _qsize++;
}

int dequeue(){
    cel_queue* current = qhead;
    qhead = qhead->seg;    
    _qsize--;
    return current->conteudo;
}