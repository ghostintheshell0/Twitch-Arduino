int led = 13; 
bool enabled = false;

void setup() {
  Serial.begin(9600);
  pinMode(led, OUTPUT);
}

void loop()
{
  char incomingChar;
  
  if (Serial.available() > 0)
  {
    incomingChar = Serial.read();  

    switch (incomingChar) 
    {
      case 'n':
        ON();
        break;
      case 'f':
        OFF();
        break;
      case 's':
        SWITCH();
        break;
    }
  }   
  delay(300);             
}

void ON()
{
	digitalWrite(led, HIGH);
	enabled = true;
}
void OFF()
{
	digitalWrite(led, LOW);
	enabled = false;
}

void SWITCH()
{
	if(enabled)
	{
		OFF();
	}
	else
	{
		ON();	
	}
}