#include <SPI.h>
#include <Ethernet.h>
#include "Timer.h"

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
EthernetClient client;
char server[] = "play.avalie.net";
boolean signal = true;

void Blink1()
{
  digitalWrite(13, !digitalRead(13));
}

void Blink2()
{
  //digitalWrite(9, !digitalRead(9));
  connectToServer();
  readMessage();
}

void Blink3()
{
  digitalWrite(11, !digitalRead(11));
}


Timer T1(Blink1, 88000, _US_); // 88000us (NOT ms)
Timer T2(Blink2, 10000);  // 1000ms
Timer T3(Blink3, 2000); // 2000ms

void setup()
{
  Serial.begin(9600);
  Ethernet.begin(mac);

  delay(1000);
  Serial.println("connecting...");

  //connectToServer();
  // put your setup code here, to run once:
  pinMode(8, OUTPUT);
  pinMode(11, OUTPUT);
  pinMode(9, OUTPUT);
  //T1.start()    ; // optional to use the .start( ) function
  delay(2568); // On Purpose for you  to compare T1 vs. T2, T3
}

void loop()
{
  /* switch (Ethernet.maintain())
     {
     case 1:
       Serial.println("Error: renewed fail");
       break;
     case 2:
       Serial.println("Renewed success");
       //print your local IP address:
       break;
     case 3:
       Serial.println("Error: rebind fail");
       break;
     case 4:
       Serial.println("Rebind success");
       //print your local IP address:
       break;
     default:
       //nothing happened
       break;
     }*/
  if(!signal){
     digitalWrite(9, !digitalRead(9));
     delay(1000);
  }
  // put your main code here, to run repeatedly:
  //T1.check();
  T2.check();
  T3.check();
}

void connectToServer() {
  client.stop();
  int result = client.connect("",80);
  
  result = client.connect(server, 80);
  Serial.println(result);
  if (result) {
    signal = true;
    Serial.println("connected");
    String url = "GET /HealthCheck/Index?_=";
    url += random(1, 999999);
    url += " HTTP/1.1";
    client.println(url);
    client.println("Host: play.avalie.net");
    client.println("Connection: close");
    client.println();
    while (client.connected() && !client.available()) delay(1); //waits for data
  } else {
    Serial.println("connection failed");
    signal = false;
  }
}
void readMessage() {
  if(!signal) return;
  
  String message = "";
  boolean success = false;
  if (client.available()) {
    //char c = client.read();
    //Serial.print(c);
  }
  while (client.available()) {
    char c = client.read();

    if (c == '\n') {
      if (message.startsWith("HTTP")) {
        client.stop();
        break;
      }
    } else {
      message += c;
    }
  }
  success = message.indexOf("200") > 0;

  if (success) {
    digitalWrite(9, false);
    digitalWrite(8, true);
  } else {
    digitalWrite(9, true);
    digitalWrite(8, false);
  }

  Serial.print(message);
}

