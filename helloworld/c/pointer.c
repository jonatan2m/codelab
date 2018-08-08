#include <stdio.h>
#include <time.h>
#include <utils.h>

void ex1() {
   int  var1;
   char var2[10];

   printf("Address of var1 variable: %x\n", &var1  );
   printf("Address of var2 variable: %x\n", &var2  );
   line();
}

void ex2() {
    int  var = 20;   /* actual variable declaration */
    int  *ip;        /* pointer variable declaration */

    ip = &var;  /* store address of var in pointer variable*/

    printf("Address of var variable: %x\n", &var  );

    /* address stored in pointer variable */
    printf("Address stored in ip variable: %x\n", ip );

    /* access the value using the pointer */
    printf("Value of *ip variable: %d\n", *ip );
    
    line();    
}

void ex3() {
    int  *ptr = NULL;
    printf("The value of ptr is : %x\n", ptr  );
    //if(ptr)     /* succeeds if p is not null */
    //if(!ptr)    /* succeeds if p is null */
    line();
}

void ex4() {
    
   int  var[] = {10, 100, 200};
   const int MAX = 3;
   int  i, *ptr;

   /* let us have array address in pointer */
   ptr = var;
	
   for ( i = 0; i < MAX; i++) {

      printf("Address of var[%d] = %x\n", i, ptr );
      printf("Value of var[%d] = %d\n", i, *ptr );

      /* move to the next location */
      ptr++;      
   }  
   line();
   
   /* let us have address of the first element in pointer */
   ptr = var;
   i = 0;
	
   while ( ptr <= &var[MAX - 1] ) {

      printf("Address of var[%d] = %x\n", i, ptr );
      printf("Value of var[%d] = %d\n", i, *ptr );

      /* point to the previous location */
      ptr++;
      i++;
   }
   line();
}

void ex5() {
    int  var[] = {10, 100, 200};
    const int MAX = 3;
    int i, *ptr[MAX];
 
   for ( i = 0; i < MAX; i++) {
      ptr[i] = &var[i]; /* assign the address of integer. */
   }
   
   for ( i = 0; i < MAX; i++) {
      printf("Value of var[%d] = %d\n", i, ptr[i] );
   }
   line();
    char *names[] = {
      "Zara Ali",
      "Hina Ali",
      "Nuha Ali",
      "Sara Ali"
   };
   
   for ( i = 0; i < 4; i++) {
      printf("Value of names[%d] = %s\n", i, names[i] );
   }
   line();
}

void ex6() {
    unsigned long sec;
    getSeconds( &sec );

    /* print the actual value */
    printf("Number of seconds: %ld\n", sec );
    line();
}

void hm(int minutes, int *h, int *m) {
    *h = 0;
    
    while(minutes >= 60) {
        minutes -= 60;
        *h += 1;
    }
    *m = minutes;
    line();
}

void getSeconds(unsigned long *par) {
   /* get the current number of seconds */
   *par = time( NULL );   
}