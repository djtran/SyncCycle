

int potPin = A2;    // select the input pin for the potentiometer
int ledPin = 6;   // select the pin for the LED
int led2 = 10;
int val = 0;       // variable to store the value coming from the sensor
int reedVal = 0;
int ccVal = 0;

int lock;
int ccLastInput;
long ccCurrTime;
long ccPrevTime;

double Kp = 3;
double error;
double Ki = 5; 
double integral;
double Kd = 1;
double derivative;
double preError;
double Dt;

int currOutput = 0;
double correction = 0;

float setPoint;
int scaledOut = 100 + (int)((setPoint + 6.308) / 0.124);

int reedSwitch = A4;
int ccSwitch = A5;
int buttonState = 0;

int motorPin = 3;

long int timeout = 0;
int incomingByte = 0;

long curr_time = 0;         // the last time the output pin was toggled
long prev_time = 0;
long debounce = 20;   // the debounce time, increase if the output flickers
long dur_on = 0;    //the duration between ON/OFF

float duration;
float curr_speed;
float prev_speed;

float p1 = 0;
float p2 = 0;
float p3 = 0;

int state = 0;      // the current state of the output pin
int reading;           // the current reading from the input pin
int previous = 0;    // the previous reading from the input pin
int cc_reading = 0;

float fps_mph = (float)((float)3600/(float)5280);

void setup() {
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);  // declare the ledPin as an OUTPUT
  pinMode(led2, OUTPUT);
  pinMode(reedSwitch, INPUT);
  pinMode(motorPin, OUTPUT);
  pinMode(ccSwitch, INPUT);

  lock = 0;
  setPoint = 0;

  //Serial.println(fps_mph, 4);
  Setpoint = (double)setPoint;

  myPID.SetMode(AUTOMATIC);
}

void loop() {

  timeout++;

  if (timeout > 15000) {
    timeout = 0;
    Serial.print(0.00, 2); Serial.println(" mph!!");
    prev_time = curr_time;
    curr_speed = 0.00;
    return;
  }
  //Serial.println(ccVal);
  
  reedVal = analogRead(reedSwitch);
  //Serial.println(reedVal);
  if (reedVal > 1010) {
    reading = 1;
    digitalWrite(ledPin, HIGH);

    if (previous <= 1010 && timeout > 500) {
      timeout = 0;
      prev_speed = curr_speed;
      p1 = p2;
      p2 = p3;
      
      curr_time = millis();
      duration = (float)(curr_time - prev_time) / 1000;
      curr_speed = ((float)(6.725) / duration) * fps_mph;
      if (curr_speed > (prev_speed + 20) || curr_speed < (prev_speed - 20)) {
        curr_speed = prev_speed;
      }
      p3 = curr_speed;
      curr_speed = (p1 + p2 + p3) / (float)(3);

      Serial.print(curr_speed, 4); Serial.print(" mph");
      Serial.print(" MOP ");Serial.print(currOutput);
      Serial.print(" SET ");Serial.print(setPoint);
      Serial.print(" ERR ");Serial.println(error);
    }
  } else {
    reading = 0;
    digitalWrite(ledPin, LOW);
  }

  prev_time = curr_time;
  previous = reedVal;

  ccLastInput = ccVal;

  ccPrevTime = ccCurrTime;
  

  ccVal = analogRead(ccSwitch);

  if (ccVal > 600) {
    ccCurrTime = millis();
    if (ccLastInput <= 600 && (ccCurrTime - ccPrevTime > 200)) {
      setPoint = curr_speed;
    }
    
    digitalWrite(led2, HIGH);
    //CRUISE CONTROL

    error = setPoint - curr_speed; 

    ccLastInput = currOutput;
    
    if (preError - error < 0 || preError + error > 5) {
      currOutput = ccLastInput + (int)(20 * (setPoint/20)); 
    } else {
      currOutput = scaledOut + (int)(Kp * error) + (int)((preError + error) * 2);
    }

    if (abs(ccLastInput - currOutput) > 20) {
      currOutput = (int)((setPoint + 6.308) / 0.124);
    }
    
    if (currOutput > 255) { currOutput = 255;}
    if (curr_speed < 1) { currOutput = 0;}
    
    if (error < 0) {
      analogWrite(motorPin, 0);
    } else {
      analogWrite(motorPin, currOutput);
    }
    // remember the error for the next time around.
    preError = error;


  } else {
    digitalWrite(led2, LOW);
    val = analogRead(potPin);

    analogWrite(motorPin, val / 4);
  }


}