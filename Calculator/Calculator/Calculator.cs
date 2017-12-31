using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    enum OperationName { Equals, Divide, Multiply, Substract, Add, Pow};

    class Operation
    {
        public delegate double BinaryOperationMethod(double first, double second);
        private BinaryOperationMethod methodName;
        private string operationSign;

        internal BinaryOperationMethod MethodName { get => methodName; set => methodName = value; }
        public string OperationSign { get => operationSign; set => operationSign = value; }

        public Operation(BinaryOperationMethod methodName, string operationSign)
        {
            this.MethodName = methodName;
            this.OperationSign = operationSign;
        }
    }

    class Calc
    {
        private string outputResult= "0";
        private string currentNumber = "0";
        private string currentState;
        private double firstNumber;
        private double secondNumber;
        private bool firstNumberExist;
        private bool currentOperationIsNotOver;
        private bool divideByZeroException;
        private OperationName currentOperation = OperationName.Equals;

        public string OutputResult
        {
            get => outputResult;
            set
            {
                outputResult = value;
                currentNumber = outputResult;
            }
        }

        public string CurrentState { get => currentState; set => currentState = value; }

        private Dictionary<OperationName, Operation> operationList = new Dictionary<OperationName, Operation>();
        
        public Calc()
        {
            operationList.Add(OperationName.Equals, null);
            operationList.Add(OperationName.Add, new Operation(Add, "+"));
            operationList.Add(OperationName.Substract, new Operation(Substract, "-"));
            operationList.Add(OperationName.Multiply, new Operation(Multiply, "*"));
            operationList.Add(OperationName.Divide, new Operation(Divide, "/"));
            operationList.Add(OperationName.Pow, new Operation(Pow, "^"));
        }

        public void PerformOperation(string parameter)
        {
            switch (parameter)
            {
                case "/":
                    CheckAndPerform(OperationName.Divide);
                    break;
                case "=":
                    CheckAndPerform(OperationName.Equals);
                    break;
                case "*":
                    CheckAndPerform(OperationName.Multiply);
                    break;
                case "-":
                    CheckAndPerform(OperationName.Substract);
                    break;
                case "+":
                    CheckAndPerform(OperationName.Add);
                    break;
                case "%":
                    Percent();
                    break;
                case "pow":
                    CheckAndPerform(OperationName.Pow);
                    break;
                case "sqr":
                    SquareRoot();
                    break;
                case "C":
                    ResetAll();
                    break;
                case ",":
                    if (!CheckForComma())
                    {
                        OutputResult += parameter;
                    }
                    break;
                default:
                    if (CheckForZero())
                    {
                        OutputResult = parameter;
                    }
                    else if (!CheckLenghtOverflow())
                    {
                        OutputResult += parameter;
                    }
                    break;
            }
 
        }

        private bool CheckLenghtOverflow()
        {
            if (OutputResult.Length > 15)
            {
                return true;
            }
            return false;
        }

        private bool CheckForZero()
        {
            if (OutputResult == "0")
            {
                return true;
            }
            return false;
        }

        private bool CheckForComma()
        {
            for (int i = 0; i < outputResult.Length; i++)
            {
                if (outputResult[i] == ',')
                {
                    return true;
                }
            }
            return false;
        }

        private void Percent()
        {
            if (firstNumberExist)
            {
                secondNumber = firstNumber * (Convert.ToDouble(currentNumber) / 100);
                string state = firstNumber + " " + operationList[currentOperation].OperationSign + " " + secondNumber;
                CheckAndPerform(OperationName.Equals);
                CurrentState = state;
            }
        }

        private void ResetAll()
        {
            currentOperation = OperationName.Equals;
            firstNumber = 0;
            firstNumberExist = false;
            currentOperationIsNotOver = false;
            secondNumber = 0;
            OutputResult = "0";
            currentState = "";
            divideByZeroException = false;
        }

        private string ToStringWithCheckLenght(double numberToCheck)
        {
            if (numberToCheck.ToString().Length > 15)
            {
                if (numberToCheck > 999999999999999)
                    return numberToCheck.ToString("E10");
                else
                    return numberToCheck.ToString("G");
            }
            else
            {
                return numberToCheck.ToString();
            }
        }

        private double Add(double first, double second)
        {
            return (first + second);
        }

        private double Substract(double first, double second)
        {
            return (first - second);
        }

        private double Multiply(double first, double second)
        {
            return (first * second);
        }

        private double Divide(double first, double second)
        {
            if(second == 0)
            {
                divideByZeroException = true;
                return 0;
            }
            return (first / second);
        }

        private double Pow(double number, double rise)
        {
            return (Math.Pow(number, rise));
        }

        private void SquareRoot()
        {
            if (!divideByZeroException)
            {
                CurrentState = "sqrt(" + OutputResult +")";
                OutputResult = ToStringWithCheckLenght(Math.Sqrt(Convert.ToDouble(outputResult)));
            }
        }

        private void CheckAndPerform(OperationName selectedOperation)
        {
            if (divideByZeroException)
            {
                return;
            }

            if (currentOperationIsNotOver)
            {
                currentOperationIsNotOver = false;
                CheckAndPerform(currentOperation);
            }

            if (selectedOperation == OperationName.Equals)
            {
                currentOperation = OperationName.Equals;
                return;
            }

            currentOperation = selectedOperation;
            if (!firstNumberExist)
            {
                Console.WriteLine(outputResult);
                firstNumber = Convert.ToDouble(outputResult); //System.Globalization.CultureInfo.InvariantCulture.NumberFormat
                currentState = outputResult + " " + operationList[currentOperation].OperationSign;
                OutputResult = "0";
                firstNumberExist = true;
                currentOperationIsNotOver = true;
            }
            else
            {
                secondNumber = Convert.ToDouble(outputResult);
                double result = operationList[currentOperation].MethodName(firstNumber, secondNumber);
                OutputResult = ToStringWithCheckLenght(result);
                currentState = "";
                currentNumber = "0";
                firstNumber = 0;
                firstNumberExist = false;
                if (divideByZeroException)
                {
                    currentState = "Divide by Zero is";
                    outputResult = "IMPOSIBURU!!!";
                }
            }
        }
    }
}
