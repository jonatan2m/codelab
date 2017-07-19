#include <SPI.h>
#include <Ethernet.h>
#include "Timer.h"

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
EthernetClient client;
boolean signal = true;
const int FAIL_PIN = 2, SUCCESS_PIN = 3;

void playAvalie()
{
  String url = "GET /HealthCheck/Index?_=";
  url += random(1, 999999);
  url += " HTTP/1.1";
  char server[] = "play.avalie.net";

  if (connectToServer(server, url)) {
    readMessage();
  }
}

//Timer T1(Blink1, 88000, _US_); // 88000us (NOT ms)
Timer T2(playAvalie, 10000);  // 1000ms
//Timer T3(Blink3, 2000); // 2000ms

void setup()
{
  Serial.begin(9600);
  Ethernet.begin(mac);

  Serial.println("connecting...");

  pinMode(SUCCESS_PIN, OUTPUT);
  pinMode(FAIL_PIN, OUTPUT);
  //T1.start()    ; // optional to use the .start( ) function
  //delay(2568); // On Purpose for you  to compare T1 vs. T2, T3
}

void loop()
{
  if (!signal) {
    digitalWrite(FAIL_PIN, !digitalRead(FAIL_PIN));
    delay(1000);
  }

  T2.check();
}

boolean connectToServer(char server[], String url) {
  stopAllPinStatus();
  client.stop();
  //force clean cache
  int result = client.connect("", 80);

  result = client.connect(server, 80);
  if (result) {
    signal = true;
    Serial.println("connected");

    client.println(url);
    String host = "Host: ";
    host += server;
    client.println(host);
    client.println("Connection: close");
    client.println();
    while (client.connected() && !client.available()) delay(1); //waits for data
    return true;
  } else {
    Serial.println("connection failed");
    signal = false;
    return false;
  }
}
void readMessage() {
  if (!signal) return;

  String message = "";
  boolean success = false;

  while (client.available()) {
    char c = client.read();

    if (c == '\n' && message.startsWith("HTTP")) {
      client.stop();
      break;
    } else {
      message += c;
    }
  }
  success = message.indexOf("200") > 0;

  if (success) {
    successRequestPinStatus();
  } else {
    failRequestPinStatus();
  }

  Serial.println(message);
}

void successRequestPinStatus() {
  int pulse = 10;
  while((pulse--) > 0){
    digitalWrite(SUCCESS_PIN, !digitalRead(SUCCESS_PIN));
    delay(100);
  }
  digitalWrite(FAIL_PIN, false);
  //digitalWrite(SUCCESS_PIN, true);
}

void failRequestPinStatus() {
  digitalWrite(FAIL_PIN, true);
  digitalWrite(SUCCESS_PIN, false);
}
void stopAllPinStatus() {
  digitalWrite(FAIL_PIN, false);
  digitalWrite(SUCCESS_PIN, false);
}

