using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    enum Operation {Idle, Divide, Equals, Multiply, Substract, Add};

    class Calc
    {
        private string currentNumber = "0";
        private double firstNumber;
        private double secondNumber;
        private bool firstNumberExist;
        private Operation currentOperation = Operation.Idle;

        public string CurrentNumber { get => currentNumber; set => currentNumber = value; }

        public void PerformOperation(string parameter)
        {
            switch (parameter)
            {
                case "/":
                    //DivideMethod
                    break;
                case "=":
                    if(currentOperation == Operation.Add)
                    {
                        PerformOperation("+");
                    }
                    break;
                case "*":
                    //Multiply
                    break;
                case "-":
                    //Substract
                    break;
                case "+":
                    currentOperation = Operation.Add;
                    if (!firstNumberExist)
                    {
                        firstNumber = Convert.ToDouble(currentNumber);
                        currentNumber = "0";
                        firstNumberExist = true;
                    }
                    else
                    {
                        secondNumber = Convert.ToDouble(currentNumber);
                        currentNumber = Add(firstNumber, secondNumber);
                    }

                    break;
                case "C":
                    currentOperation = Operation.Idle;
                    firstNumber = 0;
                    firstNumberExist = false;
                    secondNumber = 0;
                    currentNumber = "0";
                    break;
                case "0":
                    if (currentNumber == "0")
                    {
                        break;
                    }
                    if(currentNumber.Length > 15)
                    {
                        break;
                    }
                    currentNumber += parameter;
                    break;
                default:
                    if (currentNumber == "0")
                    {
                        currentNumber = parameter;
                        break;
                    }
                    if (currentNumber.Length > 15)
                    {
                        break;
                    }
                    currentNumber += parameter;
                    break;
            }
 
        }
        private string Add(double first, double second)
        {
            return (first + second).ToString();
        }
    }
}
