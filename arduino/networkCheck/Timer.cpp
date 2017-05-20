/*
  Timer.h - Library to support run jobs
*/

#include "Arduino.h"
#include "Timer.h"

Timer::Timer(void (*userFunc)(), unsigned long T, bool isTinUs = false)
    {
      Func = backupFunc = userFunc;  // save the function pointer
      T_int = T * 1000;
      if (isTinUs)
        T_int = T;  // T is in micro second as per unit

      lastTime = 0;  // initialization should be in constructor
    }
  void Timer::check()
    {
      if ( Func == 0 )
        return; // no function pointer

      unsigned long _micros = micros(); // Local variable could be in Register

      if (_micros - lastTime >= T_int)
      {
        lastTime = _micros;  // latest time doing Func( )
        Func();
      }
    }// check

    void Timer::start( )   // first run of doing Func( )
    {
      if (Func == 0)
        Func = backupFunc; // restore after .stop( )

      lastTime = micros();  // latest time doing Func( )
      Func();   // First run
    } // start

    void Timer::stop( )   // stop the scheduled Func  (userFunc)
    {
      Func = 0;  // set as NULL
    } // stop

