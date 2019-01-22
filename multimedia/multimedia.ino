#include <NewPing.h>
#include <Wire.h>
#include <Servo.h>

/* 
 *  
  Pin  Anschluss
  12  2 Color LED RG R
  A0  Lichtsensor
  ~3  Magnet
  8   Ultraschall Trig
  ~11 Ultraschall Echo
  7   Klingle Taster
  A1  Potenziometer
  5   dachfenster Servo Pin

  Gyro:
  VCC to 5V(MPU-6050 works with 3.3V but GY-521 increases it to 5V.),
  GND to GND,
  SCL to A5,
  SDA to A4,
  ADO to GND,
  INT to digital pin 2.
 */

int PinPotiTuer = A1;
int PinKlingel = 7;
int PinUltraEcho = 11;
int PinUltraTrig = 8;
int PinMagnet = 3;
int PinLichtsensor = A0;
int PinAlarmLED = 12;
int PinAlarmBuzzer = 4;
int PinDachfensterServo = 5;

/* Alarm */
bool isAlarm = false;
const long interval = 250;  
int alarmOutputState = LOW; 
unsigned long previousMillis = 0;        // will store last time LED was updated

/* Gyro */
const int MPU_addr=0x68;
int16_t AcX,AcY,AcZ,Tmp,GyX,GyY,GyZ;

int minVal=265;
int maxVal=402;

double x;
double y;
double z;

/* dachfenster */
Servo dachfensterServo;

#define MAX_DISTANCE 200 // Maximum distance we want to ping for (in centimeters). Maximum sensor distance is rated at 400-500cm.

NewPing sonar(PinUltraTrig, PinUltraEcho, MAX_DISTANCE); // NewPing setup of pins and maximum distance.

int incomingByte = 0;

void setup() {
  /* Gyro setup */
  Wire.begin();
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);

  
  Serial.begin(9600);
  pinMode(PinKlingel, INPUT);
  pinMode(PinMagnet, INPUT);
  pinMode(PinAlarmLED, OUTPUT);
  pinMode(PinAlarmBuzzer, OUTPUT);

  dachfensterServo.attach(PinDachfensterServo);
  // open dachfenster at beginning
  dachfensterServo.write(0);
}

void loop() {
  delay(29);  // Wait 500ms between pings (about 2 pings/sec). 29ms should be the shortest delay between pings.

  if (Serial.available() > 0) {
    // read the incoming byte:
    incomingByte = Serial.read();
    if(incomingByte == 'u'){
      // recived u => open Dachfenster
      dachfensterServo.write(0);
    }
    else if(incomingByte == 'd'){
      // recived d => close Dachfenster
      dachfensterServo.write(180);
    }
  }
  
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

  readGyro();
  
  Serial.println(String(potiTuer) + "," + String(klingelState) + "," + String(sonarCM) + "," + String(isAlarm) + ","   
    + String(magnetState) + "," 
    + String(lichtsensorState) + ","
    /* Gyro x, y, z*/
    + String(x) + ","
    + String(y) + ","
    + String(z)
    );
  Serial.flush();
}

void readGyro(){
  Wire.beginTransmission(MPU_addr);
  Wire.write(0x3B);
  Wire.endTransmission(false);
  Wire.requestFrom(MPU_addr,14,true);
  AcX=Wire.read()<<8|Wire.read();
  AcY=Wire.read()<<8|Wire.read();
  AcZ=Wire.read()<<8|Wire.read();
  int xAng = map(AcX,minVal,maxVal,-90,90);
  int yAng = map(AcY,minVal,maxVal,-90,90);
  int zAng = map(AcZ,minVal,maxVal,-90,90);

  x = RAD_TO_DEG * (atan2(-yAng, -zAng)+PI);
  y = RAD_TO_DEG * (atan2(-xAng, -zAng)+PI);
  z = RAD_TO_DEG * (atan2(-yAng, -xAng)+PI);
}
