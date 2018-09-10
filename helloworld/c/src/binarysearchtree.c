#include <stdlib.h>
//prefix all method as bs_[name_of_method]
struct node
{
    int data;
    struct node* left;
    struct node* right;
};

typedef int (*comparer)(int, int);

/*
    create a new node
*/
struct node* create_node(int data)
{
    struct node* new_node = (struct node*)malloc(sizeof(struct node));
    if(new_node == NULL)
    {
        printf("Out of memory!!! (create_node)\n");
        exit(1);
    }
    new_node->data = data;
    new_node->left = NULL;
    new_node->right = NULL;
    return(new_node);
}

void printInorder(struct node* node) {
    if(node == NULL)
        return;
    
    printInorder(node->left);    
    printf("%d ", node->data);    
    printInorder(node->right);
}

void printPreOrder(struct node* node) {
    if(node == NULL)
        return;
    printf("%d ", node->data);
    printPreOrder(node->left);
    printPreOrder(node->right);
}

void printPostOrder(struct node* node) {
    if(node == NULL)
        return;    
    printPostOrder(node->left);
    printPostOrder(node->right);
    printf("%d ", node->data);
}

void printGivenLevel(struct node* node) {
    if(node == NULL)
        return;
        
    if(node->left != NULL)
        printf("%d ", node->left->data);
    if(node->right != NULL)
        printf("%d ", node->right->data);
}


void printLevelOrder(struct node* node) {
    if(node == NULL)
        return;
    
    printf("%d ", node->data);
    printGivenLevel(node);
    if(node->left != NULL) {
        printLevelOrder(node->left->left);
        printLevelOrder(node->left->right);    
    }
    if(node->right != NULL){
        printLevelOrder(node->right->left);
        printLevelOrder(node->right->right);
    }
}

struct node* teste_delete(struct node* node, int value) {
    if(node == NULL) return node;
    
    if(node->data == value) {
        if(node->left == NULL && node->right == NULL){            
            free(node);            
            return NULL;
        }
        else if(node->left != NULL && node->right != NULL) {
            struct node *next = node->right;
            while(next->left != NULL) next = next->left;            
            node->data = next->data;
            teste_delete(node->right, next->data);
        }
        else if(node->left != NULL)
            node = node->left;
        else if(node->right != NULL)
            node = node->right;            
    }
    else if(value < node->data)
        node->left = teste_delete(node->left, value);
    else
        node->right = teste_delete(node->right, value);
    
    return node;
}

void binarySearchTree(){
    /*struct node* root = create_node(1);
    root->left            = create_node(2);    
    root->right           = create_node(3);
    root->right->right    = create_node(6);
    root->left->left      = create_node(4);
    root->left->right     = create_node(5); */
    struct node* root = create_node(5);
    root->left            = create_node(3);    
    root->right           = create_node(10);
    root->right->left     = create_node(9);
    root->right->right    = create_node(15);
    root->right->right->left  = create_node(11);
    root->right->right->right = create_node(20);
    root->left->left      = create_node(2);
    root->left->right     = create_node(4); 
    root = teste_delete(root, 2);
    printLevelOrder(root);
}