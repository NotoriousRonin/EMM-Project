#include <NewPing.h>

/* 
 *  
  Pin  Anschluss
  12  2 Color LED RG R
  A0  Lichtsensor
  ~3  Magnet
  8 Ultraschall Trig
  ~11 Ultraschall Echo
  7 Klingle Taster
  A1  Potenziometer
 */

int PinPotiTuer = A1;
int PinKlingel = 7;
int PinUltraEcho = 11;
int PinUltraTrig = 8;
int PinMagnet = 3;
int PinLichtsensor = A0;
int PinAlarmLED = 12;
int PinAlarmBuzzer = 4;

/* Alarm */
bool isAlarm = false;
const long interval = 250;  
int alarmOutputState = LOW; 
unsigned long previousMillis = 0;        // will store last time LED was updated

#define MAX_DISTANCE 200 // Maximum distance we want to ping for (in centimeters). Maximum sensor distance is rated at 400-500cm.

NewPing sonar(PinUltraTrig, PinUltraEcho, MAX_DISTANCE); // NewPing setup of pins and maximum distance.


void setup() {
  Serial.begin(9600);
  pinMode(PinKlingel, INPUT);
  pinMode(PinMagnet, INPUT);
  pinMode(PinAlarmLED, OUTPUT);
  pinMode(PinAlarmBuzzer, OUTPUT);
}

void loop() {
  delay(29);  // Wait 500ms between pings (about 2 pings/sec). 29ms should be the shortest delay between pings.
  
  int potiTuer = analogRead(PinPotiTuer);
  int klingelState = digitalRead(PinKlingel);
  
  unsigned int uS = sonar.ping(); // Send ping, get ping time in microseconds (uS).
  int sonarCM = uS / US_ROUNDTRIP_CM; // in cm


  isAlarm = sonarCM > 8;
  unsigned long currentMillis = millis();
  if(isAlarm){
    if (currentMillis - previousMillis >= interval) {
      // save the last time you blinked the LED
      previousMillis = currentMillis;
  
      // if the LED is off turn it on and vice-versa:
      if (alarmOutputState == LOW) {
        alarmOutputState = HIGH;
      } else {
        alarmOutputState = LOW;
      }
  
      // set the LED with the ledState of the variable:
      digitalWrite(PinAlarmLED, alarmOutputState);
      digitalWrite(PinAlarmBuzzer, alarmOutputState);
    }

  }
  else{
    digitalWrite(PinAlarmBuzzer, LOW);
    digitalWrite(PinAlarmLED, LOW);
  }
  

  int magnetState = digitalRead(PinMagnet);

  int lichtsensorState = analogRead(PinLichtsensor);
  Serial.println(String(potiTuer) + "," + String(klingelState) + "," + String(sonarCM) + "," + String(isAlarm) + ","   
    + String(magnetState) + "," 
    + String(lichtsensorState));
  Serial.flush();
}
