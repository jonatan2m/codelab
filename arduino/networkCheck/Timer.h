/*
	Timer.h - Library to support run jobs
*/

#ifndef Timer_h
#define Timer_h

#include "Arduino.h"

class Timer
{
  private:
    unsigned long lastTime;  // the latest time had doing Func( )
    void (*Func) (void);
    void (*backupFunc) (void);  // backup Func for start( )
    unsigned long T_int;   // interval in ms, can be set as in us (optional)

  public:    
    Timer(void (*userFunc)(), unsigned long T, bool isTinUs = false);
    void stop();
    void check();
    void start();    

#define _MS_ false /*Milliseconds*/
#define _US_ true  /*Microseconds*/
};

#endif