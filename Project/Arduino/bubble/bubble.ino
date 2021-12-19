#include "Adafruit_CCS811.h"
 
Adafruit_CCS811 ccs;
 
void setup() {
  Serial.begin(9600);
  pinMode(13, INPUT);
  Serial.println("CCS811 test");
 
  if(!ccs.begin()){
    Serial.println("Failed to start sensor! Please check your wiring.");
    while(1);
  }
 
  // Wait for the sensor to be ready
  while(!ccs.available());
  delay(500);
  //delay(5000);
}
 
void loop() {
  if (digitalRead(13) == HIGH)
    Serial.write("button");
    Serial.flush();
  
  if(ccs.available()){
    if(!ccs.readData()){
      //Serial.print("CO2: ");
      Serial.println(ccs.geteCO2());
      Serial.write(ccs.geteCO2());
      Serial.flush();
    }
    else{
      Serial.println("ERROR!");
      while(1);
    }
  }
  delay(500);
}
