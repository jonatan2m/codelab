//Two-dimensional Arrays

#include <utils.h>

void ar1() {
    int a[3][4] = {  
        {0, 1, 2, 3} ,   /*  initializers for row indexed by 0 */
        {4, 5, 6, 7} ,   /*  initializers for row indexed by 1 */
        {8, 9, 10, 11}   /*  initializers for row indexed by 2 */
    };
    //the same initializer as above
    int b[3][4] = {0,1,2,3,4,5,6,7,8,9,10,11};
    
     /* output each array element's value */
   for ( int i = 0; i < 3; i++ ) {

      for ( int j = 0; j < 4; j++ ) {
         printf("a[%d][%d] = %d b(%d)\n", i,j, a[i][j], b[i][j] );
      }
   }
   line();
   
   int *memo;
   int *c[10];
   memo = c;
   for ( int i = 0; i < 10; i++ ) {
       c[i] = i + 10;
       printf("%d memo(%d) __ %d\n", i, c[i], *(c + i));
       //memo++;
   }
   line();
}

void ar2() {
    int *v, i = 0;
    v = malloc(2 * sizeof( int ));
    *(v + i) = i++;
    *(v + i) = i++;
    
    *(v + i) = i++;
    *(v + i) = i++;
    *(v + i) = i++;
    printf("first value %d and %d\n", v[0], v[1]);
    printf("first value %d and %d\n", v[2], v[3]);
    line();
}

struct node 
{
    int data;
    struct node *next;
};
struct node *head;
void createStruct(){
    struct node *newnode, *temp;
    newnode = (struct node*)malloc(sizeof(struct node));
}