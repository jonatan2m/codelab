//sempre retornar um numero positivo (numero & 0x7FFFFFFF)
//Inteiro de 32 bits: é 2.147.483.647; ou seja, 0x7FFFFFFF hexadecimal.
void ht1(){
    char S[] = "ababcd";
       for(char c = 'a';c <= 'z';++c)
        {
            int frequency = 0;
            for(int i = 0;i < 6;++i)
                if(S[i] == c)
                    frequency++;
            printf("%c %d", c, frequency);
        }
}

int hashFunc(char c)  {
    return (c - 'a');
}

void ht2(char* S){
    int Frequency[26];
    for(int i = 0;i < 26;++i) {
        Frequency[i] = 0;
    }
    
    for(int i = 0;i < sizeof(S);++i) {
        char a = S[i];
        int index = hashFunc(a);
        Frequency[index]++;
    }
    
    for(int i = 0;i < 26;++i)
        printf("%c %d\n", (i + 'a'), Frequency[i]);
        
    
}