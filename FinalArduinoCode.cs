#include "I2Cdev.h"
#include "MPU6050.h"
#include <HMC5883L.h>
#include "HMC5883L.h"
#include <SoftwareSerial.h>
SoftwareSerial bluetooth_port(10, 11);

#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
    #include "Wire.h"
#endif

MPU6050 accelgyro;
HMC5883L mag;

int16_t ax, ay, az;
int16_t mx, my, mz;
float calibrated_values[3]; 

#define OUTPUT_READABLE_ACCELGYRO
#define OUTPUT_READABLE_MAG

#define LED_PIN 13
bool blinkState = false;

void setup() {

    #if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
        Wire.begin();
    #elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
        Fastwire::setup(400, true);
    #endif
    
  Serial.begin(57600);
  while (!Serial) {
    ; 
  }

    accelgyro.initialize();
    mag.initialize();
 
    Serial.println(accelgyro.testConnection() ? "" : "");
    Serial.println(mag.testConnection() ? "" : "");

    Serial.println("Goodnight moon!");

 bluetooth_port.begin(9600);
 bluetooth_port.println("Hello, world?");
  

    pinMode(LED_PIN, OUTPUT);
}

void loop() {

    accelgyro.getAcceleration(&ax, &ay, &az);
  
   float values_from_magnetometer[3];
    mag.getHeading(&mx, &my, &mz);

     values_from_magnetometer[0] = mx;
  values_from_magnetometer[1] = my;
  values_from_magnetometer[2] = mz;
  transformation(values_from_magnetometer);

        #ifdef  OUTPUT_READABLE_MAG
         
          bluetooth_port.print(calibrated_values[0]); bluetooth_port.print(",");
          bluetooth_port.print(calibrated_values[1]); bluetooth_port.print(",");
          bluetooth_port.println(calibrated_values[2]);
          
        #endif

      #ifdef OUTPUT_READABLE_ACCELGYRO

   
        bluetooth_port.print(ax); bluetooth_port.print(","); 
        bluetooth_port.print(ay); bluetooth_port.print(",");
        bluetooth_port.print(az); bluetooth_port.print(",");
        delay(20);
        
       #endif
       
#ifdef OUTPUT_BINARY_MAG

   bluetooth_port.write((uint8_t)(calibrated_values[0] >> 8)); bluetooth_port.write((uint8_t)(mx & 0xFF));
        bluetooth_port.write((uint8_t)(calibrated_values[1] >> 8)); bluetooth_port.write((uint8_t)(my & 0xFF));
        bluetooth_port.write((uint8_t)(calibrated_values[2] >> 8)); bluetooth_port.write((uint8_t)(mz & 0xFF));
        
        #endif


    #ifdef OUTPUT_BINARY_ACCELGYRO 
  
        bluetooth_port.write((uint8_t)(ax >> 8)); bluetooth_port.write((uint8_t)(ax & 0xFF)); 
        bluetooth_port.write((uint8_t)(ay >> 8)); bluetooth_port.write((uint8_t)(ay & 0xFF));  
        bluetooth_port.write((uint8_t)(az >> 8)); bluetooth_port.write((uint8_t)(az & 0xFF));

    #endif

    float heading = atan2(my, mx);
   if(heading < 0)
      heading += 2 * M_PI;
     float headingDegrees = heading * 180/M_PI; 

    blinkState = !blinkState;
    digitalWrite(LED_PIN, blinkState);
}

void transformation(float uncalibrated_values[3])    
{

  double calibration_matrix[3][3] = 
  {
    {1.106, -0.02, 0.034},
    {0.035, 1.193, 0.088},
    {0.016, 0.013, 1.32}  
  };

  double bias[3] = 
  {
    67.363,
    -136.572,
    75.23
  };  

  for (int i=0; i<3; ++i) uncalibrated_values[i] = uncalibrated_values[i] - bias[i];
  float result[3] = {0, 0, 0};
  for (int i=0; i<3; ++i)
    for (int j=0; j<3; ++j)
      result[i] += calibration_matrix[i][j] * uncalibrated_values[j];
  for (int i=0; i<3; ++i) calibrated_values[i] = result[i];
}

