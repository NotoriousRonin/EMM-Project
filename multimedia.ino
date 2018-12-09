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

#define MAX_DISTANCE 200 // Maximum distance we want to ping for (in centimeters). Maximum sensor distance is rated at 400-500cm.

NewPing sonar(PinUltraTrig, PinUltraEcho, MAX_DISTANCE); // NewPing setup of pins and maximum distance.


void setup() {
  Serial.begin(9600);
  pinMode(PinKlingel, INPUT);
  pinMode(PinMagnet, INPUT);
}

void loop() {
  delay(29);  // Wait 500ms between pings (about 2 pings/sec). 29ms should be the shortest delay between pings.
  
  int potiTuer = analogRead(PinPotiTuer);
  int klingelState = digitalRead(PinKlingel);
  
  unsigned int uS = sonar.ping(); // Send ping, get ping time in microseconds (uS).
  int sonarCM = uS / US_ROUNDTRIP_CM; // in cm

  int magnetState = digitalRead(PinMagnet);
  int lichtsensorState = analogRead(PinLichtsensor);
  Serial.println(String(potiTuer) + "," + String(klingelState) + "," + String(sonarCM) + "," + String(magnetState) + "," 
    + String(lichtsensorState));
  Serial.flush();
}
